using IS.ScaleModelsShop.Domain.Contracts;

namespace IS.ScaleModelsShop.Infrastructure.Common;

/// <inheritdoc cref="IDateTime" />
public class DateTimeService : IDateTime
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}