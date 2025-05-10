namespace Mongo.FileStorage.Tests.Setup
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Mongo.FileStorage.DependencyInjection;
    using MongoDB.Driver;

    internal class Dependencies
    {
        private readonly ServiceCollection services;
        private readonly IServiceProvider provider ;

        internal Dependencies()
        {
            this.services = new ServiceCollection();

            this.services.RegisterFileStorageRepository(
                RegisterMode.Singleton,
                ReadConcern.Default,
                ReadPreference.Primary,
                WriteConcern.WMajority);

            this.provider = this.services.BuildServiceProvider();
        }
        
        internal T GetRequiredService<T>() where T : notnull
        {
            return (T)this.provider.GetRequiredService(typeof(T));
        }
    }
}