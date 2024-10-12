namespace Mongo.FileStorage.Tests.Setup
{
    using System;

    public static class EnvVars
    { 
        public static void Configure()
        {
            Environment.SetEnvironmentVariable("MONGODB_CONNECTION_STRING", "mongodb://root:root@localhost:27017");
            Environment.SetEnvironmentVariable("MONGODB_DATABASE_NAME", TestConstants.DatabaseName);
            Environment.SetEnvironmentVariable("BUCKET_NAME", TestConstants.BucketName);
            Environment.SetEnvironmentVariable("CHUNK_SIZE_BYTES", TestConstants.ChunkSizeBytes.ToString());
        }
    }
}