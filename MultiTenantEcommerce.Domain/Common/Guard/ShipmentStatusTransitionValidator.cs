using MultiTenantEcommerce.Domain.Shipping.Enums;

namespace MultiTenantEcommerce.Domain.Common.Guard;
public static class ShipmentStatusTransitionValidator
{
    private static readonly Dictionary<ShipmentStatus, List<ShipmentStatus>> AllowedTransitions = new()
    {
        {ShipmentStatus.Pending, new List<ShipmentStatus>() {ShipmentStatus.Cancelled, ShipmentStatus.Shipped } },
        {ShipmentStatus.Shipped, new List<ShipmentStatus>() {ShipmentStatus.InTransit } },
        {ShipmentStatus.InTransit, new List<ShipmentStatus>() {ShipmentStatus.Delivered } },
    };

    public static void ValidateTransition(ShipmentStatus CurrentStatus, ShipmentStatus newStatus)
    {
        if (AllowedTransitions.TryGetValue(CurrentStatus, out var allowed) && allowed.Contains(newStatus))
        {
            return;
        }

        throw new Exception($"Invalid shipment status transition from {CurrentStatus} to {newStatus}.");
    }
}
