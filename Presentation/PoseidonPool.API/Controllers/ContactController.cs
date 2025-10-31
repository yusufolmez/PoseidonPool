using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly PoseidonPool.Application.Abstractions.Services.IContactService _service;
        public ContactController(PoseidonPool.Application.Abstractions.Services.IContactService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] bool? isRead)
        {
            var list = await _service.GetAllAsync(isRead);
            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] PoseidonPool.Application.DTOs.Catalog.ContactDTO model)
        {
            var created = await _service.CreateAsync(model);
            return Ok(created);
        }

        [HttpPut("{id}/read")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkRead([FromRoute] string id, [FromQuery] bool isRead = true)
        {
            var updated = await _service.MarkReadAsync(id, isRead);
            return Ok(updated);
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


