using Inmeta.Moving.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using IOrdersService = Inmeta.Moving.Services.IService<Inmeta.Moving.Services.Models.Order>;
using ISearchingOrdersService = Inmeta.Moving.Services.ISearchingService<Inmeta.Moving.Services.Models.Order>;

namespace Inmeta.Moving.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [RequiredScope("orders_user")]
    public class OrdersController : ControllerBase
    {
        private IOrdersService _ordersService;
        private ISearchingOrdersService _searchingOrdersService;

        public OrdersController(
            IOrdersService ordersService,
            ISearchingOrdersService searchingOrdersService
        )
        {
            if (ordersService == null) { throw new ArgumentNullException(nameof(ordersService)); }
            if (searchingOrdersService == null) { throw new ArgumentNullException(nameof(searchingOrdersService)); }

            _ordersService = ordersService;
            _searchingOrdersService = searchingOrdersService;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        [RequiredScope("orders_user")]
        public async Task<IActionResult> GetAsync([FromQuery] string? text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var ordersFilteredByAddress = await _searchingOrdersService.FindByText(text);
                return Ok(ordersFilteredByAddress);
            }

            var orders = await _ordersService.GetAsync();
            return Ok(orders);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            if (id < 0) { return NotFound(); }

            var order = await _ordersService.GetByIdAsync(id);
            if (order == null) { return NotFound(); }
            return Ok(order);
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Order order)
        {
            if (order == null) { return BadRequest(order); }

            var newlyCreatedOrder = await _ordersService.CreateAsync(order);
            return Ok(newlyCreatedOrder);
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Order order)
        {
            if (id < 0) { return NotFound(); }
            if (order == null) { return BadRequest(order); }

            var updatedOrder = await _ordersService.UpdateAsync(id, order);
            return Ok(updatedOrder);
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 0) { return NotFound(); }

            await _ordersService.DeleteAsync(id);
            return Ok();
        }
    }
}
