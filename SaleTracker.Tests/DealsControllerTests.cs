using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleTracker.API.Controllers;
using SaleTracker.API.Data;
using SaleTracker.API.Models;

namespace SaleTracker.Tests
{
    public class DealsControllerTests
    {
        private AppDbContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddDeal_ReturnsCreated()
        {
            var db = GetInMemoryDb();
            var brand = new Brand { Name = "Zara", Url = "https://zara.com/sale" };
            db.Brands.Add(brand);
            await db.SaveChangesAsync();

            var controller = new DealsController(db);
            var deal = new Deal
            {
                BrandId = brand.Id,
                Title = "Zara mid season sale",
                Description = "Up to 50% off",
                SourceUrl = "https://reddit.com/r/deals/123"
            };

            var result = await controller.Add(deal);

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task AddDeal_WithInvalidBrand_ReturnsNotFound()
        {
            var db = GetInMemoryDb();
            var controller = new DealsController(db);
            var deal = new Deal
            {
                BrandId = 999,
                Title = "Some deal"
            };

            var result = await controller.Add(deal);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetByBrand_ReturnsOnlyThatBrandDeals()
        {
            var db = GetInMemoryDb();
            var zara = new Brand { Name = "Zara" };
            var nordstrom = new Brand { Name = "Nordstrom" };
            db.Brands.AddRange(zara, nordstrom);
            await db.SaveChangesAsync();

            db.Deals.Add(new Deal { BrandId = zara.Id, Title = "Zara sale" });
            db.Deals.Add(new Deal { BrandId = zara.Id, Title = "Zara clearance" });
            db.Deals.Add(new Deal { BrandId = nordstrom.Id, Title = "Nordstrom sale" });
            await db.SaveChangesAsync();

            var controller = new DealsController(db);
            var result = await controller.GetByBrand(zara.Id);

            var ok = Assert.IsType<OkObjectResult>(result);
            var deals = Assert.IsAssignableFrom<List<Deal>>(ok.Value);
            Assert.Equal(2, deals.Count);
        }
    }
}