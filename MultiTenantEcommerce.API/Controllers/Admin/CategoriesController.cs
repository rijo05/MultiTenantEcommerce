using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.Application.Catalog.Categories.Commands.Create;
using MultiTenantEcommerce.Application.Catalog.Categories.Commands.Delete;
using MultiTenantEcommerce.Application.Catalog.Categories.Commands.Update;
using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Catalog.Categories.Queries.GetById;
using MultiTenantEcommerce.Application.Catalog.Categories.Queries.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers.Admin;

[ApiController]
[Authorize(Policy = "EmployeeOnly")]
[Area("Admin")]
[Route("api/[area]/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HasPermission("read.category")]
    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryResponseAdminDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ICategoryDTO>>> GetCategories([FromQuery] CategoryFilterAdminDTO filteredQuery)
    {
        var query = new GetFilteredCategoriesQuery(
            filteredQuery.Name,
            filteredQuery.Description,
            filteredQuery.IsActive,
            true,
            filteredQuery.Page,
            filteredQuery.PageSize,
            filteredQuery.Sort);

        var categories = await _mediator.Send(query);

        return Ok(categories);
    }

    [HasPermission("read.category")]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryResponseAdminDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICategoryDTO>> GetById(Guid id)
    {
        var query = new GetCategoryByIdQuery(id, true);
        var category = await _mediator.Send(query);

        return Ok(category);
    }

    [HasPermission("create.category")]
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

    [HasPermission("update.category")]
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

    [HasPermission("delete.category")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCategoryCommand(id);

        await _mediator.Send(command);
        return NoContent();
    }
}
