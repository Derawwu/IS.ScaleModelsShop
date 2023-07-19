namespace IS.ScaleModelsShop.API.Contracts.Manufacturer.UpdateManufacturer;

/// <summary>
/// An UpdateManufacturer model.
/// </summary>
public class UpdateManufacturerModel
{
    /// <summary>
    /// Gets or sets name of the manufacturer.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets website of thr manufacturer.
    /// </summary>
    public string? Website { get; set; }
}