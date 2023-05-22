namespace IS.ScaleModelsShop.Domain.Contracts;

public interface IDateTime
{
    /// <inheritdoc cref="DateTime.UtcNow" />
    DateTime UtcNow { get; }
}