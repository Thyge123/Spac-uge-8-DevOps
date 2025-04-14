using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using CerealAPI.Manager;
using CerealAPI.Model;
using CerealAPI.DbContext;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Moq;
using System;

namespace CerealAPI_Tests
{
    [TestClass]
    public class CerealManagerTests
    {
        private DBContext _dbContext;
        private CerealManager _cerealManager;

        [TestInitialize]
        public void Initialize()
        {
            // Use in-memory database for testing
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: $"TestCerealDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new DBContext(options);
            _cerealManager = new CerealManager(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task GetAllAsync_WithNoFiltersAndSort_ReturnsAllCereals()
        {
            // Arrange
            await _dbContext.Cereals.AddRangeAsync(
                new Cereal { Id = 1, Name = "Test Cereal 1", Mfr = "T", Type = "C" },
                new Cereal { Id = 2, Name = "Test Cereal 2", Mfr = "T", Type = "C" }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _cerealManager.GetAllAsync(new CerealFilterModel(), new CerealSortModel());

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetAllAsync_WithManufacturerFilter_ReturnsFilteredCereals()
        {
            // Arrange
            await _dbContext.Cereals.AddRangeAsync(
                new Cereal { Id = 1, Name = "Cereal 1", Mfr = "K", Type = "C" },
                new Cereal { Id = 2, Name = "Cereal 2", Mfr = "G", Type = "C" }
            );
            await _dbContext.SaveChangesAsync();

            var filter = new CerealFilterModel { Manufacturer = "Kellogg" };

            // Act
            var result = await _cerealManager.GetAllAsync(filter, new CerealSortModel());

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("K", result[0].Mfr);
        }

        [TestMethod]
        public async Task GetAllAsync_WithSorting_ReturnsSortedCereals()
        {
            // Arrange
            await _dbContext.Cereals.AddRangeAsync(
                new Cereal { Id = 1, Name = "B Cereal", Mfr = "K", Type = "C" },
                new Cereal { Id = 2, Name = "A Cereal", Mfr = "K", Type = "C" }
            );
            await _dbContext.SaveChangesAsync();

            var sort = new CerealSortModel { SortBy = "name", SortOrder = "asc" };

            // Act
            var result = await _cerealManager.GetAllAsync(new CerealFilterModel(), sort);

            // Assert
            Assert.AreEqual("A Cereal", result[0].Name);
            Assert.AreEqual("B Cereal", result[1].Name);
        }

        [TestMethod]
        public async Task GetById_ExistingId_ReturnsCereal()
        {
            // Arrange
            var expectedCereal = new Cereal { Id = 1, Name = "Test Cereal", Type = "C", Mfr = "K" };
            await _dbContext.Cereals.AddAsync(expectedCereal);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _cerealManager.GetById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Cereal", result.Name);
        }

        [TestMethod]
        public async Task GetByName_ExistingName_ReturnsCereal()
        {
            // Arrange
            var expectedCereal = new Cereal { Id = 1, Name = "Test Cereal", Type = "C", Mfr = "K" };
            await _dbContext.Cereals.AddAsync(expectedCereal);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _cerealManager.GetByName("Test Cereal");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task Add_ValidCereal_AddsToDatabaseAndReturns()
        {
            // Arrange
            var cereal = new Cereal { Name = "New Cereal", Mfr = "T", Type = "C" };

            // Act
            var result = await _cerealManager.Add(cereal);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("New Cereal", result.Name);
            Assert.IsTrue(result.Id > 0);

            // Verify it was added to the database
            var fromDb = await _dbContext.Cereals.FindAsync(result.Id);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual("New Cereal", fromDb.Name);
        }

        [TestMethod]
        public async Task Update_ExistingCereal_UpdatesAndReturns()
        {
            // Arrange
            var cereal = new Cereal { Id = 1, Name = "Test Cereal", Type = "C", Mfr = "K" };
            await _dbContext.Cereals.AddAsync(cereal);
            await _dbContext.SaveChangesAsync();

            cereal.Name = "Updated Cereal";

            // Act
            var result = await _cerealManager.Update(cereal);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Cereal", result.Name);

            // Verify it was updated in the database
            var fromDb = await _dbContext.Cereals.FindAsync(1);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual("Updated Cereal", fromDb.Name);
        }

        [TestMethod]
        public async Task Delete_ExistingId_RemovesFromDatabase()
        {
            // Arrange
            var cereal = new Cereal { Id = 1, Name = "Test Cereal", Mfr = "K", Type = "C" };
            await _dbContext.Cereals.AddAsync(cereal);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _cerealManager.Delete(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Cereal", result.Name);

            // Verify it was removed from the database
            var fromDb = await _dbContext.Cereals.FindAsync(1);
            Assert.IsNull(fromDb);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task Add_DbUpdateException_ThrowsException()
        {
            // We can use a mock of CerealManager for this specific test
            var mockDbContext = new Mock<DBContext>(new DbContextOptions<DBContext>());
            mockDbContext.Setup(db => db.SaveChangesAsync(default))
                .ThrowsAsync(new DbUpdateException("Update failed", new Exception()));

            var manager = new CerealManager(mockDbContext.Object);

            // Act - This should throw an exception
            await manager.Add(new Cereal { Name = "Test" });
        }
    }
}
