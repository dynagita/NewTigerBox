using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewTigerBox.Domain.Repositories;
using NewTigerBox.Infrastructure.Messaging.Configuration;
using NewTigerBox.Infrastructure.Messaging.MediaQueue;
using NewTigerBox.Infrastructure.Repository;
using NewTigerBox.Infrastructure.Repository.Context;

namespace NewTigerBox.Infrastructure.IoC
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddInfraBootstrapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration(configuration);
            services.AddContext(configuration);
            services.AddRepositories();
            services.AddRabbitMQ();
            return services;
        }

        private static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitConfiguration>(config =>
            {
                configuration.Bind(nameof(RabbitConfiguration), config);
            });

            return services;
        }

        private static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<NewTigerBoxContext>(option => option.UseSqlServer(configuration.GetConnectionString("NewTigerBox")));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();

            return services;
        }

        private static IServiceCollection AddRabbitMQ(this IServiceCollection services)
        {
            services.AddScoped<IMediaConsumer, MediaConsumer>();
            services.AddScoped<IMediaProducer, MediaProducer>();
            return services;
        }
    }
}
