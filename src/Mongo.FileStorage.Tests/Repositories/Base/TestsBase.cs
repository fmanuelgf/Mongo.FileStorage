namespace Mongo.FileStorage.Tests.Repositories.Base
{
    using Mongo.FileStorage.Repositories;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public abstract class TestsBase : IDisposable
    {
        private readonly Random rnd;

        protected IFileStorageRepository FilesRepository { get; }
        
        public TestsBase()
        {
            TestSetup.Configure();
            this.FilesRepository = TestSetup.Dependencies.GetRequiredService<IFileStorageRepository>();
            this.rnd = new Random((int)DateTime.UtcNow.Ticks);
        }

        public void Dispose()
        {
            var client = new MongoClient(AppConfig.ConnectionString);
            client.DropDatabase(AppConfig.DatabaseName);
        }

        protected async Task<ObjectId> CreateAndUploadFileAsync(string fileName)
        {
            var filePath = $"TestFiles/{fileName}";
            File.Copy($"TestFiles/Robby-Robot.jpg", filePath);
            
            var fs = File.OpenRead(filePath);
            var fileId = await this.FilesRepository.UploadAsync(fs);
            File.Delete(filePath);
            
            return fileId;
        }

        protected string RandomString(int length)
        {
            return new string(Enumerable
                .Repeat("abcdefghijklmnopqrstuvwxyz0123456789", length)
                .Select(s => s[this.rnd.Next(s.Length)])
                .ToArray()
            );
        }
    }
}