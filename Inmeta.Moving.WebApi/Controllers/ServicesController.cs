using Inmeta.Moving.Services.Models;
using Microsoft.AspNetCore.Mvc;

using IServicesService = Inmeta.Moving.Services.IService<Inmeta.Moving.Services.Models.Service>;

namespace Inmeta.Moving.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ServicesController : ControllerBase
    {
        private IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            if (servicesService == null) { throw new ArgumentNullException(nameof(servicesService)); }

            _servicesService = servicesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var orders = await _servicesService.GetAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Service service)
        {
            if (service == null) { return BadRequest(service); }

            await _servicesService.CreateAsync(service);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 0) { return NotFound(); }

            await _servicesService.DeleteAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Service service)
        {
            if (id < 0) { return NotFound(); }
            if (service == null) { return BadRequest(service); }

            await _servicesService.UpdateAsync(id, service);
            return Ok();
        }
    }
}
