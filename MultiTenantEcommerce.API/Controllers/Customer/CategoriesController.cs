using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Catalog.Categories.Queries.GetById;
using MultiTenantEcommerce.Application.Catalog.Categories.Queries.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers.Customer;

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
    public async Task<ActionResult<List<CategoryResponseDTO>>> GetCategories([FromQuery] CategoryFilterDTO filteredQuery)
    {
        var query = new GetFilteredCategoriesQuery(
            filteredQuery.Name,
            filteredQuery.Description,
            true,  //isActive
            false, //isAdmin
            filteredQuery.Page,
            filteredQuery.PageSize,
            filteredQuery.Sort);

        var categories = await _mediator.Send(query);

        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryResponseDTO>> GetById(Guid id)
    {
        var query = new GetCategoryByIdQuery(id, false);
        var category = await _mediator.Send(query);

        return Ok(category);
    }
}
