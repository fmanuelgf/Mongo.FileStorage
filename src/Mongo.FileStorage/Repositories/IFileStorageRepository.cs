namespace Mongo.FileStorage.Repositories
{
    using MongoDB.Bson;
    using MongoDB.Driver.GridFS;

    public interface IFileStorageRepository
    {
        /// <summary>
        /// The <see cref="GridFSBucket"/>.
        /// </summary>
        GridFSBucket Bucket { get; }

        /// <summary>
        /// Store a file in the database.
        /// </summary>
        /// <param name="fileStream">The <see cref="FileStream"/> to store.</param>
        /// <returns>The ID of the stored file.</returns>
        Task<ObjectId> UploadAsync(FileStream fileStream);

        /// <summary>
        /// Download a stored file as a <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="fileId">The ID of the stored file to be downloaded.</param>
        /// <returns>The <see cref="MemoryStream"/>.</returns>
        Task<MemoryStream> DownloadAsStreamAsync(ObjectId fileId);

        /// <summary>
        /// Download a stored file as a <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="idOrName">The ID or the Name of the stored file to be downloaded.</param>
        /// <returns>The <see cref="MemoryStream"/>.</returns>
        Task<MemoryStream> DownloadAsStreamAsync(string idOrName);

        /// <summary>
        /// Download a stored file as a byte array.
        /// </summary>
        /// <param name="fileId">The ID of the stored file to be downloaded.</param>
        /// <returns>The <see cref="byte"/> array.</returns>
        Task<byte[]> DownloadAsByteArrayAsync(ObjectId fileId);

        /// <summary>
        /// Download a stored file as a byte array.
        /// </summary>
        /// <param name="idOrName">The ID or the Name of the stored file to be downloaded.</param>
        /// <returns>The <see cref="byte"/> array.</returns>
        Task<byte[]> DownloadAsByteArrayAsync(string idOrName);

        /// <summary>
        /// Get the <see cref="GridFSFileInfo"/> of the given file.
        /// </summary>
        /// <param name="fileId">The ID of the stored file.</param>
        /// <returns>The <see cref="GridFSFileInfo"/>.</returns>
        Task<GridFSFileInfo<ObjectId>> GetFileInfoAsync(ObjectId fileId);

        /// <summary>
        /// Get the <see cref="GridFSFileInfo"/> of the given file.
        /// </summary>
        /// <param name="idOrName">The ID or the Name of the stored file.</param>
        /// <returns>The <see cref="GridFSFileInfo"/>.</returns>
        Task<GridFSFileInfo<ObjectId>> GetFileInfoAsync(string idOrName);

        /// <summary>
        /// Delete a stored file
        /// </summary>
        /// <param name="fileId">The ID of the stored file to be deleted.</param>
        Task DeleteAsync(ObjectId fileId);
        
        /// <summary>
        /// Delete a stored file
        /// </summary>
        /// <param name="fileId">The ID or the Name of the stored file to be deleted.</param>
        Task DeleteAsync(string fileId);

        /// <summary>
        /// Delete a collection of stored files
        /// </summary>
        /// <param name="fileIds">The array of IDs of the stored files to be deleted.</param>
        Task DeleteAsync(ObjectId[] fileIds);

        /// <summary>
        /// Delete a collection of stored files
        /// </summary>
        /// <param name="fileIds">The array of IDs of the stored files to be deleted.</param>
        Task DeleteAsync(IList<ObjectId> fileIds);
        
        /// <summary>
        /// Delete a collection of stored files
        /// </summary>
        /// <param name="fileIds">The array of ID or Names of the stored files to be deleted.</param>
        Task DeleteAsync(string[] fileIds);

        /// <summary>
        /// Delete a collection of stored files
        /// </summary>
        /// <param name="fileIds">The array of ID or Names of the stored files to be deleted.</param>
        Task DeleteAsync(IList<string> fileIds);
    }
}