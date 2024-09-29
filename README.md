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

        Task<MemoryStream> DownloadAsync(string idOrName);

        Task<GridFSFileInfo<ObjectId>> GetAsync(string idOrName);

        Task DeleteAsync(ObjectId fileId);
    }
}
```

## Usage

To upload a file

_Example:_

```C#
var fs = File.OpenRead(filePath);
var fileId = await this.fileStorageRepository.UploadAsync(fs);
```

To download a file

_Example:_

```C#
MemoryStream file = await this.fileStorageRepository.DownloadAsync(idOrName);
```

To get a file

_Example:_

```C#
GridFSFileInfo<ObjectId> file = await this.fileStorageRepository.GetAsync(idOrName);
```

To delete a file

_Example:_

```C#
await this.fileStorageRepository.DeleteAsync(fileId);
````

## Namespace `Mongo.FileStorage.DependencyInjection`

An extension method has been added to facilitate the registration of the `IFileStorage` repository.

_Example:_

```C#
services.ResisterFileStorageRepository(RegisterMode.Transient);
````

## Note

- In order to run the tests, you must first run `docker compose up -d`.

- You can then manage the database at [http://localhost:8081](http://localhost:8081), with:
    - User: user
    - Password: pwd