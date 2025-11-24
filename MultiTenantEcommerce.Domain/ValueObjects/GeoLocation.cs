namespace MultiTenantEcommerce.Domain.ValueObjects;
public class GeoLocation
{
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public GeoLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
