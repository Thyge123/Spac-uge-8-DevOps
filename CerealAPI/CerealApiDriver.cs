using CerealAPI.Model;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CerealAPI
{
        public class CerealApiDriver
        {
            private readonly HttpClient _client;
            private string _token;

            public CerealApiDriver(string baseUrl)
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
                var response = await _client.PostAsJsonAsync("/api/auth/login", new { Username = username, Password = password });
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _token = authResponse.Token;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                Console.WriteLine("Login successful, authentication token received.");
            }
    

            // Get Methods
            public async Task GetAllCerealsAsync()
            {
                var response = await _client.GetAsync("/api/products/cereals");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var cereals = JsonSerializer.Deserialize<List<Cereal>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Console.WriteLine($"Retrieved {cereals.Count} cereals:");
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
                    Console.WriteLine($"Retrieved cereal by ID: {cereal.Name} (ID: {cereal.Id})");
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
                    Console.WriteLine($"Retrieved cereal by name: {cereal.Name} (ID: {cereal.Id})");
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

                    Console.WriteLine($"Available cereal images ({imageNames.Count}):");
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
            public async Task AddCerealAsync(Cereal cereal)
            {
                var response = await _client.PostAsJsonAsync("/api/products/cereal/add", cereal);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var addedCereal = JsonSerializer.Deserialize<Cereal>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Console.WriteLine($"Cereal added successfully: {addedCereal.Name} (ID: {addedCereal.Id})");
                }
                else
                {
                    Console.WriteLine($"Error adding cereal: {response.StatusCode}");
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Details: {error}");
                }
            }

            public async Task AddFromFileAsync()
            {
                var response = await _client.PostAsync("/api/products/cereal/file/add", null);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Cereals added from file successfully");
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
                    Console.WriteLine($"Cereal ID {cereal.Id} updated successfully");
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
                    Console.WriteLine($"Cereal ID {id} deleted successfully");
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

                    Console.WriteLine($"Retrieved {cereals.Count} filtered cereals:");
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

                    Console.WriteLine($"Retrieved {cereals.Count} sorted cereals by {sortBy} ({sortOrder}):");
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

        // Simple class to hold auth response
        public class AuthResponse
        {
            public string Token { get; set; }
        }

        // Demo program
        public class Program
        {
            public static async Task Main()
            {
                Console.WriteLine("CerealAPI Demo Starting...");
                var driver = new CerealApiDriver("https://localhost:7226");

                try
                {
                    // Anonymous user operations
                    Console.WriteLine("\n--- Anonymous User Operations ---");
                    await driver.GetAllCerealsAsync();
                    await driver.GetCerealByIdAsync(1);
                    await driver.GetCerealByNameAsync("Cheerios");
                    await driver.GetFilteredCerealsAsync(minCalories: 100, manufacturer: "K");
                    await driver.GetSortedCerealsAsync(sortBy: "Calories", sortOrder: "asc");
                    await driver.GetCerealImagesAsync();

                    // Admin operations (requires login)
                    Console.WriteLine("\n--- Admin Operations (after login) ---");
                    await driver.LoginAsync("admin", "admin123");

                    // Add a new cereal
                    var newCereal = new Cereal
                    {
                        Name = "Test Cereal",
                        Mfr = "K",
                        Type = "C",
                        Calories = 110,
                        Protein = 2,
                        Fat = 0,
                        Sodium = 200,
                        Fiber = 1.0f,
                        Carbo = 22.0f,
                        Sugars = 3,
                        Potass = 95,
                        Vitamins = 25,
                        Shelf = 3,
                        Weight = 1.0f,
                        Cups = 0.75f,
                        Rating = 42.5f
                    };
                    await driver.AddCerealAsync(newCereal);

                    // Update a cereal
                    var cerealToUpdate = new Cereal
                    {
                        Id = 1,
                        Name = "Updated Cereal",
                        Mfr = "G",
                        Type = "C",
                        Calories = 120,
                        Protein = 3,
                        Fat = 1,
                        Sodium = 210,
                        Fiber = 2.0f,
                        Carbo = 21.0f,
                        Sugars = 5,
                        Potass = 100,
                        Vitamins = 25,
                        Shelf = 3,
                        Weight = 1.0f,
                        Cups = 0.75f,
                        Rating = 44.5f
                    };
                    await driver.UpdateCerealAsync(cerealToUpdate);

                    // Delete a cereal (assuming we know the ID of the one we just added)
                    // You would need to get the ID from the AddCereal response in a real application
                    await driver.DeleteCerealAsync(77);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                Console.WriteLine("\nCerealAPI Demo Completed");
            }
        }
}
