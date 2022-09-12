using Inmeta.Moving.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ICustomersService = Inmeta.Moving.Services.IService<Inmeta.Moving.Services.Models.Customer>;
using ISearchingCustomersService = Inmeta.Moving.Services.ISearchingService<Inmeta.Moving.Services.Models.Customer>;

namespace Inmeta.Moving.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersService _customersService;
        private readonly ISearchingCustomersService _searchingCustomersService;

        public CustomersController(
            ICustomersService customersService,
            ISearchingCustomersService searchingCustomersService
        )
        {
            if (customersService == null) { throw new ArgumentNullException(nameof(_customersService)); }
            if (searchingCustomersService == null) { throw new ArgumentNullException(nameof(_searchingCustomersService)); }

            _customersService = customersService;
            _searchingCustomersService = searchingCustomersService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Customer customer)
        {
            if (customer == null) { return BadRequest(customer); }

            var newlyCreatedCustomer = await _customersService.CreateAsync(customer);
            return Ok(newlyCreatedCustomer);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? name = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var resultFilteredByName = await _searchingCustomersService.FindByText(name);
                return Ok(resultFilteredByName);
            }

            var result = await _customersService.GetAsync();
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 0) { return NotFound(); }

            await _customersService.DeleteAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Customer customer)
        {
            if (id < 0) { return NotFound(); }
            if (customer == null) { return BadRequest(customer); }

            await _customersService.UpdateAsync(id, customer);
            return Ok();
        }
    }
}
