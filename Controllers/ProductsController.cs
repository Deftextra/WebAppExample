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
    [Route("/api/[action]")]
    [EnableCors(policyName: "Test")]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public ProductsController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        // api/products
        [HttpGet]
        public IAsyncEnumerable<Product> Products()
        {
            // returns an object is equivelant to return ok(returnType)
            // when action method is null is equivelant to returning null.
            return _dbContext.Products;
        }


        // api/Product
        [HttpPost]
        public async Task<IActionResult> Product(ProductBindingTarget productBindingTarget)
        {
                await _dbContext.Products.AddAsync(productBindingTarget.ToProduct());
                await _dbContext.SaveChangesAsync();
                return Ok(productBindingTarget);
        }

        //api/Product
        [HttpDelete("{id}")]
        public async Task Product(long id)
        {
            _dbContext.Products.Remove(new Product() {ProductId = id});

            await _dbContext.SaveChangesAsync();
        }

        // api/Product
        [HttpPut]
        public async Task Product(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        // api/Product/{id?}
        [HttpGet("{id?}")]
        public async Task<IActionResult> Product(long id, [FromServices] ILogger<ProductsController> logger)
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


        // api/Redirect
        [HttpGet("{value=hello}")]
        public IActionResult RedirectTest(string value)
        {
            var endpoint = HttpContext.GetEndpoint();


            return RedirectToRoute(new
            {
                controller = "Products", action = "Product", id = "1"
            });
        }
    }
}