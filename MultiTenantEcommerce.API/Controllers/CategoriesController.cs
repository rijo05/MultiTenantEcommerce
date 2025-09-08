using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Catalog.Categories.Commands.Create;
using MultiTenantEcommerce.Application.Catalog.Categories.Commands.Delete;
using MultiTenantEcommerce.Application.Catalog.Categories.Commands.Update;
using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Catalog.Categories.Queries.GetById;
using MultiTenantEcommerce.Application.Catalog.Categories.Queries.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponseDTO>>> GetCategories([FromQuery] GetFilteredCategoriesQuery filteredQuery)
    {
        var categories = await _mediator.Send(filteredQuery);

        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryResponseDTO>> GetById(Guid id)
    {
        var query = new GetCategoryByIdQuery(id);
        var category = await _mediator.Send(query);

        return Ok(category);
    }


    [HttpPost]
    public async Task<ActionResult<CategoryResponseDTO>> Create([FromBody] CreateCategoryCommand command)
    {
        if (command is null)
            return BadRequest("Category data must be provided.");

        var category = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            category
            );
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<CategoryResponseDTO>> Update(Guid id, [FromBody] UpdateCategoryDTO categoryDTO)
    {
        if (categoryDTO is null)
            return BadRequest("Category data must be provided.");

        var command = new UpdateCategoryCommand(id,
            categoryDTO.Name,
            categoryDTO.Description,
            categoryDTO.IsActive);

        var category = await _mediator.Send(command);
        return Ok(category);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCategoryCommand(id);

        await _mediator.Send(command);
        return NoContent();
    }
}
