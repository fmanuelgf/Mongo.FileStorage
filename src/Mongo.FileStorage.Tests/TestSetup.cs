namespace Mongo.FileStorage.Tests
{
    using Mongo.FileStorage.Tests.Setup;

    internal static class TestSetup
    {
        internal static Dependencies Dependencies = new();

        internal static void Configure()
        {
            EnvVars.Configure();
        }
    }
}