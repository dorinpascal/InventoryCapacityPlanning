using LEGO.Inventory.Capacity.Planning.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders(); // Clear default providers

builder.Host.UseSerilog((context, services, configuration) => configuration
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(context.Configuration)
);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddApplicationServices();
builder.Services.AddStorageServices();
builder.Services.AddAutoMapperProfiles();


builder.WebHost.UseUrls("http://*:5100");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
