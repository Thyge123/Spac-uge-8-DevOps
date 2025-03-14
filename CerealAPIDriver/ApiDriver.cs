using CerealAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CerealAPIDriver
{
    public class ApiDriver
    {
        private readonly HttpClient _client;
        private string _token;

        public ApiDriver(string baseUrl)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //Authentication
        public async Task LoginAsync(string username, string password)
        {
            var response = await _client.PostAsJsonAsync("/login", new { Username = username, Password = password });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            _token = authResponse.Token;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine("\nLogin successful, authentication token received.");
        }

        // Get Methods
        public async Task GetAllCerealsAsync()
        {
            var response = await _client.GetAsync("/api/products/cereals");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var cereals = JsonSerializer.Deserialize<List<Cereal>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Console.WriteLine($"\nRetrieved {cereals.Count} cereals:");
            foreach (var cereal in cereals.Take(5))
            {
                Console.WriteLine($"- {cereal.Name} (Rating: {cereal.Rating})");
            }
            if (cereals.Count > 5) Console.WriteLine("... (more items)");
        }

        public async Task GetCerealByIdAsync(int id)
        {
            var response = await _client.GetAsync($"/api/products/cereal/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cereal = JsonSerializer.Deserialize<Cereal>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Console.WriteLine($"\nRetrieved cereal by ID: {cereal.Name} (ID: {cereal.Id})");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Cereal with ID {id} not found.");
            }
            else
            {
                Console.WriteLine($"Error retrieving cereal: {response.StatusCode}");
            }
        }

        public async Task GetCerealByNameAsync(string name)
        {
            var response = await _client.GetAsync($"/api/products/cereal/{Uri.EscapeDataString(name)}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cereal = JsonSerializer.Deserialize<Cereal>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Console.WriteLine($"\nRetrieved cereal by name: {cereal.Name} (ID: {cereal.Id})");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Cereal '{name}' not found.");
            }
            else
            {
                Console.WriteLine($"Error retrieving cereal: {response.StatusCode}");
            }
        }

        public async Task GetCerealImagesAsync()
        {
            var response = await _client.GetAsync("/api/products/cereal/images");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var imageNames = JsonSerializer.Deserialize<List<string>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Console.WriteLine($"\nAvailable cereal images ({imageNames.Count}):");
                foreach (var name in imageNames.Take(5))
                {
                    Console.WriteLine($"- {name}");
                }
                if (imageNames.Count > 5) Console.WriteLine("... (more images)");
            }
            else
            {
                Console.WriteLine($"Error retrieving cereal images: {response.StatusCode}");
            }
        }

        // Post/Put/Delete Methods (Admin Only)
        public async Task<Cereal> AddCerealAsync(Cereal cereal)
        {
            var response = await _client.PostAsJsonAsync("/api/products/cereal/add", cereal);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var addedCereal = JsonSerializer.Deserialize<Cereal>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Console.WriteLine($"\nCereal added successfully: {addedCereal.Name} (ID: {addedCereal.Id})");
            return addedCereal;  
        }

        public async Task AddFromFileAsync()
        {
            var response = await _client.PostAsync("/api/products/cereal/file/add", null);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("\nCereals added from file successfully");
            }
            else
            {
                Console.WriteLine($"Error adding cereals from file: {response.StatusCode}");
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Details: {error}");
            }
        }

        public async Task UpdateCerealAsync(Cereal cereal)
        {
            var response = await _client.PutAsJsonAsync("/api/products/cereal/update", cereal);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"\nCereal ID {cereal.Id} updated successfully");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Cereal ID {cereal.Id} not found for update");
            }
            else
            {
                Console.WriteLine($"Error updating cereal: {response.StatusCode}");
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Details: {error}");
            }
        }

        public async Task DeleteCerealAsync(int id)
        {
            var response = await _client.DeleteAsync($"/api/products/cereal/delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"\nCereal ID {id} deleted successfully");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Cereal ID {id} not found for deletion");
            }
            else
            {
                Console.WriteLine($"Error deleting cereal: {response.StatusCode}");
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Details: {error}");
            }
        }

        // Filtered Queries
        public async Task GetFilteredCerealsAsync(int? minCalories = null, string manufacturer = null, float? minRating = null)
        {
            var queryParams = new List<string>();

            if (minCalories.HasValue)
                queryParams.Add($"Calories={minCalories}");

            if (!string.IsNullOrEmpty(manufacturer))
                queryParams.Add($"Manufacturer={Uri.EscapeDataString(manufacturer)}");

            if (minRating.HasValue)
                queryParams.Add($"Rating={minRating}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";

            var response = await _client.GetAsync($"/api/products/cereals{queryString}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cereals = JsonSerializer.Deserialize<List<Cereal>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Console.WriteLine($"\nRetrieved {cereals.Count} filtered cereals (minCalories: 100, manufacturer: K): ");
                foreach (var cereal in cereals.Take(5))
                {
                    Console.WriteLine($"- {cereal.Name} (Rating: {cereal.Rating}, Calories: {cereal.Calories}, Mfr: {cereal.Mfr})");
                }
                if (cereals.Count > 5) Console.WriteLine("... (more items)");
            }
            else
            {
                Console.WriteLine($"Error retrieving filtered cereals: {response.StatusCode}");
            }
        }

        public async Task GetSortedCerealsAsync(string sortBy = "Rating", string sortOrder = "desc")
        {
            var queryString = $"?SortBy={Uri.EscapeDataString(sortBy)}&SortOrder={Uri.EscapeDataString(sortOrder)}";

            var response = await _client.GetAsync($"/api/products/cereals{queryString}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cereals = JsonSerializer.Deserialize<List<Cereal>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Console.WriteLine($"\nRetrieved {cereals.Count} sorted cereals by {sortBy} ({sortOrder}):");
                foreach (var cereal in cereals.Take(5))
                {
                    Console.WriteLine($"- {cereal.Name} (Rating: {cereal.Rating}, Calories: {cereal.Calories})");
                }
                if (cereals.Count > 5) Console.WriteLine("... (more items)");
            }
            else
            {
                Console.WriteLine($"Error retrieving sorted cereals: {response.StatusCode}");
            }
        }
    }
}