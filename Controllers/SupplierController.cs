using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppExample.Models;

namespace WebAppExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public SupplierController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSupplier(long id)
        {
            // This works fine for teh 
            var supplier = await _dbContext.Suppliers.Include(s => s.Products)
                .FirstAsync(sp => sp.SupplierId == id);

            if (supplier == null)
            {
                return NotFound();
            }

            foreach (var supplierProduct in supplier.Products)
            {
                supplierProduct.Supplier = null;
            }

            return Ok("hello");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchSupplier(long id, JsonPatchDocument<Supplier> patchDocument)
        {
            var supplier = await _dbContext.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(supplier);
            await _dbContext.SaveChangesAsync();

            return Ok(supplier);
        }
    }
}