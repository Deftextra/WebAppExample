using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAppExample.Models;

namespace WebAppExample.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [EnableCors(policyName: "Test")]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public ProductsController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IAsyncEnumerable<Product> GetProducts()
        {
            return _dbContext.Products;
        }


        [HttpPost]
        public async Task<StatusCodeResult> CreateProduct(ProductBindingTarget productBindingTarget)
        {
            await _dbContext.Products.AddAsync(productBindingTarget.ToProduct());
            await _dbContext.SaveChangesAsync();
            
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete("{id}")]
        public async Task DeleteProduct(long id)
        {
            _dbContext.Products.Remove(new Product() {ProductId = id});

            await _dbContext.SaveChangesAsync();
        }

        [HttpPut]
        public async Task UpdateProduct(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(long id, [FromServices] ILogger<ProductsController> logger)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                logger.LogInformation($"product with id = {id} not found");
                return NotFound();
            }

            logger.LogInformation($"product with id = {id} found");

            return Ok(product);
        }
    }
}