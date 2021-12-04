using Ardalis.GuardClauses;
using Catalog.API.Commands;
using Catalog.API.Data.Entities;
using Catalog.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = Guard.Against.Null(mediator, nameof(mediator));
    }

    #region Read Actions

    [HttpGet(Name = "Get all Products")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        GetAllProductsResponse response = await _mediator.Send(new GetAllProductsQuery());

        return Ok(response.Products);
    }

    [HttpGet("{id:length(24)}", Name = "Get Product by ID")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetById([FromRoute] string id)
    {
        GetProductByIdResponse response = await _mediator.Send(new GetProductByIdQuery(id));

        return response == null ? NotFound() : Ok(response.Product);
    }

    [HttpGet("name/{name}", Name = "Get Product by Name")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetByName([FromRoute] string name)
    {
        GetProductByNameResponse response = await _mediator.Send(new GetProductByNameQuery(name));

        return Ok(response.Products);
    }

    [HttpGet("category/{category}", Name = "Get Product by Category")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetByCategory([FromRoute] string category)
    {
        GetProductByCategoryResponse response = await _mediator.Send(new GetProductByCategoryQuery(category));

        return Ok(response.Products);
    }

    #endregion Read Actions

    #region Write Actions

    [HttpPost(Name = "Create new Product")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<CreateProductResponse>> Create([FromBody] Product product)
    {
        CreateProductResponse response = await _mediator.Send(new CreateProductCommand(product));

        return Ok(response);
    }

    [HttpPut("{id:length(24)}", Name = "Update existing Product")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UpdateProductResponse>> Update([ValidatedNotNull][FromRoute] string id, [FromBody] Product product)
    {
        UpdateProductResponse response = await _mediator.Send(new UpdateProductCommand(id, product));

        return Ok(response);
    }


    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteProductResponse>> DeleteProductById([ValidatedNotNull][FromRoute] string id)
    {
        DeleteProductResponse response = await _mediator.Send(new DeleteProductCommand(id));

        return Ok(response);
    }

    #endregion Write Actions
}
