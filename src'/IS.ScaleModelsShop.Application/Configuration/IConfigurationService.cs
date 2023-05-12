namespace IS.ScaleModelsShop.Application.Configuration
{
    /// <summary>
    /// Defines functionality to configure application settings.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets configuration by type.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="configurationKey">Key for getting configuration value.</param>
        /// <returns>Configuration instance.</returns>
        T GetConfiguration<T>(string configurationKey);
    }
}