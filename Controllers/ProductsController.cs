using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;
using WebAppExample.Models;

namespace WebAppExample.Controllers
{
    public class ProductsController : ControllerBase
     {
         public Category Category { get; set; }
 
         public IActionResult Details(string id, [Bind(Prefix = "home")] bool canJump, string height)
         {
             return Content($"id = {id} and canJump = {canJump} and height {height}");
         }
 
         [HttpPost]
         public IActionResult Details(int id, Product product)
         {
             return ControllerContext.MyDisplayRouteInfo(id, product.Name);
         }
 
         public IActionResult Get(Product height)
         {
             if (height == null)
             {
                 
                 return NotFound();
             }
 
             return Ok(height);
         }
     }
 }