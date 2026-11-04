using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Order.Data
{
    public static class OrderDataExtension
    {
        public static IServiceCollection AddOrderData(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(OrderDataExtension).Assembly);
            return services;
        }
    }
}

