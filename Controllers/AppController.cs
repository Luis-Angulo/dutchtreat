using Microsoft.AspNetCore.Mvc;
using DutchTreat.ViewModels;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        // works essentially like a web2py controller. The action binds the input to a req and output to a response
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        /* The attributes route request verbs to the correct handler 
         * and method overloading allows sharing the same endpoint.
         */
        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel formData)
        {   
            if (ModelState.IsValid)
            {
                System.Console.WriteLine("Valid");
            } else
            {
                System.Console.WriteLine("Invalid");
            }
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            return View();
        }

    }
}
