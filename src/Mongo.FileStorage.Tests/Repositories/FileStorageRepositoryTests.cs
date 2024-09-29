namespace Mongo.FileStorage.Tests.Repositories
{
    using Mongo.FileStorage.Repositories;
    using Mongo.FileStorage.Tests.IoC;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;
    using ZstdSharp.Unsafe;

    public class FileStorageRepositoryTests : IDisposable
    {
        private readonly IFileStorageRepository fileStorageRepository;

        public FileStorageRepositoryTests()
        {
            TestConfig.Configure();
            Dependencies.Configure();
            
            this.fileStorageRepository = Dependencies.GetRequiredService<IFileStorageRepository>();
        }
        
        [Test]
        public async Task CanUploadAFileAsync()
        {
            // Arrange
            // Act
            var fileId = await this.CreateAndUploadFileAsync("image01.jpg");

            // Assert
            Assert.That(fileId, Is.InstanceOf<ObjectId>());
            Assert.That(fileId.Timestamp, Is.GreaterThan(0));
        }

        [Test]
        public async Task CanDownloadAFileByIdAsync()
        {
            // Arrange
            var fileId = await this.CreateAndUploadFileAsync("image02.jpg");
            
            // Act
            var file = await this.fileStorageRepository.DownloadAsync(fileId.ToString());

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.GreaterThan(0));

        }

        [Test]
        public async Task CanDownloadAFileByNameAsync()
        {
            // Arrange
            var fileName = "image03.jpg";
            await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.fileStorageRepository.DownloadAsync(fileName);

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.GreaterThan(0));
        }

        [Test]
        public async Task CanGetAFileByIdAsync()
        {
            // Arrange
            var fileName = "image04.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.fileStorageRepository.GetAsync(fileId.ToString());

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.GreaterThan(0));
            Assert.That(file.Id, Is.EqualTo(fileId));
            Assert.That(file.Filename, Is.EqualTo(fileName));
        }

        [Test]
        public async Task CanGetAFileByNameAsync()
        {
            // Arrange
            var fileName = "image05.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.fileStorageRepository.GetAsync(fileName);

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.GreaterThan(0));
            Assert.That(file.Id, Is.EqualTo(fileId));
            Assert.That(file.Filename, Is.EqualTo(fileName));
        }

        [TestCase("objectId")]
        [TestCase("string")]
        public async Task CanDeleteAFileAsync(string type)
        {
            // Arrange
            var fileId = await this.CreateAndUploadFileAsync("image06.jpg");

            // Act
            if (type == "string")
            {
                await this.fileStorageRepository.DeleteAsync(fileId.ToString());
            }
            else
            {
                await this.fileStorageRepository.DeleteAsync(fileId);
            }

            // Assert
            Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.fileStorageRepository.DownloadAsync(fileId.ToString())
            );
        }

        public void Dispose()
        {
            var client = new MongoClient(AppConfig.ConnectionString);
            client.DropDatabase(AppConfig.DatabaseName);
        }

        private async Task<ObjectId> CreateAndUploadFileAsync(string fileName)
        {
            var filePath = $"TestFiles/{fileName}";
            File.Copy($"TestFiles/Robby-Robot.jpg", filePath);
            
            var fs = File.OpenRead(filePath);
            var fileId = await this.fileStorageRepository.UploadAsync(fs);
            File.Delete(filePath);
            
            return fileId;
        }
    }
}