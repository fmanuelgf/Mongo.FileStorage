namespace Mongo.FileStorage.Tests.Repositories
{
    using Mongo.FileStorage.Tests.Repositories.Base;
    using Mongo.FileStorage.Tests.Setup;
    using MongoDB.Bson;
    using MongoDB.Driver.GridFS;

    public class FileStorageRepositoryTests : TestsBase
    {
        public FileStorageRepositoryTests()
            : base()
        {
        }

        [Test]
        [Category("Happy Path")]
        public void CanGetTheBucket()
        {
            // Arrange
            // Act
            var bucket = this.FilesRepository.Bucket;

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
            var file = await this.FilesRepository.DownloadAsStreamAsync(
                option == "by Id"
                    ? fileId.ToString()
                    : fileName
            );

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.EqualTo(10992));
        }

        [Test]
        [Category("Happy Path")]
        public async Task CanDownloadAFileByObjectIdAsStreamAsync()
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.FilesRepository.DownloadAsStreamAsync(fileId);

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
            var file = await this.FilesRepository.DownloadAsByteArrayAsync(
                option == "by Id"
                    ? fileId.ToString()
                    : fileName
            );

            // Assert
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Length, Is.EqualTo(10992));
        }

        [Test]
        [Category("Happy Path")]
        public async Task CanDownloadAFileByObjectIdAsByteArrayAsync()
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);
            
            // Act
            var file = await this.FilesRepository.DownloadAsByteArrayAsync(fileId);

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
            var file = await this.FilesRepository.GetFileInfoAsync(
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
            var file = await this.FilesRepository.GetFileInfoAsync(fileId);

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
                    await this.FilesRepository.DeleteAsync(fileId.ToString());
                    break;
                default:
                    await this.FilesRepository.DeleteAsync(fileId);
                    break;
            }

            // Assert
            Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.FilesRepository.DownloadAsStreamAsync(fileId.ToString())
            );
        }

        [TestCase("by Id")]
        [TestCase("by Name")]
        [Category("Unhappy Path")]
        public void CannotDownloadAsStreamAnUnexistingFile(string option)
        {
            // Arrange
            // Act
            // Assert
            var ex = Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.FilesRepository.DownloadAsStreamAsync(
                    option == "by Id"
                        ? ObjectId.GenerateNewId().ToString()
                        : "unexisting-file.txt"
                )
            );
        }

        [TestCase("by Id")]
        [TestCase("by Name")]
        [Category("Unhappy Path")]
        public void CannotDownloadAsByteArrayAnUnexistingFile(string option)
        {
            // Arrange
            // Act
            // Assert
            var ex = Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.FilesRepository.DownloadAsByteArrayAsync(
                    option == "by Id"
                        ? ObjectId.GenerateNewId().ToString()
                        : "unexisting-file.txt"
                )
            );
        }

        [Test]
        [Category("Unhappy Path")]
        public void CannotDeleteByInvalidId()
        {
            // Arrange
            // Act
            // Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await this.FilesRepository.DeleteAsync("foo")
            );
            Assert.That(ex.Message, Is.EqualTo($"'foo' is not a valid ObjectId"));
        }

        [Test]
        [Category("Unhappy Path")]
        public void CannotDeleteByUnexistingId()
        {
            // Arrange
            // Act
            // Assert
            var ex = Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.FilesRepository.DeleteAsync(ObjectId.GenerateNewId())
            );
        }
    }
}