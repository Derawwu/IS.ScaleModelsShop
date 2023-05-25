using IS.ScaleModelsShop.API.SlugifyParameters;

namespace IS.ScaleModelsShop.API.UnitTests.SlugifyParameters;

internal class ParameterTransformerTests
{
    private const string Prefix = "api/";

    [TestCase(null, null)]
    [TestCase("Test", $"{Prefix}test")]
    [TestCase("TestName", $"{Prefix}test-name")]
    [TestCase("TestControllerName", $"{Prefix}test-controller-name")]
    public void TransformOutbound_ParameterNameWithUpperCase_ReturnsParameterNameMatchToRuleBookWithPrefix(
        string controllerName, string expected)
    {
        var parameterTransformer = new ParameterTransformer();
        var actual = parameterTransformer.TransformOutbound(controllerName);

        Assert.AreEqual(expected, actual);
    }
}