using CerealAPI.DbContext;
using CerealAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CerealAPI.Manager
{
    public class CerealManager
    {
        public readonly DBContext _dbContext;

        public CerealManager(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        // Get all cereals from the database
        public async Task<List<Cereal>> GetAllAsync(CerealFilterModel filter, CerealSortModel sort)
        {
            try
            {
                var query = _dbContext.Cereals.AsQueryable(); // Get all cereals from the database

                // Apply filters
                query = ApplyFilters(query, filter); // Apply filters to the query

                // Apply sorting
                query = ApplySorting(query, sort); // Apply sorting to the query

                return await query.ToListAsync(); // Return the list of cereals after applying filters and sorting
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving cereals: {ex.Message}");
                return new List<Cereal>();
            }
        }

        // Apply filters to the query
        private IQueryable<Cereal> ApplyFilters(IQueryable<Cereal> query, CerealFilterModel filter)
        {
            if (filter == null) return query; // If no filter is specified, return the query as is

            // if the filter has a value, apply the filter to the query
            if (filter.Calories.HasValue) query = query.Where(c => c.Calories == filter.Calories.Value); 
            if (filter.Protein.HasValue) query = query.Where(c => c.Protein == filter.Protein.Value);
            if (filter.Fat.HasValue) query = query.Where(c => c.Fat == filter.Fat.Value);
            if (filter.Sodium.HasValue) query = query.Where(c => c.Sodium == filter.Sodium.Value);
            if (filter.Sugars.HasValue) query = query.Where(c => c.Sugars == filter.Sugars.Value);
            if (filter.Potass.HasValue) query = query.Where(c => c.Potass == filter.Potass.Value);
            if (filter.Vitamins.HasValue) query = query.Where(c => c.Vitamins == filter.Vitamins.Value);
            if (filter.Fiber.HasValue) query = query.Where(c => c.Fiber == filter.Fiber.Value);
            if (filter.Carbo.HasValue) query = query.Where(c => c.Carbo == filter.Carbo.Value);
            if (filter.Shelf.HasValue) query = query.Where(c => c.Shelf == filter.Shelf.Value);
            if (filter.Weight.HasValue) query = query.Where(c => c.Weight == filter.Weight.Value);
            if (filter.Cups.HasValue) query = query.Where(c => c.Cups == filter.Cups.Value);
            if (filter.Rating.HasValue) query = query.Where(c => c.Rating == filter.Rating.Value);

            // String filters
            if (!string.IsNullOrEmpty(filter.Manufacturer))
            {
                string manufacturerCode = GetManufacturerCode(filter.Manufacturer); // Get the manufacturer code
                if (!string.IsNullOrEmpty(manufacturerCode)) // If the manufacturer code is not empty
                {
                    query = query.Where(c => c.Mfr == manufacturerCode); // Apply the filter to the query
                }
            }

            if (!string.IsNullOrEmpty(filter.Type)) // If the type filter is not empty
            {
                query = query.Where(c => EF.Functions.Like(c.Type.ToLower(), filter.Type.ToLower())); // Apply the filter to the query
            }

            return query; // Return the query with the applied filters
        }

        // Apply sorting to the query
        private IQueryable<Cereal> ApplySorting(IQueryable<Cereal> query, CerealSortModel sort)
        {
            if (sort?.SortBy == null) return query; // If no sorting is specified, return the query as is

            string sortByLower = sort.SortBy.ToLower(); // Normalize the sort by value
            bool isAscending = (sort.SortOrder?.ToLower() ?? "asc") == "asc"; // Normalize the sort order value (default is ascending)

            return sortByLower switch
            {
                "name" => isAscending ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
                "calories" => isAscending ? query.OrderBy(c => c.Calories) : query.OrderByDescending(c => c.Calories),
                "protein" => isAscending ? query.OrderBy(c => c.Protein) : query.OrderByDescending(c => c.Protein),
                "fat" => isAscending ? query.OrderBy(c => c.Fat) : query.OrderByDescending(c => c.Fat),
                "sodium" => isAscending ? query.OrderBy(c => c.Sodium) : query.OrderByDescending(c => c.Sodium),
                "fiber" => isAscending ? query.OrderBy(c => c.Fiber) : query.OrderByDescending(c => c.Fiber),
                "carbo" => isAscending ? query.OrderBy(c => c.Carbo) : query.OrderByDescending(c => c.Carbo),
                "sugars" => isAscending ? query.OrderBy(c => c.Sugars) : query.OrderByDescending(c => c.Sugars),
                "potass" => isAscending ? query.OrderBy(c => c.Potass) : query.OrderByDescending(c => c.Potass),
                "vitamins" => isAscending ? query.OrderBy(c => c.Vitamins) : query.OrderByDescending(c => c.Vitamins),
                "shelf" => isAscending ? query.OrderBy(c => c.Shelf) : query.OrderByDescending(c => c.Shelf),
                "weight" => isAscending ? query.OrderBy(c => c.Weight) : query.OrderByDescending(c => c.Weight),
                "cups" => isAscending ? query.OrderBy(c => c.Cups) : query.OrderByDescending(c => c.Cups),
                "rating" => isAscending ? query.OrderBy(c => c.Rating) : query.OrderByDescending(c => c.Rating),
                _ => query
            };
        }


        // Get the manufacturer code for a given manufacturer name 
        private string GetManufacturerCode(string manufacturerName)
        {
            string normalizedName = manufacturerName.Trim().ToLowerInvariant(); // Normalize the name
            // Return the manufacturer code based on the normalized name
            return normalizedName switch
            {
                var n when n.Contains("american home") => "A",
                var n when n.Contains("general mills") => "G",
                var n when n.Contains("kellogg") => "K",
                var n when n.Contains("nabisco") => "N",
                var n when n.Contains("post") => "P",
                var n when n.Contains("quaker") => "Q",
                var n when n.Contains("ralston") || n.Contains("purina") => "R",
                _ => manufacturerName.Length == 1 ? manufacturerName.ToUpperInvariant() : string.Empty // Return the name in uppercase if it's a single character
            };
        }

        // Get a cereal by its ID
        public Task<Cereal?> GetById(int id)
        {
            try
            {
                return _dbContext.Cereals.FirstOrDefaultAsync(c => c.Id == id); // Find the cereal with the given ID
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving cereal: {ex.Message}");
                return null;
            }
        }

        // Get a cereal by its name
        public Task<Cereal?> GetByName(string name)
        {
            try
            {
                return _dbContext.Cereals.FirstOrDefaultAsync(c => c.Name == name); // Find the cereal with the given name
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving cereal: {ex.Message}");
                return null;
            }
        }

        // Get all pictures from the pictures folder
        public async Task<List<string>> GetPicturesAsync()
        {
            try
            {
                var pictures = new List<string>(); // List to store the paths of the pictures
                var path = Path.Combine(Directory.GetCurrentDirectory(), "pictures"); // Path to the pictures folder

                if (Directory.Exists(path)) // If the folder exists
                { 
                    var files = await Task.Run(() => Directory.GetFiles(path)); // Get all files in the folder
                    pictures.AddRange(files); // Add the files to the list
                }
                return pictures;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving images: {ex.Message}");
                return new List<string>();
            }
        }

        // Get the picture of a product by its name
        public async Task<byte[]?> GetPictureOfProductByName(string name)
        {
            try
            {
                var files = await GetPicturesAsync();

                string fileName = name.Replace(" ", "").ToLowerInvariant(); // Normalize the name

                foreach (var file in files)
                {
                    string fileNameWithoutPath = Path.GetFileNameWithoutExtension(file).Replace(" ", "").ToLowerInvariant(); // Normalize the file name

                    if (fileNameWithoutPath.Contains(fileName) || fileName.Contains(fileNameWithoutPath)) // Check if the file name contains the product name or vice versa
                    {
                        return await File.ReadAllBytesAsync(file); // Return the image as a byte array
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving image: {ex.Message}");
                return null;
            }
        }

        // Add a new cereal to the database
        public async Task<Cereal?> Add(Cereal cereal)
        {
            try
            {
                _dbContext.Cereals.Add(cereal); // Add the cereal to the database
                await _dbContext.SaveChangesAsync(); // Save changes
                return cereal; // Return the added cereal
            }
            catch (DbUpdateException ex)
            {
                throw new Exception ("Failed to create cereal due to database error", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding cereal", ex);
            }
        }

        // Add cereals from a CSV file
        public async void AddFromFile()
        {
            try
            {
                var parser = new CerealParser("C:\\Users\\spac-25\\source\\repos\\CerealAPI\\CerealAPI\\Cereal.csv", this); // Create a new parser
                await parser.ParseAsync(); // Parse the file
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding cereals from file: {ex.Message}");
            }
        }

        // Update a cereal in the database
        public Task<Cereal?> Update(Cereal cereal)
        {
            try
            {
                _dbContext.Cereals.Update(cereal); // Update the cereal
                _dbContext.SaveChanges(); // Save changes
                return GetById(cereal.Id); // Return the updated cereal
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to update cereal due to database error", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating cereal", ex);
            }
        }

        // Delete a cereal from the database
        public Task<Cereal?> Delete(int id)
        {
            try
            {
                var cereal = _dbContext.Cereals.FirstOrDefault(c => c.Id == id); // Find the cereal with the given ID
                if (cereal != null) // If the cereal exists
                {
                    _dbContext.Cereals.Remove(cereal); // Remove the cereal
                    _dbContext.SaveChanges(); // Save changes
                }
                return Task.FromResult(cereal); 
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting cereal", ex);
            }
        }
    }
}