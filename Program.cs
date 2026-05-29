using ApprenticeshipManagement.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Build connection string from environment variables or config
var dbServer = Environment.GetEnvironmentVariable("DB_SERVER") ?? builder.Configuration["ConnectionStrings:DbServer"] ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? builder.Configuration["ConnectionStrings:DbPort"] ?? "3306";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? builder.Configuration["ConnectionStrings:DbName"] ?? "drdo_apprenticeship";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? builder.Configuration["ConnectionStrings:DbUser"] ?? "root";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? builder.Configuration["ConnectionStrings:DbPassword"] ?? "";

var connectionString = $"Server={dbServer};Port={dbPort};Database={dbName};User={dbUser};Password={dbPassword};";

builder.Services.AddDbContext<InternshipDb>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InternshipDb>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
