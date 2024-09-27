namespace Mongo.FileStorage.Repositories
{
    using MongoDB.Bson;
    using MongoDB.Driver.GridFS;

    public interface IFileStorageRepository
    {
        Task<ObjectId> UploadAsync(FileStream fileStream);

        Task<MemoryStream> DownloadAsync(string idOrName);

        Task<GridFSFileInfo<ObjectId>> GetAsync(string idOrName);

        Task DeleteAsync(ObjectId fileId);
    }
}