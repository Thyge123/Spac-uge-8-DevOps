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

        public Task<List<Cereal>> GetAllAsync(CerealFilterModel filter)
        {
            var query = _dbContext.Cereals.AsQueryable();

            // Combine all integer filters
            if (filter.Calories.HasValue) query = query.Where(c => c.Calories == filter.Calories.Value);
            if (filter.Protein.HasValue) query = query.Where(c => c.Protein == filter.Protein.Value);
            if (filter.Fat.HasValue) query = query.Where(c => c.Fat == filter.Fat.Value);
            if (filter.Sodium.HasValue) query = query.Where(c => c.Sodium == filter.Sodium.Value);
            if (filter.Sugars.HasValue) query = query.Where(c => c.Sugars == filter.Sugars.Value);
            if (filter.Potass.HasValue) query = query.Where(c => c.Potass == filter.Potass.Value);
            if (filter.Vitamins.HasValue) query = query.Where(c => c.Vitamins == filter.Vitamins.Value);

            // Combine all float filters
            if (filter.Fiber.HasValue) query = query.Where(c => c.Fiber == filter.Fiber.Value);
            if (filter.Carbo.HasValue) query = query.Where(c => c.Carbo == filter.Carbo.Value);
            if (filter.Shelf.HasValue) query = query.Where(c => c.Shelf == filter.Shelf.Value);
            if (filter.Weight.HasValue) query = query.Where(c => c.Weight == filter.Weight.Value);
            if (filter.Cups.HasValue) query = query.Where(c => c.Cups == filter.Cups.Value);
            if (filter.Rating.HasValue) query = query.Where(c => c.Rating == filter.Rating.Value);

            // Handle manufacturer filter with name-to-code mapping
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

            // Execute the query once with all filters applied
            return query.ToListAsync();
        }

        private string GetManufacturerCode(string manufacturerName)
        {
            // Case-insensitive comparison
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


        public Task<Cereal> Get(int id)
        {
            return _dbContext.Cereals.FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<Cereal> GetByName(string name)
        {
            return _dbContext.Cereals.FirstOrDefaultAsync(c => c.Name == name);
        }

        public Task<Cereal> Add(Cereal cereal)
        {
            _dbContext.Cereals.Add(cereal);
            _dbContext.SaveChanges();
            return Get(cereal.Id);
        }

        public void AddFromFile()
        {
            var parser = new CerealParser("C:\\Users\\spac-25\\source\\repos\\CerealAPI\\CerealAPI\\Cereal.csv", this);
            parser.Parse();
        }

        public Task<Cereal> Update(Cereal cereal)
        {
            _dbContext.Cereals.Update(cereal);
            _dbContext.SaveChanges();
            return Get(cereal.Id);
        }

        public Task<Cereal> Delete(int id)
        {
            var cereal = _dbContext.Cereals.FirstOrDefault(c => c.Id == id);
            if (cereal != null)
            {
                _dbContext.Cereals.Remove(cereal);
                _dbContext.SaveChanges();
            }
            return Task.FromResult(cereal);
        }
    }
}