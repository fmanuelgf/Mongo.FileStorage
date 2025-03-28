namespace Mongo.FileStorage.Tests.Repositories.FileStorageRepository
{
    using Mongo.FileStorage.Tests.Repositories.Base;
    using Mongo.FileStorage.Tests.Setup;
    using MongoDB.Bson;
    using MongoDB.Driver.GridFS;

    public class DownloadTests : TestsBase
    {
        public DownloadTests()
            : base()
        {
        }

        [Category("Happy Path")]
        [Test]
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

        [Category("Happy Path")]
        [TestCase("by Id")]
        [TestCase("by Name")]
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

        [Category("Happy Path")]
        [Test]
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
        
        [Category("Happy Path")]
        [TestCase("by Id")]
        [TestCase("by Name")]
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

        [Category("Happy Path")]
        [Test]
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

        [Category("Happy Path")]
        [TestCase("by Id")]
        [TestCase("by Name")]
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

        [Category("Happy Path")]
        [Test]
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

        [Category("Unhappy Path")]
        [TestCase("by Id")]
        [TestCase("by Name")]
        public void CannotDownloadAsStreamAnUnexistingFile(string option)
        {
            // Arrange
            // Act
            // Assert
            Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.FilesRepository.DownloadAsStreamAsync(
                    option == "by Id"
                        ? ObjectId.GenerateNewId().ToString()
                        : "unexisting-file.txt"
                )
            );
        }

        [Category("Unhappy Path")]
        [TestCase("by Id")]
        [TestCase("by Name")]
        public void CannotDownloadAsByteArrayAnUnexistingFile(string option)
        {
            // Arrange
            // Act
            // Assert
            Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.FilesRepository.DownloadAsByteArrayAsync(
                    option == "by Id"
                        ? ObjectId.GenerateNewId().ToString()
                        : "unexisting-file.txt"
                )
            );
        }
    }
}