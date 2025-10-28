using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoseidonPool.Application.Features.Commands.Category.CreateCategory;
using PoseidonPool.Application.Features.Commands.Category.UpdateCategory;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new PoseidonPool.Application.Features.Queries.Category.GetAllCategory.GetAllCategoryQueryRequest());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await _mediator.Send(new PoseidonPool.Application.Features.Queries.Category.GetCategoryById.GetCategoryByIdQueryRequest { Id = id });
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateCategoryCommandRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var response = await _mediator.Send(new PoseidonPool.Application.Features.Commands.Category.DeleteCategory.DeleteCategoryCommandRequest { Id = id });
            return Ok(response);
        }
    }
}
