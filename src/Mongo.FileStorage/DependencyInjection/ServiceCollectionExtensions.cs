namespace Mongo.FileStorage.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;
    using Mongo.FileStorage.Repositories;

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register the IFileStorageRepository and its implementation.
        /// </summary>
        /// <param name="registerMode">The <see cref="RegisterMode"> (Transient, Scoped, Singleton).</param>
        public static void RegisterFileStorageRepository(this IServiceCollection services, RegisterMode registerMode)
        {
            switch (registerMode)
            {
                case RegisterMode.Scoped:
                    services.AddScoped<IFileStorageRepository, FileStorageRepository>();
                    break;
                
                case RegisterMode.Singleton:
                    services.AddSingleton<IFileStorageRepository, FileStorageRepository>();
                    break;

                default:
                    services.AddTransient<IFileStorageRepository, FileStorageRepository>();
                    break;
            }
        }
    }
}