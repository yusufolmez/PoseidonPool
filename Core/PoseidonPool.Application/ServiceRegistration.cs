﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PoseidonPool.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection collection)
        {
            collection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
            collection.AddHttpClient();
        }
    }
}