using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TemplateApp.Server.Infrastructure;

namespace Tools.Test.API;

public abstract class ApiTestBase<TDbContext>
{
    protected readonly HttpClient Client;
    protected readonly TestServer Server;
    protected string BaseUrl { get; set; }

    protected ApiTestBase(string baseUrl, Action<IServiceCollection> serviceCollection = null)
    {
        BaseUrl = baseUrl;
        var builder = new WebHostBuilder()
            .UseStartup<TestStartup>();

        // TODO: Mock needed dependencies. Example:
        // var fileStorageMock = new ...
        // var factory = new FileStorageFactory(...)
        // A.CallTo(() => factory.Create(FileStorageProvider.AzureBlob).Returns(fileStorageMock);
        // ...  services.AddSingleton(factory);

        // TODO: Setup authentication

        builder.ConfigureTestServices(services => { serviceCollection?.Invoke(services); });
        Server = new TestServer(builder);
        Client = Server.CreateClient();
        SetupTestDb(Server.Services.GetService<TDbContext>());
    }

    protected abstract void SetupTestDb(TDbContext dbContext);


    protected async Task<HttpResponseMessage> PostAsync(string url, object content)
    {
        url = AppendBaseUrl(url);
        var httpContent = GetAsHttpContent(content);
        return await Client.PostAsync(url, httpContent);
    }


    protected virtual async Task<T> GetAsyncAndDeserialize<T>(string url)
    {
        url = AppendBaseUrl(url);

        var response = await Client.GetAsync(url);
        return await Deserialize<T>(response);
    }

    protected async Task<T> PostAsyncAndDeserialize<T>(string url, object content)
    {
        url = AppendBaseUrl(url);
        var response = await PostAsync(url, content);
        return await Deserialize<T>(response);
    }

    protected async Task PutAsyncAndDeserialize<T>(string url, object content)
    {
        url = AppendBaseUrl(url);
        var httpContent = GetAsHttpContent(content);

        var response = await Client.PutAsync(url, httpContent);
        response.EnsureSuccessStatusCode();
    }

    protected async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        url = AppendBaseUrl(url);
        return await Client.DeleteAsync(url);
    }


    private static async Task<T> Deserialize<T>(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        return JsonSerializer.Deserialize<T>(responseContent, options);
    }


    private static HttpContent GetAsHttpContent(object content)
    {
        return content as HttpContent ??
               new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
    }

    private string AppendBaseUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(BaseUrl)) return url;

        if (string.IsNullOrWhiteSpace(url) || url.StartsWith("?") || url.StartsWith("/"))
        {
            return $"/{BaseUrl}{url}";
        }

        return $"/{BaseUrl}/{url}";
    }
}