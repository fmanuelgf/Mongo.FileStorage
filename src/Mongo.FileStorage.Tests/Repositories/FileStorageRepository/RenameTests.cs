namespace Mongo.FileStorage.Tests.Repositories.FileStorageRepository
{
    using Mongo.FileStorage.Tests.Repositories.Base;
    using MongoDB.Bson;
    using MongoDB.Driver.GridFS;

    public class RenameTests : TestsBase
    {
        public RenameTests()
            : base()
        {
        }

        [Category("Happy Path")]
        [TestCase("ObjectId")]
        [TestCase("string")]
        public async Task CanRenameAFileAsync(string type)
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
            var fileId = await this.CreateAndUploadFileAsync(fileName);

            // Act
            switch (type)
            {
                case "string":
                    await this.FilesRepository.RenameAsync(fileId.ToString(), "newFileName.jpg");
                    break;
                default:
                    await this.FilesRepository.RenameAsync(fileId, "newFileName.jpg");
                    break;
            }

            // Assert
            var file = await this.FilesRepository.GetFileInfoAsync(fileId);
            Assert.That(file, Is.Not.Null);
            Assert.That(file.Id, Is.EqualTo(fileId));
            Assert.That(file.Filename, Is.EqualTo("newFileName.jpg"));
        }

        [Category("Unhappy Path")]
        [Test]
        public void CannotRenameByInvalidId()
        {
            // Arrange
            // Act
            // Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await this.FilesRepository.RenameAsync("foo", "newFileName.jpg")
            );
            Assert.That(ex?.Message, Is.EqualTo($"'foo' is not a valid ObjectId"));
        }

        [Category("Unhappy Path")]
        [Test]
        public void CannotRenameByUnexistingId()
        {
            // Arrange
            // Act
            // Assert
            Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.FilesRepository.RenameAsync(ObjectId.GenerateNewId(), "newFileName.jpg")
            );
        }
    }
}