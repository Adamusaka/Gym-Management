using web_frontend.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped((sp) => {
    HttpClientHandler clientHandler = new HttpClientHandler();
    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

    HttpClient _httpClient = new HttpClient(clientHandler);
    _httpClient.BaseAddress = new Uri(builder.Configuration.GetSection("BaseURL:backend").Value);
    _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
    _httpClient.DefaultRequestHeaders.Add("User-Agent", "gymmanagement/1.0");
    _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
    _httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", builder.Configuration.GetSection("BaseURL:backend").Value);

    return _httpClient;
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
