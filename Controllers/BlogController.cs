using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace WebAppExample.Controllers
{
    public class BlogController : ControllerBase
    {
        public IActionResult Article(string article)
        {
            return Content($"This is article with id = {article}\n");
        }
    }
}
