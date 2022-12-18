using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.StaticFiles;
using SmartBuilding.FaceApp.Data;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddTransient<AzureBlobHelper>();
builder.Services.AddTransient<FaceService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET, PATCH, DELETE, PUT, POST, OPTIONS"));
});
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.MaximumReceiveMessageSize = 128 * 1024; // 1MB
});
var configBuilder = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json", optional: false);
IConfiguration Configuration = configBuilder.Build();

//AppConstants.ProxyIP = Configuration["ProxyIP"];

//var proxies = AppConstants.ProxyIP.Split(';');
//builder.Services.Configure<ForwardedHeadersOptions>(options =>
//{
//    foreach (var proxy in proxies)
//    {
//        options.KnownProxies.Add(IPAddress.Parse(proxy));
//    }
//});

AppConstants.UploadUrlPrefix = Configuration["UploadUrlPrefix"];
AppConstants.SQLConn = Configuration["ConnectionStrings:SqlConn"];
AppConstants.RedisCon = Configuration["RedisCon"];
AppConstants.BlobConn = Configuration["ConnectionStrings:BlobConn"];
AppConstants.GMapApiKey = Configuration["GmapKey"];

var app = builder.Build();

//var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();

//provider.Mappings.Add(new KeyValuePair<string, string>(".bin", "application/octet-stream"));



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

/*
app.UseStaticFiles(new StaticFileOptions()
{
    ContentTypeProvider = provider
});
*/
app.UseStaticFiles();

app.UseRouting();

app.UseCors(x => x
.AllowAnyMethod()
.AllowAnyHeader()
.SetIsOriginAllowed(origin => true) // allow any origin  
.AllowCredentials());               // allow credentials 

// BLAZOR COOKIE Auth Code (begin)
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

var db = new SmartbuildingDb();
db.Database.EnsureCreated();

app.Run();
