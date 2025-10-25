namespace Mongo.FileStorage.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;
    using Mongo.FileStorage.Repositories;
    using MongoDB.Driver;

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register the IFileStorageRepository and its implementation.
        /// </summary>
        /// <param name="registerMode">The <see cref="RegisterMode"> (Transient, Scoped, Singleton).</param>
        /// <param name="readConcern">The <see cref="ReadConcern"> or its default value.</param>
        /// <param name="readPreference">The <see cref="ReadPreference"> or its default value.</param>
        /// <param name="writeConcern">The <see cref="WriteConcern"> or its default value.</param>
        public static void RegisterFileStorageRepository(
            this IServiceCollection services,
            RegisterMode registerMode,
            ReadConcern? readConcern = default,
            ReadPreference? readPreference = default,
            WriteConcern? writeConcern = default)
        {
            switch (registerMode)
            {
                case RegisterMode.Scoped:
                    services.AddScoped<IFileStorageRepository, FileStorageRepository>(
                        x => new FileStorageRepository(readConcern, readPreference, writeConcern));
                    break;
                
                case RegisterMode.Singleton:
                    services.AddSingleton<IFileStorageRepository, FileStorageRepository>(
                        x => new FileStorageRepository(readConcern, readPreference, writeConcern));
                    break;

                default:
                    services.AddTransient<IFileStorageRepository, FileStorageRepository>(
                        x => new FileStorageRepository(readConcern, readPreference, writeConcern));
                    break;
            }
        }
    }
}