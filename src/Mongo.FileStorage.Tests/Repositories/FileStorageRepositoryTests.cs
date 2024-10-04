namespace Mongo.FileStorage.Tests.Repositories
{
    using Mongo.FileStorage.Repositories;
    using Mongo.FileStorage.Tests.IoC;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;

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

        [TestCase("by Id")]
        [TestCase("by Name")]
        public async Task CanDownloadAFileAsStreamAsync(string option)
        {
            // Arrange
            var fileName = "image02.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.fileStorageRepository.DownloadAsStreamAsync(
                option == "by Id"
                    ? fileId.ToString()
                    : fileName
            );

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.EqualTo(10992));
        }

        [TestCase("by Id")]
        [TestCase("by Name")]
        public async Task CanDownloadAFileAsByteArrayAsync(string option)
        {
            // Arrange
            var fileName = "image03.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.fileStorageRepository.DownloadAsByteArrayAsync(
                option == "by Id"
                    ? fileId.ToString()
                    : fileName
            );

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.EqualTo(10992));
        }

        [Test]
        public async Task CanGetAFileInfoByIdAsync()
        {
            // Arrange
            var fileName = "image04.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.fileStorageRepository.GetFileInfoAsync(fileId.ToString());

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.EqualTo(10992));
            Assert.That(file.Id, Is.EqualTo(fileId));
            Assert.That(file.Filename, Is.EqualTo(fileName));
        }

        [Test]
        public async Task CanGetAFileInfoByNameAsync()
        {
            // Arrange
            var fileName = "image05.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.fileStorageRepository.GetFileInfoAsync(fileName);

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.EqualTo(10992));
            Assert.That(file.Id, Is.EqualTo(fileId));
            Assert.That(file.Filename, Is.EqualTo(fileName));
        }

        [TestCase("ObjectId")]
        [TestCase("string")]
        public async Task CanDeleteAFileAsync(string type)
        {
            // Arrange
            var fileId = await this.CreateAndUploadFileAsync("image06.jpg");

            // Act
            switch (type)
            {
                case "string":
                    await this.fileStorageRepository.DeleteAsync(fileId.ToString());
                    break;
                default:
                    await this.fileStorageRepository.DeleteAsync(fileId);
                    break;
            }

            // Assert
            Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.fileStorageRepository.DownloadAsStreamAsync(fileId.ToString())
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