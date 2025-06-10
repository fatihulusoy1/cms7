using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Infrastructure.Persistence;
using CMS.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SitesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/sites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Site>>> GetSites()
        {
            // Include Company data to be able to display company name in the frontend
            return await _context.Sites.Include(s => s.Company).ToListAsync();
        }

        // GET: api/sites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Site>> GetSite(int id)
        {
            // Include Company data
            var site = await _context.Sites.Include(s => s.Company)
                                         .FirstOrDefaultAsync(s => s.Id == id);

            if (site == null)
            {
                return NotFound();
            }

            return site;
        }

        // POST: api/sites
        [HttpPost]
        public async Task<ActionResult<Site>> PostSite(Site site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the referenced CompanyId exists
            var companyExists = await _context.Companies.AnyAsync(c => c.Id == site.CompanyId);
            if (!companyExists)
            {
                ModelState.AddModelError("CompanyId", "Invalid Company ID.");
                return BadRequest(ModelState);
            }

            _context.Sites.Add(site);
            await _context.SaveChangesAsync();

            // It's good practice to return the created resource with its navigation properties loaded.
            // This requires another query, but ensures the client gets the full picture.
             var createdSite = await _context.Sites.Include(s => s.Company)
                                         .FirstOrDefaultAsync(s => s.Id == site.Id);

            return CreatedAtAction(nameof(GetSite), new { id = createdSite.Id }, createdSite);
        }

        // PUT: api/sites/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSite(int id, Site site)
        {
            if (id != site.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the referenced CompanyId exists
            var companyExists = await _context.Companies.AnyAsync(c => c.Id == site.CompanyId);
            if (!companyExists)
            {
                ModelState.AddModelError("CompanyId", "Invalid Company ID.");
                return BadRequest(ModelState);
            }

            _context.Entry(site).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/sites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSite(int id)
        {
            var site = await _context.Sites.FindAsync(id);
            if (site == null)
            {
                return NotFound();
            }

            _context.Sites.Remove(site);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SiteExists(int id)
        {
            return _context.Sites.Any(e => e.Id == id);
        }
    }
}
