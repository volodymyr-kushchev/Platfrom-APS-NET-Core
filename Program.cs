using Platform;
using Platform.Endpoints;
using Platform.Middelwares;
using Platform.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.Configure<MessageOptions>(options => {
//    options.CityName = "Albany";
//});

//builder.Services.AddScoped<IResponseFormatter, TextResponseFormatter>();
//builder.Services.AddScoped<IResponseFormatter, HtmlResponseFormatter>();
//builder.Services.AddScoped<IResponseFormatter, GuidService>();

var app = builder.Build();
app.UseHttpLogging();
app.UseStaticFiles();

//app.MapGet("{first}/{second}/{third}", async context => {
//    await context.Response.WriteAsync("Request Was Routed\n");
//    foreach (var kvp in context.Request.RouteValues)
//    {
//        await context.Response
//        .WriteAsync($"{kvp.Key}: {kvp.Value}\n");
//    }
//});

//app.MapGet("capital/{country}", Capital.Endpoint);
//app.MapGet("size/{city}", Population.Endpoint)
//    .WithMetadata(new RouteNameMetadata("population"));
//app.MapGet("routing", async context =>
//{
//    await context.Response.WriteAsync("Request was Routed");
//});

//app.UseMiddleware<WeatherMiddleware>();

//IResponseFormatter formatter = new TextResponseFormatter();

//app.MapGet("single", async context => {
//    IResponseFormatter formatter = context.RequestServices
//    .GetRequiredService<IResponseFormatter>();
//    await formatter.Format(context, "Single service");
//});

//app.MapGet("/", async context => {
//    IResponseFormatter formatter = context.RequestServices
//    .GetServices<IResponseFormatter>().First(f => f.RichOutput);
//    await formatter.Format(context, "Multiple services");
//});

var pipelineConfig = app.Configuration;
// - use configuration settings to set up pipeline
var pipelineEnv = app.Environment;
// - use envirionment to set up pipeline

app.MapGet("config", async (HttpContext context,
         IConfiguration config, IWebHostEnvironment env) => {
     string defaultDebug = config["Logging:LogLevel:Default"];

     await context.Response
     .WriteAsync($"The config setting is: {defaultDebug}");
     await context.Response
     .WriteAsync($"\nThe env setting is: {env.EnvironmentName}");

     string wsID = config["WebService:Id"];
     string wsKey = config["WebService:Key"];

     await context.Response.WriteAsync($"\nThe secret ID is: {wsID}");
     await context.Response.WriteAsync($"\nThe secret Key is: {wsKey}");
 });

app.MapGet("/", async context => {
    await context.Response.WriteAsync("Hello World!");
});

var env = app.Environment;
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new
 PhysicalFileProvider($"{env.ContentRootPath}/staticfiles"),
    RequestPath = "/files"
});

app.Run();
