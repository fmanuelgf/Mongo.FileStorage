namespace MongoDB.BlobStorage.Tests.Repositories
{
    using MongoDB.BlobStorage.Repositories;
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
            var response = await this.UploadFileAsync("image01.png");

            // Assert
            Assert.That(response, Is.InstanceOf<ObjectId>());
        }

        [Test]
        public async Task CanDownloadAFileByIdAsync()
        {
            // Arrange
            var fileId = await this.UploadFileAsync("image02.png");
            
            // Act
            var file = await this.blobStorageRepository.DownloadAsync(fileId);

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.GreaterThan(0));

        }

        [Test]
        public async Task CanDownloadAFileByNameAsync()
        {
            // Arrange
            var fileName = "image03.png";
            await this.UploadFileAsync(fileName);
            
            // Act
            var file = await this.blobStorageRepository.DownloadAsync(fileName);

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.GreaterThan(0));
        }

        [Test]
        public async Task CanGetAFileByIdAsync()
        {
            // Arrange
            var fileName = "image04.png";
            var fileId = await this.UploadFileAsync(fileName);
            
            // Act
            var file = await this.blobStorageRepository.GetAsync(fileId);

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
            var fileName = "image05.png";
            var fileId = await this.UploadFileAsync(fileName);
            
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
            var fileId = await this.UploadFileAsync("image06.png");

            // Act
            await this.blobStorageRepository.DeleteAsync(fileId);

            // Assert
            Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.blobStorageRepository.DownloadAsync(fileId)
            );
        }

        public void Dispose()
        {
            var client = new MongoClient(AppConfig.ConnectionString);
            client.DropDatabase(AppConfig.DatabaseName);
        }

        private async Task<ObjectId> UploadFileAsync(string fileName)
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