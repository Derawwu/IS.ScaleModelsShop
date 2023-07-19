using Newtonsoft.Json.Linq;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Constants;

public class MockConstants
{
    private const string BaseDirectoryPathToTheMockFolder = @"..\..\..\Mocks\";

    private static readonly string MockFolderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BaseDirectoryPathToTheMockFolder));

    public static readonly string CategoriesListOfContent = MockFolderPath + "CategoriesListMock.json";

    public static readonly string ManufacturersListOfContent = MockFolderPath + "ManufacturersListMock.json";

    public static string ProductsListOfContent = MockFolderPath + "ProductsListMock.json";
}