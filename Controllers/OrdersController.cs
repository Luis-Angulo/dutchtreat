using System;
using System.Collections.Generic;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IDutchRepository _repo;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        public OrdersController(IDutchRepository repo, ILogger<OrdersController> logger, IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<OrderViewModel>> Get(bool includeItems = false)
        {
            _logger.LogInformation($"Request - api/orders/get : {HttpContext.Request}");
            try
            {
                var userName = User.Identity.Name;
                var userOrders = _repo.GetAllOrdersByUser(userName, includeItems);
                return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(userOrders));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        public ActionResult<OrderViewModel> Get(int id)
        {
            _logger.LogInformation($"Request - api/orders/get(id) : {HttpContext.Request}");
            try
            {
                var order = _repo.GetOrderById(id);
                var ovm = _mapper.Map<Order, OrderViewModel>(order);
                if (order != null)
                {
                    return Ok(ovm);
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
                var order = _mapper.Map<OrderViewModel, Order>(model);
                if (order.OrderDate == DateTime.MinValue)
                {
                    order.OrderDate = DateTime.Now;
                }
                _repo.AddEntity(order);
                if (ModelState.IsValid)
                {
                    if (_repo.SaveAll())
                    {
                        return Created($"http://localhost:5000/api/orders/{order.Id}", _mapper.Map<Order, OrderViewModel>(order));
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
