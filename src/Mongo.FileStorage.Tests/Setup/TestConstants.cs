namespace Mongo.FileStorage.Tests.Setup
{
    public static class TestConstants
    {
        public static string DatabaseName => "files_db_test";
        public static string BucketName => "test_bucket";
        public static int ChunkSizeBytes => 32768;
    }
}