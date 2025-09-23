﻿namespace MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
public class CategoryResponseAdminDTO : ICategoryDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
