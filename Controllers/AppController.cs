using Microsoft.AspNetCore.Mvc;
using DutchTreat.ViewModels;
using DutchTreat.Services;
using DutchTreat.Data;
using Microsoft.AspNetCore.Authorization;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IDutchRepository _repository;
        public AppController(IMailService mailService, IDutchRepository repository)
        {
            _repository = repository;
            _mailService = mailService;
        }

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
                _mailService.SendMessage(formData.Email, formData.Name, formData.Message);
                ViewBag.UserMessage = "Mail sent!";
            } else
            {
                ViewBag.UserMessage = "Mail not sent!";
            }
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            return View();
        }
        [Authorize]
        public IActionResult Shop()
        {
            var resultList = _repository.GetAllProducts();
            return View(resultList);
        }

          

    }
}
