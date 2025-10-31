using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialOfferController : ControllerBase
    {
        private readonly ISpecialOfferService _service;
        public SpecialOfferController(ISpecialOfferService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] SpecialOfferDTO model)
        {
            var item = await _service.CreateAsync(model);
            return Ok(item);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] SpecialOfferDTO model)
        {
            var item = await _service.UpdateAsync(id, model);
            return Ok(item);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var ok = await _service.DeleteAsync(id);
            return Ok(new { success = ok });
        }
    }
}


