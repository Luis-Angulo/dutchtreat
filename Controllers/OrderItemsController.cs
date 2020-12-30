using System.Collections.Generic;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DutchTreat.Controllers
{
    // I don't get why Shawn wants it structured this way
    // Says he wants the API to represent the relationship between the entities
    // Like, the way you access the resource should be reflective of the underlying data shape?
    [Route("api/orders/{orderid}/items")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : ControllerBase
    {
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IDutchRepository _repository;
        private readonly IMapper _mapper;

        public OrderItemsController(ILogger<OrderItemsController> logger, IDutchRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Get(int orderId)
        {
            var userName = User.Identity.Name;
            var order = _repository.GetOrderById(userName, orderId);
            if (order != null)
            {
                return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
            }
            return NotFound();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Get(int orderId, int id)
        {
            var userName = User.Identity.Name;
            var order = _repository.GetOrderById(userName, orderId);
            if (order != null)
            {
                var item = order.Items.Where(i => i.Id == id).FirstOrDefault();
                if (item != null)
                {
                    return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(item));
                }
            }
            return NotFound();
        }
    }
}
