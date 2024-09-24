# Mongo.BlobStorage

Library to store files using [MongoDB](https://www.Mongo.com)

Source code: [https://github.com/fmanuelgf/Mongo.BlobStorage](https://github.com/fmanuelgf/Mongo.BlobStorage)

##  Required environment variables

- `"MONGODB_CONNECTION_STRING"` (e.g., "mongodb://root:root@localhost:27017")

- `"MONGODB_DATABASE_NAME"` (e.g., "my_blob_storage")

- `"BUCKET_NAME"` (e.g., "my_bucket")

- `"CHUNK_SIZE_BYTES"` (e.g., "261120")


## Interface

```C#
namespace Mongo.BlobStorage.Repositories
{
    using MongoDB.Bson;
    using MongoDB.Driver.GridFS;

    public interface IBlobStorageRepository
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

```C#
var fs = File.OpenRead(filePath);
var fileId = await this.blobStorageRepository.UploadAsync(fs);
```

To download a file

```C#
MemoryStream file = await this.blobStorageRepository.DownloadAsync(idOrName);
```

To get a file

```C#
GridFSFileInfo<ObjectId> file = await this.blobStorageRepository.GetAsync(idOrName);
```

To delete a file

```C#
await this.blobStorageRepository.DeleteAsync(fileId);
````

## Note

- In order to run the tests, you must first run `docker compose up -d`.

- You can then manage the database at [http://localhost:8081](http://localhost:8081), with:
    - User: user
    - Password: pwd