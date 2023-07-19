using Newtonsoft.Json;
using System.Text;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.Formatters;

public class StringContentFormatter : IStringContentFormatter
{
    public StringContent CreateStringContent<T>(T data)
    {
        var jsonString = JsonConvert.SerializeObject(data);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        return content;
    }
}