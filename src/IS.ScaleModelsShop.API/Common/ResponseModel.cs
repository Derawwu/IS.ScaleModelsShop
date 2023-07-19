namespace IS.ScaleModelsShop.API.Common;

/// <summary>
///     The model represents the response.
/// </summary>
/// <typeparam name="TItem">Page item type.</typeparam>
public class ResponseModel<TItem>
{
    /// <summary>
    ///     Gets or sets the items in response.
    /// </summary>
    public IEnumerable<TItem> Value { get; set; }
}