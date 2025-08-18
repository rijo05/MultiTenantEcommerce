using MultiTenantEcommerce.Application.DTOs.Category;

namespace MultiTenantEcommerce.Application.Interfaces;

public interface ICategoryService
{
    public Task<List<CategoryResponseDTO>> GetFilteredCategoriesAsync(CategoryFilterDTO categoryFilter);
    public Task<CategoryResponseDTO?> GetCategoryByIdAsync(Guid id);

    public Task<CategoryResponseDTO> AddCategoryAsync(CreateCategoryDTO categoryDTO);
    public Task DeleteCategoryAsync(Guid id);
    public Task<CategoryResponseDTO> UpdateCategoryAsync(Guid id, UpdateCategoryDTO categoryDTO);
}
