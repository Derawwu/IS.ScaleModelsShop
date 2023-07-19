namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.FilesProvider;

/// <summary>
/// Class for working with files.
/// </summary>
public class FileProviderService : IFileProviderService
{
    /// <inheritdoc />
    public string GetJsonStringFromFile(string filePath)
    {
        using (var sr = new StreamReader(filePath))
        {
            return sr.ReadToEnd();
        }
    }
}