using Microsoft.AspNetCore.Mvc;

namespace Rhisis.Admin.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns the Index view.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() => View();
    }
}
