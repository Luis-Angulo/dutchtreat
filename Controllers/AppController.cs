using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        // works essentially like a web2py controller. The action binds the input to a req and output to a response
        public IActionResult Index()
        {
            return View();
        }
    }
}
