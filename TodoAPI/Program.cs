using Contracs;
using NLog;
using TodoAPI.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using TodoAPI;
using TodoAPI.Presentation.ActionFilters;
using Shared.DataTransferObjects;
using Service.DataShaping;
using TodoAPI.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LogManager.Setup().LoadConfigurationFromFile(
    string.Concat(Directory.GetCurrentDirectory(), "\\nlog.config"));

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureApiVersioning();
// builder.Services.ConfigureResponseCache();
builder.Services.ConfigureOutputCache();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<IDataShaper<UserDto>, DataShaper<UserDto>>();
builder.Services.AddScoped<IDataShaper<TodoDto>, DataShaper<TodoDto>>();
builder.Services.AddScoped<ValidateMediaTypeAttribute>();
builder.Services.AddScoped<IUserLinks, UserLinks>();
// after .NET 8
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
        config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
        config.CacheProfiles.Add("120SecondsDuration", new CacheProfile() { Duration = 120 });
    }).AddXmlDataContractSerializerFormatters()
    .AddCustomCsvFormatter()
    .AddApplicationPart(typeof(TodoAPI.Presentation.AssemblyReference).Assembly);

builder.Services.AddCustomMediaType();

var app = builder.Build();
// before .NET 8 we use this for global handling exception :
// var logger = app.Services.GetRequiredService<ILoggerManager>();
// app.ConfigureExceptionHandler(logger);

// after .NET 8
app.UseExceptionHandler(opt => { });

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");
// app.UseResponseCaching();
app.UseOutputCache();

app.UseAuthorization();


app.MapControllers();


// Use and Run methods
// Run method is for terminal middlewares
// Use for middleware in general


//app.Map("/usingmapbranch", builder =>
//{
//    builder.Use(async (context, next) =>
//    {
//        Console.WriteLine("Map method for branch logic before next method");
//        await next.Invoke();
//        Console.WriteLine("Map method for branch logic after next method");
//    });

//    builder.Run(async context =>
//    {
//        Console.WriteLine("Map method for branch writing response in Run method");
//        await context.Response.WriteAsync("Hello from map method for branch");
//    });
//});

//app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), b =>
//{
//    b.Run(async context =>
//    {
//        await context.Response.WriteAsync("Hello from MapWhen method.");

//    });
//});

//app.Use(async (context, next) =>
//{
//    Console.WriteLine("Logic before next in Use method");
//    await next.Invoke();
//    Console.WriteLine("Logic after next in Use method");

//});

//app.Run(async context =>
//{
//    Console.WriteLine("Writing Response in Run method");
//    await context.Response.WriteAsync("Writing Response in Run method");
//});

app.Run();
return;

//NewtonsoftJson
NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
{
    return new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson().Services.BuildServiceProvider()
        .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>()
        .First();
}
