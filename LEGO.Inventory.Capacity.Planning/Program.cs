using LEGO.Inventory.Capacity.Planning.Services;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IGoodsReceiptService, GoodsReceiptService>();
builder.Services.AddTransient<ISalesOrderService, SalesOrderService>();
builder.Services.AddTransient<IStockTransportOrderService, StockTransportOrderService>();
builder.Services.AddTransient<IPreparationService, PreparationService>();
builder.Services.AddTransient<IRegionalDistributionCenterService, RegionalDistributionCenterService>();
builder.Services.AddSingleton<IStorage, Storage>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
