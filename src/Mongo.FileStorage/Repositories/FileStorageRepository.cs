namespace Mongo.FileStorage.Repositories
{
    using Mongo.FileStorage;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;

    public class FileStorageRepository : IFileStorageRepository
    {
        public GridFSBucket Bucket { get; }
        
        public FileStorageRepository()
        {
            var client = new MongoClient(AppConfig.ConnectionString);
            var database = client.GetDatabase(AppConfig.DatabaseName);
            var options = new GridFSBucketOptions
            {
                BucketName = AppConfig.BucketName,
                ChunkSizeBytes = AppConfig.ChunkSizeBytes
            };
            
            this.Bucket = new GridFSBucket(database, options);
        }

        /// <inheritdoc />
        public virtual async Task<ObjectId> UploadAsync(FileStream fileStream)
        {
            var fileName = Path.GetFileName(fileStream.Name);
            return await this.Bucket.UploadFromStreamAsync(filename: fileName, source: fileStream);
        }

        /// <inheritdoc />
        public virtual async Task<MemoryStream> DownloadAsStreamAsync(ObjectId fileId)
        {
            var stream = new MemoryStream();
            await this.Bucket.DownloadToStreamAsync(fileId, stream);
            
            stream.Position = 0;
            return stream;
        }

        /// <inheritdoc />
        public virtual async Task<MemoryStream> DownloadAsStreamAsync(string idOrName)
        {
            if (ObjectId.TryParse(idOrName, out var fileId))
            {
                return await this.DownloadAsStreamAsync(fileId);
            }
            
            var stream = new MemoryStream();
            await this.Bucket.DownloadToStreamByNameAsync(idOrName, stream);
            
            stream.Position = 0;
            return stream;
        }

        /// <inheritdoc />
        public virtual async Task<byte[]> DownloadAsByteArrayAsync(ObjectId fileId)
        {
            return await this.Bucket.DownloadAsBytesAsync(fileId);
        }

        /// <inheritdoc />
        public virtual async Task<byte[]> DownloadAsByteArrayAsync(string idOrName)
        {
            if (ObjectId.TryParse(idOrName, out var fileId))
            {
                return await this.DownloadAsByteArrayAsync(fileId);
            }
            else
            {
                return await this.Bucket.DownloadAsBytesByNameAsync(idOrName);
            }
        }

        /// <inheritdoc />
        public virtual async Task<GridFSFileInfo<ObjectId>> GetFileInfoAsync(ObjectId fileId)
        {
            var stream = new MemoryStream();
            var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(x => x.Id, fileId);
            var cursor = await this.Bucket.FindAsync(filter);
            
            return cursor.FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual async Task<GridFSFileInfo<ObjectId>> GetFileInfoAsync(string idOrName)
        {
            if (ObjectId.TryParse(idOrName, out var fileId))
            {
                return await this.GetFileInfoAsync(fileId);
            }

            var stream = new MemoryStream();
            var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(x => x.Filename, idOrName);
            var cursor = await this.Bucket.FindAsync(filter);
            
            return cursor.FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual async Task DeleteAsync(ObjectId fileId)
        {
            await this.Bucket.DeleteAsync(fileId);
        }

        /// <inheritdoc />
        public virtual async Task DeleteAsync(string fileId)
        {
            if (!ObjectId.TryParse(fileId, out var objectId))
            {
                throw new ArgumentException($"'{fileId}' is not a valid ObjectId");
            }
            
            await this.Bucket.DeleteAsync(objectId);
        }

        /// <inheritdoc />
        [Obsolete]
        public Task<MemoryStream> DownloadAsync(string idOrName)
        {
            return this.DownloadAsStreamAsync(idOrName);
        }

        /// <inheritdoc />
        [Obsolete]
        public Task<GridFSFileInfo<ObjectId>> GetAsync(string idOrName)
        {
            return this.GetFileInfoAsync(idOrName);
        }
    }
}