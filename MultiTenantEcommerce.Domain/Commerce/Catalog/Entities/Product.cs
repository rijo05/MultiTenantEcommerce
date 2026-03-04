using MultiTenantEcommerce.Domain.Commerce.Catalog.Enums;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Events;
using MultiTenantEcommerce.Domain.Commerce.Catalog.ValueObjects;
using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Utilities.Constants;
using MultiTenantEcommerce.Shared.Utilities.Guards;
using static System.Net.Mime.MediaTypeNames;

namespace MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;

public class Product : TenantBase
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Money Price { get; }
    public bool IsActive { get; private set; }
    public Guid CategoryId { get; private set; }
    public StockStatus StockStatus { get; private set; }
    public DateTime LastStockUpdateAt { get; private set; } = DateTime.MinValue;
    public IReadOnlyCollection<ProductImages> Images => _images.AsReadOnly();

    private readonly List<ProductImages> _images = new();

    private Product()
    {
    }

    public Product(Guid tenantId,
        string name,
        Money price,
        Category category,
        string? description,
        bool? isActive) : base(tenantId)
    {
        GuardCommon.AgainstNullOrEmpty(name, nameof(name));
        GuardCommon.AgainstMaxLength(name, 100, nameof(name));
        GuardCommon.AgainstEmptyGuid(tenantId, nameof(tenantId));

        Name = name;
        Description = description;
        Price = price;
        IsActive = isActive ?? true;
        CategoryId = category.Id;
        StockStatus = StockStatus.OutOfStock;

        AddDomainEvent(new ProductCreatedEvent(tenantId, Id));
    }

    public void UpdateStockStatus(StockStatus newStatus, DateTime eventOccurredOn)
    {
        if (eventOccurredOn <= LastStockUpdateAt)
            return;

        StockStatus = newStatus;
        LastStockUpdateAt = eventOccurredOn;
    }

    #region Images


    public IReadOnlyList<ProductImages> AddImages(IEnumerable<ImageMetadata> newImages)
    {
        var newImagesList = newImages.ToList();

        if (_images.Count + newImagesList.Count > 30)
            throw new Exception($"Cant add more images");

        var listContentType = new List<string> { MimeTypes.Png, MimeTypes.Jpeg, MimeTypes.Jpg };

        var addedImages = new List<ProductImages>();

        int currentMaxOrder = _images.Any() ? _images.Max(i => i.SortOrder) : 0;

        foreach (var img in newImages)
        {
            if (!listContentType.Contains(img.ContentType))
                throw new Exception($"O formato {img.ContentType} não é permitido.");

            currentMaxOrder++;

            var imageEntity = new ProductImages(TenantId, Id, img.FileName, img.Size, img.ContentType, currentMaxOrder);
            _images.Add(imageEntity);
            addedImages.Add(imageEntity);
        }

        return addedImages;
    }

    public void DeleteImage(Guid imageId)
    {
        var imageToRemove = _images.FirstOrDefault(img => img.Id == imageId)
            ?? throw new Exception("Imagem não existe");

        _images.Remove(imageToRemove);

        int order = 1;
        foreach (var img in _images.OrderBy(i => i.SortOrder))
        {
            img.ChangeOrder(order++);
        }

        AddDomainEvent(new ProductImageDeletedEvent(TenantId, Id, imageToRemove.Id, imageToRemove.Key));
    }

    public void ReorderImages(List<Guid> orderedImageIds)
    {
        if (orderedImageIds.Count != _images.Count)
            throw new Exception("Faltam imagens");

        for (int i = 0; i < orderedImageIds.Count; i++)
        {
            var imageId = orderedImageIds[i];
            var image = _images.FirstOrDefault(img => img.Id == imageId)
                ?? throw new Exception($"imagem nao existe");

            image.ChangeOrder(i + 1);
        }
    }

    #endregion

    #region UPDATE DATA

    public void UpdateProduct(string? Name,
        string? Description,
        decimal? Price,
        bool? IsActive,
        Guid? CategoryId)
    {
        if (CategoryId.HasValue)
            UpdateCategory(CategoryId.Value);


        if (!string.IsNullOrEmpty(Name))
            UpdateName(Name);

        if (Description is not null)
        {
            if (Description == "")
                ClearDescription();
            else
                UpdateDescription(Description);
        }

        if (Price.HasValue)
            ChangePrice(Price.Value);

        if (IsActive.HasValue)
            ChangeActive(IsActive.Value);
    }

    public void UpdateName(string newName)
    {
        GuardCommon.AgainstNullOrEmpty(newName, nameof(newName));
        GuardCommon.AgainstMaxLength(newName, 100, nameof(newName));
        Name = newName;
        SetUpdatedAt();
    }

    public void UpdateDescription(string description)
    {
        GuardCommon.AgainstMaxLength(description, 255, nameof(description));
        Description = description;
        SetUpdatedAt();
    }

    public void ChangePrice(decimal newPrice)
    {
        Price.UpdatePrice(newPrice);
        SetUpdatedAt();
    }

    public void UpdateCategory(Guid newCategoryId)
    {
        GuardCommon.AgainstEmptyGuid(newCategoryId, nameof(newCategoryId));
        CategoryId = newCategoryId;
        SetUpdatedAt();
    }

    public void ChangeActive(bool isActive)
    {
        IsActive = isActive;
        SetUpdatedAt();
    }

    public void ClearDescription()
    {
        Description = null;
        SetUpdatedAt();
    }

    #endregion
}