
using Microsoft.AspNetCore.Mvc;
using WebAppExample.Attributes;

namespace WebAppExample.Controllers
{
    public class HomeController : ControllerBase
    {
        [NamespaceConstraint]
        public IActionResult Index(string id)
        {
            return Content("testing");
        }
    }
}