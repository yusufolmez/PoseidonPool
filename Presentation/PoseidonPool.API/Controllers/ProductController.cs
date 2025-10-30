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
using PoseidonPool.Application.Features.Queries.Product.GetBySearch;
using PoseidonPool.Application.Features.Queries.Product.FilterByPrice;
using Microsoft.AspNetCore.Http;
using PoseidonPool.Application.Features.Queries.Product.GetComments;
using PoseidonPool.Application.Features.Commands.Product.AddComment;
using PoseidonPool.Application.ViewModels.Comment;

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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string sort)
        {
            var response = await _mediator.Send(new GetAllProductsQueryRequest { Page = page, PageSize = pageSize, Sort = sort });
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var response = await _mediator.Send(new SearchProductsQueryRequest { Query = query });
            return Ok(response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            var response = await _mediator.Send(new FilterByPriceQueryRequest { MinPrice = minPrice, MaxPrice = maxPrice });
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

        [HttpGet("{id}/images")]
        public async Task<IActionResult> GetImages([FromRoute] string id)
        {
            var images = await _mediator.Send(new PoseidonPool.Application.Features.Queries.Product.GetImages.GetProductImagesQueryRequest { ProductId = id });
            return Ok(images);
        }

        [HttpPost("{id}/images")]
        public async Task<IActionResult> UploadImages([FromRoute] string id, [FromForm] IFormFileCollection files)
        {
            var result = await _mediator.Send(new PoseidonPool.Application.Features.Commands.Product.UploadImages.UploadProductImagesCommandRequest { ProductId = id, Files = files });
            return Ok(result);
        }

        [HttpDelete("{id}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage([FromRoute] string id, [FromRoute] string imageId)
        {
            var result = await _mediator.Send(new PoseidonPool.Application.Features.Commands.Product.DeleteImage.DeleteProductImageCommandRequest { ProductId = id, SlotOrKey = imageId });
            return Ok(result);
        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetComments([FromRoute] string id)
        {
            var result = await _mediator.Send(new GetProductCommentsQueryRequest { ProductId = id });
            return Ok(result);
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment([FromRoute] string id, [FromBody] VM_CreateComment model)
        {
            var result = await _mediator.Send(new AddProductCommentCommandRequest { ProductId = id, Model = model });
            return Ok(result);
        }
    }
}
