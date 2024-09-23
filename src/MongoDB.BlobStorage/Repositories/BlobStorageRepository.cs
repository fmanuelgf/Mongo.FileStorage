namespace MongoDB.BlobStorage.Repositories
{
    using MongoDB.BlobStorage;
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

        public async Task<MemoryStream> DownloadAsync(ObjectId fileId)
        {
            var stream = new MemoryStream();
            await this.bucket.DownloadToStreamAsync(fileId, stream);
            return stream;
        }

        public async Task<MemoryStream> DownloadAsync(string fileName)
        {
            var stream = new MemoryStream();
            await this.bucket.DownloadToStreamByNameAsync(fileName, stream);
            return stream;
        }

        public async Task<GridFSFileInfo<ObjectId>> GetAsync(ObjectId fileId)
        {
            var stream = new MemoryStream();
            var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(x => x.Id, fileId);
            var cursor = await this.bucket.FindAsync(filter);
            return cursor.FirstOrDefault();
        }

        public async Task<GridFSFileInfo> GetAsync(string fileName)
        {
            var stream = new MemoryStream();
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName);
            var cursor = await this.bucket.FindAsync(filter);
            return cursor.FirstOrDefault();
        }

        public async Task DeleteAsync(ObjectId fileId)
        {
            await this.bucket.DeleteAsync(fileId);
        }
    }
}