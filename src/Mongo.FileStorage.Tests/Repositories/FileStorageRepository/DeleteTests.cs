namespace Mongo.FileStorage.Tests.Repositories.FileStorageRepository
{
    using Mongo.FileStorage.Tests.Repositories.Base;
    using MongoDB.Bson;
    using MongoDB.Driver.GridFS;

    public class DeleteTests : TestsBase
    {
        public DeleteTests()
            : base()
        {
        }

        [Category("Happy Path")]
        [TestCase("ObjectId")]
        [TestCase("string")]
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

        [Category("Happy Path")]
        [TestCase("Array of ObjectIds")]
        [TestCase("List of ObjectIds")]
        [TestCase("Array of strings")]
        [TestCase("List of strings")]
        public async Task CanDeleteAnArrayOfFileByIdAsync(string type)
        {
            // Arrange
            var fileName = $"{this.RandomString(5)}.jpg";
            var fileIds = new List<ObjectId>();

            for (var n = 1; n < 6; n++)
            {
                var id = await this.CreateAndUploadFileAsync(fileName);
                fileIds.Add(id);
            }

            // Act
            switch (type)
            {
                case "Array of ObjectIds":
                    await this.FilesRepository.DeleteAsync(fileIds.Select(x => x.ToString()).ToArray());
                    break;
                case "List of ObjectIds":
                    await this.FilesRepository.DeleteAsync(fileIds);
                    break;
                case "Array of strings":
                    await this.FilesRepository.DeleteAsync(fileIds.Select(x => x.ToString()).ToArray());
                    break;
                case "List of strings":
                    await this.FilesRepository.DeleteAsync(fileIds.Select(x => x.ToString()).ToList());
                    break;
                default:
                    throw new NotImplementedException($"'{nameof(type)}' is not implemented.");
            }

            // Assert
            foreach (var fileId in fileIds)
            {
                Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                    await this.FilesRepository.DownloadAsStreamAsync(fileId)
                );
            }
        }

        [Category("Unhappy Path")]
        [Test]
        public void CannotDeleteByInvalidId()
        {
            // Arrange
            // Act
            // Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await this.FilesRepository.DeleteAsync("foo")
            );
            Assert.That(ex?.Message, Is.EqualTo($"'foo' is not a valid ObjectId"));
        }

        [Category("Unhappy Path")]
        [Test]
        public void CannotDeleteByUnexistingId()
        {
            // Arrange
            // Act
            // Assert
            Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.FilesRepository.DeleteAsync(ObjectId.GenerateNewId())
            );
        }

        [Category("Unhappy Path")]
        [TestCase("As Array")]
        [TestCase("As List")]
        public void CannotDeleteAnArrayByUnexistingId(string type)
        {
            // Arrange
            var fileIds = new List<ObjectId>
            {
                ObjectId.GenerateNewId(),
                ObjectId.GenerateNewId()
            };
            
            // Act
            // Assert
            Assert.ThrowsAsync<GridFSFileNotFoundException>(async () =>
                await this.FilesRepository.DeleteAsync(type == "As Array"
                    ? fileIds.ToArray()
                    : fileIds)
            );
        }
    }
}