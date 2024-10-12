# Mongo.FileStorage

Library to store files using [MongoDB](https://www.Mongo.com)

## Required environment variables

- `"MONGODB_CONNECTION_STRING"` (e.g., "mongodb://root:root@localhost:27017")

- `"MONGODB_DATABASE_NAME"` (default: "files_db")

- `"BUCKET_NAME"` (default: "files_bucket")

- `"CHUNK_SIZE_BYTES"` (default: 32768)

## Interface

```csharp
namespace Mongo.FileStorage.Repositories
{
    using MongoDB.Bson;
    using MongoDB.Driver.GridFS;

    public interface IFileStorageRepository
    {
        Task<ObjectId> UploadAsync(FileStream fileStream);

        Task<MemoryStream> DownloadAsStreamAsync(ObjectId fileId);
        
        Task<MemoryStream> DownloadAsStreamAsync(string idOrName);

        Task<byte[]> DownloadAsByteArrayAsync(ObjectId fileId);
        
        Task<byte[]> DownloadAsByteArrayAsync(string idOrName);
        
        Task<GridFSFileInfo<ObjectId>> GetFileInfoAsync(ObjectId fileId);
        
        Task<GridFSFileInfo<ObjectId>> GetFileInfoAsync(string idOrName);
        
        Task DeleteAsync(ObjectId fileId);
        
        Task DeleteAsync(string fileId);

        [Obsolete("Use DownloadAsStreamAsync instead as this method will be removed.")]
        Task<MemoryStream> DownloadAsync(string idOrName);

        [Obsolete("Use GetFileInfoAsync instead as this method will be removed")]
        Task<GridFSFileInfo<ObjectId>> GetAsync(string idOrName);
    }
}
```

## Usage

> First, register the `IFileStorageRepository` in the services collection.

```csharp
using Mongo.FileStorage.DependencyInjection;

...

services.RegisterFileStorageRepository(RegisterMode.Transient);
````

> To upload a file

```csharp
var fs = File.OpenRead(filePath);
var fileId = await this.fileStorageRepository.UploadAsync(fs);
```

> To download a file

```csharp
MemoryStream file = await this.fileStorageRepository.DownloadAsStreamAsync(idOrName);
```

> To get details from a file

```csharp
GridFSFileInfo<ObjectId> file = await this.fileStorageRepository.GetFileInfoAsync(idOrName);
```

> To delete a file

```csharp
await this.fileStorageRepository.DeleteAsync(fileId);
````

## FileStorage class

> **Note:** Auxiliary static class providing access to the GridFSBucket, if needed.

```csharp
var bucket = FileStorage.Bucket;
...
````

## Note

- In order to run the tests, you must first run `docker compose up -d`.

- You can then manage the database at [http://localhost:8081](http://localhost:8081), with:
  - User: user
  - Password: pwd