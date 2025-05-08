using Serilog;
using Serilog.Formatting.Compact;

Log.Information("building service");
// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Debug()
//     .WriteTo.Console()
//     .CreateLogger();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.Console(new CompactJsonFormatter());
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

Log.Information("setup http request pipeline");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/", () =>
    {
        var info = new Dictionary<string, int>{ { "foo", 1 }, { "bar", 2 }, { "baz", 3}, { "que", 4 } };
        // Log.Information("hit index");
        Log.Information("data: {@data}", info);
        return info;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

Log.Information("run app");
app.Run();
