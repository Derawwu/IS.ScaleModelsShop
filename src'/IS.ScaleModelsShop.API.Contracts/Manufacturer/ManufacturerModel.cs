namespace IS.ScaleModelsShop.API.Contracts.Manufacturer;

public class ManufacturerModel
{
    public Guid Id { get; set; }

    public string Website { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}