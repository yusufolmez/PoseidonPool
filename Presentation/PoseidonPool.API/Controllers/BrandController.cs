using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoseidonPool.Application.Features.Queries.Brand.GetAllBrands;
using PoseidonPool.Application.Features.Queries.Brand.GetBrandById;
using PoseidonPool.Application.Features.Commands.Brand.CreateBrand;
using PoseidonPool.Application.Features.Commands.Brand.UpdateBrand;
using PoseidonPool.Application.Features.Commands.Brand.DeleteBrand;
using PoseidonPool.Application.Features.Queries.Product.GetByBrand;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllBrandsQueryRequest());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await _mediator.Send(new GetBrandByIdQueryRequest { Id = id });
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrandCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateBrandCommandRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var response = await _mediator.Send(new DeleteBrandCommandRequest { Id = id });
            return Ok(response);
        }

        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProducts([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetProductsByBrandQueryRequest { BrandId = id });
            return Ok(response);
        }
    }
}
