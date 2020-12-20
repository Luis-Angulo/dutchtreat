using System;
using System.Collections.Generic;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
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
        public ActionResult Post([FromBody] OrderViewModel model)
        {
            try
            {
                var order = new Order() {
                    OrderDate = model.OrderDate,
                    OrderNumber = model.OrderNumber,
                    Id = model.OrderId
                };
                if (order.OrderDate == DateTime.MinValue)
                {
                    order.OrderDate = DateTime.Now;
                }
                 _repo.AddEntity(order);
                // still hate the modelstate variable
                if (ModelState.IsValid) {
                    if (_repo.SaveAll())
                    {
                        // return Created($"http://localhost:5000/api/orders/{order.Id}", order);
                        // This is just to illustrate you should use the viewmodels as DTOs, irl this is stupid
                        var viewModel = new OrderViewModel() {
                            OrderDate = order.OrderDate,
                            OrderNumber = order.OrderNumber,
                            OrderId = order.Id
                        };
                        return Created($"http://localhost:5000/api/orders/{viewModel.OrderId}", viewModel);
                    }
                    return BadRequest("Couldn't save");
                }
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest("Something went wrong");
            }
        }
    }
}
