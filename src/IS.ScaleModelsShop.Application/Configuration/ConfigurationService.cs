using Microsoft.Extensions.Configuration;

namespace IS.ScaleModelsShop.Application.Configuration;

/// <inheritdoc cref="IConfigurationService" />
public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigurationService" /> class.
    /// </summary>
    /// <param name="configuration">Instance of the <see cref="IConfiguration" />.</param>
    public ConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <inheritdoc />
    public T? GetConfiguration<T>(string configurationKey)
    {
        return _configuration.GetRequiredSection(configurationKey).Get<T>();
    }
}