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
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IDutchRepository _repo;
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(IDutchRepository repo, ILogger<OrdersController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Order>> Get()
        {
            _logger.LogInformation($"Request - api/orders/get : {HttpContext.Request}");
            try
            {                
                return Ok(_repo.GetAllOrders());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        public ActionResult<Order> Get(int id)
        {
            _logger.LogInformation($"Request - api/orders/get(id) : {HttpContext.Request}");
            try
            {
                var order = _repo.GetOrderById(id);
                if(order != null)
                {
                    return Ok(order);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }
        [HttpPost]
        public ActionResult Post(Order data)
        {
            return Ok();
        }
    }
}
