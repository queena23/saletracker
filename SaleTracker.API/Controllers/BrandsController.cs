using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleTracker.API.Data;
using SaleTracker.API.Models;

namespace SaleTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BrandsController(AppDbContext db)
        {
            _db = db;
        }

        // GET api/brands
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _db.Brands.ToListAsync();
            return Ok(brands);
        }

        // POST api/brands
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Brand brand)
        {
            if (string.IsNullOrEmpty(brand.Name))
                return BadRequest("Brand name is required");

            _db.Brands.Add(brand);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = brand.Id }, brand);
        }

        // DELETE api/brands/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _db.Brands.FindAsync(id);
            if (brand == null) return NotFound();

            _db.Brands.Remove(brand);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}