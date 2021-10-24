using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.File
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterFileStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<MyDbContext>();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddOptions<Settings>("FileStorage");
            services.AddSingleton(configuration.GetSection("FileStorage").Get<Settings>());

            return services;
        }
    }
}

