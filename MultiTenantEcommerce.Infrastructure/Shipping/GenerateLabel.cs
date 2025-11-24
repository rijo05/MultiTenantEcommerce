using MultiTenantEcommerce.Domain.ValueObjects;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MultiTenantEcommerce.Infrastructure.Shipping;
public class GenerateLabel : IDocument
{
    private readonly string _logoImage;
    private readonly string _trackingNumber;
    private readonly string _barcodeImage;
    private readonly string _recipientName;
    private readonly Address _address;

    public GenerateLabel(string logoImage,
        string trackingNumber,
        string barcodeImage,
        string recipientName,
        Address address)
    {
        _logoImage = logoImage;
        _trackingNumber = trackingNumber;
        _barcodeImage = barcodeImage;
        _recipientName = recipientName;
        _address = address;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A6);
            page.Margin(10);
            page.DefaultTextStyle(x => x.FontSize(12));

            page.Content().Column(col =>
            {
                // Carrier logo
                col.Item().AlignCenter().Image(_logoImage).FitWidth();

                // Tracking number destacado
                col.Item().PaddingVertical(10).AlignCenter()
                   .Text($"Tracking: {_trackingNumber}")
                   .FontSize(18).Bold();

                // Barcode
                col.Item().AlignCenter().Image(_barcodeImage).FitWidth();

                // Recipient info
                col.Item().PaddingTop(20).Column(c =>
                {
                    c.Item().Text("Recipient:").Bold();
                    c.Item().Text(_recipientName);
                    c.Item().Text($"{_address.Street} - {_address.HouseNumber}");
                    c.Item().Text(_address.City);
                    c.Item().Text(_address.PostalCode);
                    c.Item().Text(_address.Country);
                });
            });
        });
    }
}
