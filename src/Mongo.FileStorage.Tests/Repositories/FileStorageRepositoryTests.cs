namespace Mongo.FileStorage.Tests.Repositories
{
    using Mongo.FileStorage.Repositories;
    using Mongo.FileStorage.Tests.Setup;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;

    public class FileStorageRepositoryTests : IDisposable
    {
        private readonly IFileStorageRepository fileStorageRepository;
        private readonly Random rnd;

        public FileStorageRepositoryTests()
        {
            TestSetup.Configure();
            
            this.fileStorageRepository = TestSetup.Dependencies.GetRequiredService<IFileStorageRepository>();
            this.rnd = new Random((int)DateTime.UtcNow.Ticks);
        }

        [Test]
        [Category("Happy Path")]
        public void CanGetTheBucket()
        {
            // Arrange
            // Act
            var bucket = this.fileStorageRepository.Bucket;

            // Assert
            Assert.That(bucket.Database.DatabaseNamespace.DatabaseName, Is.EqualTo(TestConstants.DatabaseName));
            Assert.That(bucket.Options.BucketName, Is.EqualTo(TestConstants.BucketName));
            Assert.That(bucket.Options.ChunkSizeBytes, Is.EqualTo(TestConstants.ChunkSizeBytes));
        }
        
        [Test]
        [Category("Happy Path")]
        public async Task CanUploadAFileAsync()
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
            
            // Act
            var fileId = await this.CreateAndUploadFileAsync(fileName);

            // Assert
            Assert.That(fileId, Is.InstanceOf<ObjectId>());
            Assert.That(fileId.Timestamp, Is.GreaterThan(0));
        }

        [TestCase("by Id")]
        [TestCase("by Name")]
        [Category("Happy Path")]
        public async Task CanDownloadAFileByIdOrNameAsStreamAsync(string option)
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
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

        public async Task CanDownloadAFileByObjectIdAsStreamAsync()
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.fileStorageRepository.DownloadAsStreamAsync(fileId);

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.EqualTo(10992));
        }

        [TestCase("by Id")]
        [TestCase("by Name")]
        [Category("Happy Path")]
        public async Task CanDownloadAFileByIdOrNameAsByteArrayAsync(string option)
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
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

        public async Task CanDownloadAFileByObjectIdAsByteArrayAsync()
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.fileStorageRepository.DownloadAsByteArrayAsync(fileId);

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.EqualTo(10992));
        }

        [TestCase("by Id")]
        [TestCase("by Name")]
        [Category("Happy Path")]
        public async Task CanGetAFileInfoByIdOrNameAsync(string option)
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.fileStorageRepository.GetFileInfoAsync(
                option == "by Id"
                    ? fileId.ToString()
                    : fileName
            );

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.EqualTo(10992));
            Assert.That(file.Id, Is.EqualTo(fileId));
            Assert.That(file.Filename, Is.EqualTo(fileName));
        }

        [Test]
        [Category("Happy Path")]
        public async Task CanGetAFileInfoByObjectIdAsync()
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.fileStorageRepository.GetFileInfoAsync(fileId);

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.EqualTo(10992));
            Assert.That(file.Id, Is.EqualTo(fileId));
            Assert.That(file.Filename, Is.EqualTo(fileName));
        }

        [TestCase("ObjectId")]
        [TestCase("string")]
        [Category("Happy Path")]
        public async Task CanDeleteAFileByIdAsync(string type)
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);

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

        [Test]
        [Category("Unhappy Path")]
        public async Task CannotDeleteByInvalidIdAsync()
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);

            // Act
            // Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await this.fileStorageRepository.DeleteAsync("foo")
            );
            Assert.That(ex.Message, Is.EqualTo($"'foo' is not a valid ObjectId"));
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

        private string RandomString(int length)
        {
            return new string(Enumerable
                .Repeat("abcdefghijklmnopqrstuvwxyz0123456789", length)
                .Select(s => s[this.rnd.Next(s.Length)])
                .ToArray()
            );
        }
    }
}