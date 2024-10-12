namespace Mongo.FileStorage
{
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;

    public static class FileStorage
    {
        public static GridFSBucket Bucket => GetBucket();      
        
        private static GridFSBucket GetBucket()
        {
            var client = new MongoClient(AppConfig.ConnectionString);
            var database = client.GetDatabase(AppConfig.DatabaseName);
            var options = new GridFSBucketOptions
            {
                BucketName = AppConfig.BucketName,
                ChunkSizeBytes = AppConfig.ChunkSizeBytes
            };
            
            return new GridFSBucket(database, options);
        }
    }
}