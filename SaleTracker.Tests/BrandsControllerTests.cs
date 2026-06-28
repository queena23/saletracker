using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleTracker.API.Controllers;
using SaleTracker.API.Data;
using SaleTracker.API.Models;

namespace SaleTracker.Tests
{
    public class BrandsControllerTests
    {
        private AppDbContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddBrand_ReturnsCreated()
        {
            var db = GetInMemoryDb();
            var controller = new BrandsController(db);
            var brand = new Brand { Name = "Zara", Url = "https://zara.com/sale" };

            var result = await controller.Add(brand);

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task AddBrand_WithNoName_ReturnsBadRequest()
        {
            var db = GetInMemoryDb();
            var controller = new BrandsController(db);
            var brand = new Brand { Name = "" };

            var result = await controller.Add(brand);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ReturnsAllBrands()
        {
            var db = GetInMemoryDb();
            db.Brands.Add(new Brand { Name = "Zara" });
            db.Brands.Add(new Brand { Name = "Nordstrom" });
            await db.SaveChangesAsync();

            var controller = new BrandsController(db);
            var result = await controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var brands = Assert.IsAssignableFrom<List<Brand>>(ok.Value);
            Assert.Equal(2, brands.Count);
        }
    }
}