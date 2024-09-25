namespace Mongo.BlobStorage.Repositories
{
    using Mongo.BlobStorage;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;

    public class BlobStorageRepository : IBlobStorageRepository
    {
        private readonly GridFSBucket bucket;
        
        public BlobStorageRepository()
        {
            var client = new MongoClient(AppConfig.ConnectionString);
            var database = client.GetDatabase(AppConfig.DatabaseName);
            var options = new GridFSBucketOptions
            {
                BucketName = AppConfig.BucketName,
                ChunkSizeBytes = AppConfig.ChunkSizeBytes
            };
            
            this.bucket = new(database, options);
        }

        public async Task<ObjectId> UploadAsync(FileStream fileStream)
        {
            var fileName = Path.GetFileName(fileStream.Name);
            return await this.bucket.UploadFromStreamAsync(filename: fileName, source: fileStream);
        }

        public async Task<MemoryStream> DownloadAsync(string idOrName)
        {
            var stream = new MemoryStream();
            if (ObjectId.TryParse(idOrName, out var fileId))
            {
                await this.bucket.DownloadToStreamAsync(fileId, stream);
            }
            else
            {
                await this.bucket.DownloadToStreamByNameAsync(idOrName, stream);
            }
            
            stream.Position = 0;
            return stream;
        }

        public async Task<GridFSFileInfo<ObjectId>> GetAsync(string idOrName)
        {
            var stream = new MemoryStream();
            var filter = ObjectId.TryParse(idOrName, out var fileId)
                ? Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(x => x.Id, fileId)
                : Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(x => x.Filename, idOrName);
            
            var cursor = await this.bucket.FindAsync(filter);
            return cursor.FirstOrDefault();
        }

        public async Task DeleteAsync(ObjectId fileId)
        {
            await this.bucket.DeleteAsync(fileId);
        }
    }
}