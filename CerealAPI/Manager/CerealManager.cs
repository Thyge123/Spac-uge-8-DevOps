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

        public Task<List<Cereal>> GetAllAsync(CerealFilterModel filter, CerealSortModel sort)
        {
            try
            {
                var query = _dbContext.Cereals.AsQueryable();

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

                if (!string.IsNullOrEmpty(filter.Manufacturer))
                {
                    string manufacturerCode = GetManufacturerCode(filter.Manufacturer);
                    if (!string.IsNullOrEmpty(manufacturerCode))
                    {
                        query = query.Where(c => c.Mfr == manufacturerCode);
                    }
                }
                if (!string.IsNullOrEmpty(filter.Type))
                {
                    query = query.Where(c => EF.Functions.Like(c.Type.ToLower(), filter.Type.ToLower()));
                }

                if (sort?.SortBy != null)
                {
                    string sortByLower = sort.SortBy.ToLower();
                    string sortOrderLower = sort.SortOrder?.ToLower() ?? "asc";

                    switch (sortByLower)
                    {
                        case "name":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name);
                            break;
                        case "calories":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Calories) : query.OrderByDescending(c => c.Calories);
                            break;
                        case "protein":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Protein) : query.OrderByDescending(c => c.Protein);
                            break;
                        case "fat":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Fat) : query.OrderByDescending(c => c.Fat);
                            break;
                        case "sodium":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Sodium) : query.OrderByDescending(c => c.Sodium);
                            break;
                        case "fiber":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Fiber) : query.OrderByDescending(c => c.Fiber);
                            break;
                        case "carbo":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Carbo) : query.OrderByDescending(c => c.Carbo);
                            break;
                        case "sugars":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Sugars) : query.OrderByDescending(c => c.Sugars);
                            break;
                        case "potass":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Potass) : query.OrderByDescending(c => c.Potass);
                            break;
                        case "vitamins":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Vitamins) : query.OrderByDescending(c => c.Vitamins);
                            break;
                        case "shelf":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Shelf) : query.OrderByDescending(c => c.Shelf);
                            break;
                        case "weight":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Weight) : query.OrderByDescending(c => c.Weight);
                            break;
                        case "cups":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Cups) : query.OrderByDescending(c => c.Cups);
                            break;
                        case "rating":
                            query = sortOrderLower == "asc" ? query.OrderBy(c => c.Rating) : query.OrderByDescending(c => c.Rating);
                            break;
                    }
                }

                return query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving cereals: {ex.Message}");
                return Task.FromResult<List<Cereal>>(new List<Cereal>());
            }
        }


        private string GetManufacturerCode(string manufacturerName)
        {
            string normalizedName = manufacturerName.Trim().ToLowerInvariant();

            return normalizedName switch
            {
                var n when n.Contains("american home") => "A",
                var n when n.Contains("general mills") => "G",
                var n when n.Contains("kellogg") => "K",
                var n when n.Contains("nabisco") => "N",
                var n when n.Contains("post") => "P",
                var n when n.Contains("quaker") => "Q",
                var n when n.Contains("ralston") || n.Contains("purina") => "R",
                _ => manufacturerName.Length == 1 ? manufacturerName.ToUpperInvariant() : string.Empty
            };
        }

        public Task<Cereal?> GetById(int id)
        {
            try
            {
                return _dbContext.Cereals.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving cereal: {ex.Message}");
                return null;
            }
        }

        public Task<Cereal?> GetByName(string name)
        {
            try
            {
                return _dbContext.Cereals.FirstOrDefaultAsync(c => c.Name == name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving cereal: {ex.Message}");
                return null;
            }
        }

        public async Task<List<string>> GetPicturesAsync()
        {
            try
            {
                var pictures = new List<string>();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "pictures");

                if (Directory.Exists(path))
                {
                    var files = await Task.Run(() => Directory.GetFiles(path));
                    pictures.AddRange(files);
                }
                return pictures;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving images: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<byte[]?> GetPictureOfProductByName(string name)
        {
            try
            {
                var files = await GetPicturesAsync();

                string fileName = name.Replace(" ", "").ToLowerInvariant();

                foreach (var file in files)
                {
                    string fileNameWithoutPath = Path.GetFileNameWithoutExtension(file).Replace(" ", "").ToLowerInvariant();

                    if (fileNameWithoutPath.Contains(fileName) || fileName.Contains(fileNameWithoutPath))
                    {
                        return await File.ReadAllBytesAsync(file);
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

        public async Task<Cereal?> Add(Cereal cereal)
        {
            try
            {
                _dbContext.Cereals.Add(cereal);
                await _dbContext.SaveChangesAsync();
                return cereal;
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

        public async void AddFromFile()
        {
            try
            {
                var parser = new CerealParser("C:\\Users\\spac-25\\source\\repos\\CerealAPI\\CerealAPI\\Cereal.csv", this);
                await parser.ParseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding cereals from file: {ex.Message}");
            }
        }

        public Task<Cereal?> Update(Cereal cereal)
        {
            try
            {
                _dbContext.Cereals.Update(cereal);
                _dbContext.SaveChanges();
                return GetById(cereal.Id);
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

        public Task<Cereal?> Delete(int id)
        {
            try
            {
                var cereal = _dbContext.Cereals.FirstOrDefault(c => c.Id == id);
                if (cereal != null)
                {
                    _dbContext.Cereals.Remove(cereal);
                    _dbContext.SaveChanges();
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