using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Services;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage;
using Microsoft.OpenApi.Models;

namespace LEGO.Inventory.Capacity.Planning.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IGoodsReceiptService, GoodsReceiptService>();
        services.AddTransient<ISalesOrderService, SalesOrderService>();
        services.AddTransient<IStockTransportOrderService, StockTransportOrderService>();
        services.AddTransient<ISalesPreparationService, PreparationService>();
        services.AddTransient<IRegionalDistributionCenterService, RegionalDistributionCenterService>();
        return services;
    }

    public static IServiceCollection AddStorageServices(this IServiceCollection services)
    {
        services.AddScoped<IRegionalDistributionCenterStorage, RegionalDistributionCenterStorage>();
        services.AddScoped<ILocalDistributionCenterStorage, LocalDistributionCenterStorage>();
        services.AddScoped<ISalesOrderStorage, SalesOrderStorage>();
        services.AddScoped<IStockTransportOrderStorage, StockTransportOrderStorage>();
        services.AddScoped<IGoodsReceiptStorage, GoodsReceiptStorage>();
        return services;
    }

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "LEGO Inventory Capacity Planning API",
                Description = "An API for managing inventory and stock transport orders",
            });
        });

        return services;
    }

    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}
