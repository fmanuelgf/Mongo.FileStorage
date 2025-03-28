namespace Mongo.FileStorage.Tests.Repositories.FileStorageRepository
{
    using Mongo.FileStorage.Tests.Repositories.Base;
    using MongoDB.Bson;

    public class UploadTests : TestsBase
    {
        public UploadTests()
            : base()
        {
        }

        [Category("Happy Path")]
        [Test]
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
    }
}