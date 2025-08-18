using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.DTOs.Category;
using MultiTenantEcommerce.Application.Interfaces;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponseDTO>>> GetCategories([FromQuery] CategoryFilterDTO categoryFilter)
    {
        var categories = await _categoryService.GetFilteredCategoriesAsync(categoryFilter);

        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryResponseDTO>> GetById(Guid id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        return Ok(category);
    }


    [HttpPost]
    public async Task<ActionResult<CategoryResponseDTO>> Create([FromBody] CreateCategoryDTO categoryDTO)
    {
        if (categoryDTO is null)
            return BadRequest("Category data must be provided.");

        var category = await _categoryService.AddCategoryAsync(categoryDTO);

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

        var category = await _categoryService.UpdateCategoryAsync(id, categoryDTO);
        return Ok(category);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
    }
}
