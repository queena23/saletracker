using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleTracker.API.Data;
using SaleTracker.API.Models;

namespace SaleTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DealsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DealsController(AppDbContext db)
        {
            _db = db;
        }

        // GET api/deals
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var deals = await _db.Deals
                .Include(d => d.Brand)
                .OrderByDescending(d => d.FoundAt)
                .ToListAsync();
            return Ok(deals);
        }

        // GET api/deals/brand/1
        [HttpGet("brand/{brandId}")]
        public async Task<IActionResult> GetByBrand(int brandId)
        {
            var deals = await _db.Deals
                .Include(d => d.Brand)
                .Where(d => d.BrandId == brandId)
                .OrderByDescending(d => d.FoundAt)
                .ToListAsync();
            return Ok(deals);
        }

        // POST api/deals
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Deal deal)
        {
            if (string.IsNullOrEmpty(deal.Title))
                return BadRequest("Deal title is required");

            var brand = await _db.Brands.FindAsync(deal.BrandId);
            if (brand == null)
                return NotFound("Brand not found");

            _db.Deals.Add(deal);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = deal.Id }, deal);
        }
    }
}