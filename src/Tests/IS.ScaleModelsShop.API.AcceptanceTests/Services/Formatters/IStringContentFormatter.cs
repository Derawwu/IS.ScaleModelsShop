using TechTalk.SpecFlow;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.Formatters;

public interface IStringContentFormatter
{
    StringContent CreateStringContent<T>(T data);
}