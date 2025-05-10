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
        GridFSBucket Bucket { get; }

        Task<ObjectId> UploadAsync(FileStream fileStream);

        Task<MemoryStream> DownloadAsStreamAsync(ObjectId fileId);
        
        Task<MemoryStream> DownloadAsStreamAsync(string idOrName);

        Task<byte[]> DownloadAsByteArrayAsync(ObjectId fileId);
        
        Task<byte[]> DownloadAsByteArrayAsync(string idOrName);
        
        Task<GridFSFileInfo<ObjectId>> GetFileInfoAsync(ObjectId fileId);
        
        Task<GridFSFileInfo<ObjectId>> GetFileInfoAsync(string idOrName);
        
        Task DeleteAsync(ObjectId fileId);
        
        Task DeleteAsync(string fileId);

        Task DeleteAsync(ObjectId[] fileIds);
        
        Task DeleteAsync(IList<ObjectId> fileIds);

        Task DeleteAsync(string[] fileIds);
        
        Task DeleteAsync(IList<string> fileIds);
    }
}
```

## Usage

First, register the `IFileStorageRepository` in the services collection.

```csharp
using Mongo.FileStorage.DependencyInjection;

...

// Using the default configuration (see `Required environment variables`)
services.RegisterFileStorageRepository(RegisterMode.Transient);

// Using the default configuration plus custom read/write options
this.services.RegisterFileStorageRepository(
    RegisterMode.Transient,
    ReadConcern.Default,
    ReadPreference.Primary,
    WriteConcern.WMajority);
````

To upload a file

```csharp
var fs = File.OpenRead(filePath);
var fileId = await this.fileStorageRepository.UploadAsync(fs);
```

To download a file

```csharp
MemoryStream file = await this.fileStorageRepository.DownloadAsStreamAsync(idOrName);
```

To get details from a file

```csharp
GridFSFileInfo<ObjectId> file = await this.fileStorageRepository.GetFileInfoAsync(idOrName);
```

To delete a file

```csharp
await this.fileStorageRepository.DeleteAsync(fileId);
````

## Note

- In order to run the tests, you must first run `docker compose up -d`.
- You can then manage the database at [http://localhost:8081](http://localhost:8081), with:
  - User: user
  - Password: pwd
