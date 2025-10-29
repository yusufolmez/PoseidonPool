using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoseidonPool.Application.Features.Queries.Product.GetAllProducts;
using PoseidonPool.Application.Features.Queries.Product.GetProductById;
using PoseidonPool.Application.Features.Queries.Product.GetByBrand;
using PoseidonPool.Application.Features.Queries.Product.GetByCategory;
using PoseidonPool.Application.Features.Commands.Product.CreateProduct;
using PoseidonPool.Application.Features.Commands.Product.UpdateProduct;
using PoseidonPool.Application.Features.Commands.Product.DeleteProduct;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllProductsQueryRequest());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await _mediator.Send(new GetProductByIdQueryRequest { Id = id });
            return Ok(response);
        }

        [HttpGet("brand/{brandId}")]
        public async Task<IActionResult> GetByBrand([FromRoute] Guid brandId)
        {
            var response = await _mediator.Send(new GetProductsByBrandQueryRequest { BrandId = brandId });
            return Ok(response);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory([FromRoute] Guid categoryId)
        {
            var response = await _mediator.Send(new GetProductsByCategoryQueryRequest { CategoryId = categoryId });
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromForm] UpdateProductCommandRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var response = await _mediator.Send(new DeleteProductCommandRequest { Id = id });
            return Ok(response);
        }
    }
}
