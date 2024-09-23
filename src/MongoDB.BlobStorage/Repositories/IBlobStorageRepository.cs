namespace MongoDB.BlobStorage.Repositories
{
    using MongoDB.Bson;
    using MongoDB.Driver.GridFS;

    public interface IBlobStorageRepository
    {
        Task<ObjectId> UploadAsync(FileStream fileStream);

        Task<MemoryStream> DownloadAsync(ObjectId fileId);

        Task<MemoryStream> DownloadAsync(string fileName);

        Task<GridFSFileInfo<ObjectId>> GetAsync(ObjectId fileId);

        Task<GridFSFileInfo> GetAsync(string fileName);

        Task DeleteAsync(ObjectId fileId);
    }
}