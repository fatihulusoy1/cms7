var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
v
// ApiBaseUrl config ayarý (appsettings.json veya ortam deðiþkeninden okunuyor)
string apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5001/"; // Default fallback

// HttpClient ve PostService kaydý
builder.Services.AddHttpClient<CMS.Application.Services.PostService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddScoped<CMS.Application.Services.PostService>();

builder.Services.AddHttpClient<CMS.Application.Services.CompanyService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl); // Use the same base URL
});

builder.Services.AddScoped<CMS.Application.Services.CompanyService>();

builder.Services.AddHttpClient<CMS.Application.Services.SiteService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl); // Use the same base URL
});

builder.Services.AddScoped<CMS.Application.Services.SiteService>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();

//var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
