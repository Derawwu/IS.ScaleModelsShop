using TechTalk.SpecFlow;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Transforms;

[Binding]
public class HttpMethodTransformer
{
    [StepArgumentTransformation]
    public virtual HttpMethod GetValue(string method)
    {
        if (string.IsNullOrWhiteSpace(method))
        {
            throw new ArgumentException("Method can not be null or empty", nameof(method));
        }

        var returnValue = new HttpMethod(method);
        return returnValue;
    }
}