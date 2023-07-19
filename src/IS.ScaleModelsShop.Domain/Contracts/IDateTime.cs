namespace IS.ScaleModelsShop.Domain.Contracts;

/// <inheritdoc cref="DateTime" />
public interface IDateTime
{
    /// <inheritdoc cref="DateTime.UtcNow" />
    DateTime UtcNow { get; }
}