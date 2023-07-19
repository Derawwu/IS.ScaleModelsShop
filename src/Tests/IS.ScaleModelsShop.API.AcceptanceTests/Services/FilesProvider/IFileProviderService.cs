namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.Database.FilesProvider;

public interface IFileProviderService
{
    string GetJsonStringFromFile(string filePath);
}