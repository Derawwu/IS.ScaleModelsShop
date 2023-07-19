namespace IS.ScaleModelsShop.API.AcceptanceTests.Services.WebServer;

public interface IWebServerFactory
{
    HttpClient GetHttpClient();

    //Task DisposeWebServer();

    void DisposeHttpClient();
}