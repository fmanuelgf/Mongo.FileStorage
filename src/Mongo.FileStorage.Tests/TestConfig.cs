namespace Mongo.FileStorage.Tests
{
    using System;

    public static class TestConfig
    { 
        public static void Configure()
        {
            Environment.SetEnvironmentVariable("MONGODB_CONNECTION_STRING", "mongodb://root:root@localhost:27017");
            Environment.SetEnvironmentVariable("MONGODB_DATABASE_NAME", "files_test");
            Environment.SetEnvironmentVariable("BUCKET_NAME", "test_bucket");
            Environment.SetEnvironmentVariable("CHUNK_SIZE_BYTES", (255 * 1024).ToString());
        }
    }
}