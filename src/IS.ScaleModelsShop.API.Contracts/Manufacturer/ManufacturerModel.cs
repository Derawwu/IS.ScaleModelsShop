namespace IS.ScaleModelsShop.API.Contracts.Manufacturer;

/// <summary>
/// Model for mapping between application and Infrastructure model.
/// </summary>
public class ManufacturerModel
{
    /// <summary>
    /// Gets or sets Guid of the manufacturer.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets website of the Manufacturer.
    /// </summary>
    public string Website { get; set; }

    /// <summary>
    /// Gets or sets name kof the manufacturer.
    /// </summary>
    public string Name { get; set; }
}