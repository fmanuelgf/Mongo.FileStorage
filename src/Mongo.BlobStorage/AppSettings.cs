namespace Mongo.BlobStorage
{
    public static class AppConfig
    {
        public static string ConnectionString => Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")
            ?? string.Empty;

        public static string DatabaseName => Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")
            ?? "blob_storage_db";
        
        public static string BucketName => Environment.GetEnvironmentVariable("BUCKET_NAME")
            ?? "blob_storage_bucket";
        
        public static int ChunkSizeBytes => int.TryParse(Environment.GetEnvironmentVariable("CHUNK_SIZE_BYTES"), out int result)
            ? result
            : 32 * 1024; // 32 MB;
    }
}