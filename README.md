# Mongo.FileStorage

Library to store files using [MongoDB](https://www.Mongo.com)

## Required environment variables

- `"MONGODB_CONNECTION_STRING"` (e.g., "mongodb://root:root@localhost:27017")

- `"MONGODB_DATABASE_NAME"` (default: "files_db")

- `"BUCKET_NAME"` (default: "files_bucket")

- `"CHUNK_SIZE_BYTES"` (default: 32768)

## Interface

```C#
namespace Mongo.FileStorage.Repositories
{
    using MongoDB.Bson;
    using MongoDB.Driver.GridFS;

    public interface IFileStorageRepository
    {
        Task<ObjectId> UploadAsync(FileStream fileStream);

        Task<MemoryStream> DownloadAsStreamAsync(string idOrName);;

        Task<byte[]> DownloadAsByteArrayAsync(string idOrName);
        
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

> To upload a file

```C#
var fs = File.OpenRead(filePath);
var fileId = await this.fileStorageRepository.UploadAsync(fs);
```

> To download a file

```C#
MemoryStream file = await this.fileStorageRepository.DownloadAsStreamAsync(idOrName);
```

> To get details from a file

```C#
GridFSFileInfo<ObjectId> file = await this.fileStorageRepository.GetFileInfoAsync(idOrName);
```

> To delete a file

```C#
await this.fileStorageRepository.DeleteAsync(fileId);
````

## Namespace `Mongo.FileStorage.DependencyInjection`

An extension method has been added to facilitate the registration of the `IFileStorage` repository.

```C#
services.RegisterFileStorageRepository(RegisterMode.Transient);
````

## Note

- In order to run the tests, you must first run `docker compose up -d`.

- You can then manage the database at [http://localhost:8081](http://localhost:8081), with:
  - User: user
  - Password: pwd