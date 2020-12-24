using System;
using System.Collections.Generic;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]  // annotation for tooling
    public class ProductsController : ControllerBase
    {
        private readonly IDutchRepository _repo;
        private readonly ILogger _logger;
        public ProductsController(IDutchRepository repo, ILogger<ProductsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // Using this instead of IActionResult allows you to document the API with types
        [HttpGet]
        [ProducesResponseType(200)]  // annotation for tooling
        public ActionResult<IEnumerable<Product>> Get()
        {
            _logger.LogInformation($"Request - api/products/get : {HttpContext.Request}");
            try
            {
                // asp.net core mvc returns json by default
                // TODO: Make it work with different response types ("content negotiation")
                return Ok(_repo.GetAllProducts());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
            
        }
    }
}
