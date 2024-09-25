namespace Mongo.BlobStorage.Tests.Repositories
{
    using Mongo.BlobStorage.Repositories;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;

    public class BlobStorageRepositoryTests : IDisposable
    {
        private readonly IBlobStorageRepository blobStorageRepository;

        public BlobStorageRepositoryTests()
        {
            TestConfig.Configure();
            this.blobStorageRepository = new BlobStorageRepository();
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
            var file = await this.blobStorageRepository.DownloadAsync(fileId.ToString());

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
            var file = await this.blobStorageRepository.DownloadAsync(fileName);

            var buffer = new byte[file.Length];
            file.Read(buffer, 0, buffer.Length);
            File.WriteAllBytes("aaa.jpg", buffer);

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
            var file = await this.blobStorageRepository.GetAsync(fileId.ToString());

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
            var file = await this.blobStorageRepository.GetAsync(fileName);

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.GreaterThan(0));
            Assert.That(file.Id, Is.EqualTo(fileId));
            Assert.That(file.Filename, Is.EqualTo(fileName));
        }

        [Test]
        public async Task CanDeleteAFileAsync()
        {
            // Arrange
            var fileId = await this.CreateAndUploadFileAsync("image06.jpg");

            // Act
            await this.blobStorageRepository.DeleteAsync(fileId);

            // Assert
            Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.blobStorageRepository.DownloadAsync(fileId.ToString())
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
            var fileId = await this.blobStorageRepository.UploadAsync(fs);
            File.Delete(filePath);
            
            return fileId;
        }
    }
}