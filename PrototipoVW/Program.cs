using PrototipoVW.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<ApiClient>(client =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

    if (string.IsNullOrWhiteSpace(apiBaseUrl))
    {
        throw new Exception("No se configuró ApiBaseUrl en appsettings.json.");
    }

    client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "invalid",
    pattern: "{*url}",
    defaults: new { controller = "Error", action = "InvalidRoute" });

app.Run();