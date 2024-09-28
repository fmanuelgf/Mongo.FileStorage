namespace Mongo.FileStorage.Tests.IoC
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Mongo.FileStorage.DependencyInjection;

    public static class Dependencies
    {
        private static ServiceCollection services = new();
        private static IServiceProvider provider = services.BuildServiceProvider();

        public static T GetRequiredService<T>() where T : notnull
        {
            return (T)provider.GetRequiredService(typeof(T));
        }
        
        public static void Configure()
        {
            services = new ServiceCollection();
            services.ResisterFileStorageRepository(RegisterMode.Transient);

            provider = services.BuildServiceProvider();
        }
    }
}