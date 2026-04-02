using Microsoft.AspNetCore.Mvc;
using Product.Application.Features.Products.Commands.CreateProduct;
using Product.Application.Features.Products.Commands.UpdateProduct;
using Product.Application.Features.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Authorization;
namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly CreateProductCommandHandler _createProductCommandHandler;
    private readonly UpdateProductCommandHandler _updateProductCommandHandler;
    private readonly GetProductsQueryHandler _getProductsQueryHandler;

    public ProductsController(
        CreateProductCommandHandler createProductCommandHandler,
        UpdateProductCommandHandler updateProductCommandHandler,
        GetProductsQueryHandler getProductsQueryHandler)
    {
        _createProductCommandHandler = createProductCommandHandler;
        _updateProductCommandHandler = updateProductCommandHandler;
        _getProductsQueryHandler = getProductsQueryHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var productId = await _createProductCommandHandler.HandleAsync(command, cancellationToken);

        return Ok(new
        {
            Id = productId,
            Message = "Product created successfully."
        });
    }
    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Route id and command id do not match.");
        }

        var updated = await _updateProductCommandHandler.HandleAsync(command, cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return Ok(new
        {
            Message = "Product updated successfully."
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await _getProductsQueryHandler.HandleAsync(cancellationToken);
        return Ok(products);
    }
}