using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Storage;
using Webshop.Controllers;
using System.Configuration;
using Models;
using Models.ViewModels;
using System.Net.Http.Headers;
using System.Reflection;
using SwedbankPay.Sdk.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, options =>
    {
        options.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
    }));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ShoppingCartManager, ShoppingCartManager>();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "ShoppingCart";
    options.IdleTimeout = TimeSpan.FromDays(3);
    options.Cookie.IsEssential = true;
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var swedbankPayConSettings = builder.Configuration.GetSection("SwedbankPay");
builder.Services.Configure<SwedbankPayConnectionSettings>(swedbankPayConSettings);

var swedbankPayOptions = swedbankPayConSettings.Get<SwedbankPayConnectionSettings>();
builder.Services.AddSingleton(s => swedbankPayOptions);

builder.Services.Configure<PayeeInfoConfig>(options =>
{
    options.PayeeId = swedbankPayOptions.PayeeId;
    options.PayeeReference = DateTime.Now.Ticks.ToString();
});

builder.Services.Configure<UrlsOptions>(builder.Configuration.GetSection("Urls"));
builder.Services.AddScoped(provider => SessionCart.GetCart(provider));
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

void configureClient(HttpClient a)
{
    a.BaseAddress = swedbankPayOptions.ApiBaseUrl;
    a.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", swedbankPayOptions.Token);
    a.DefaultRequestHeaders.Add("User-Agent", $"swedbankpay-webshop/{Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version}");
}

builder.Services.AddSwedbankPayClient(configureClient);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
