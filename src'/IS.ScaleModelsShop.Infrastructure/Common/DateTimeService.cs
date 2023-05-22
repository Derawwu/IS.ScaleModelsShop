using IS.ScaleModelsShop.Domain.Contracts;

namespace IS.ScaleModelsShop.Infrastructure.Common;

public class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}