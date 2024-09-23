# MongoDB.BlobStorage

Library to store files using [MongoDB](https://www.mongodb.com)

[MIT License](LICENSE)

<hr>

##  Required environment variables

- `"MONGODB_CONNECTION_STRING"` (e.g., "mongodb://root:root@localhost:27017")

- `"MONGODB_DATABASE_NAME"` (e.g., "my_blob_storage")

- `"BUCKET_NAME"` (e.g., "my_bucket")

- `"CHUNK_SIZE_BYTES"` (e.g., "261120")

<hr>

## Interface

```
namespace MongoDB.BlobStorage.Repositories
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

<hr>

## Usage

### 1) To upload a file

```
var fs = File.OpenRead(filePath);
var fileId = await this.blobStorageRepository.UploadAsync(fs);
```

### 2) To download a file

```
MemoryStream file = await this.blobStorageRepository.DownloadAsync(idOrName);
```

### 3) To get a file

```
GridFSFileInfo file = await this.blobStorageRepository.GetAsync(idOrName);
```

### 4) To delete a file

```
await this.blobStorageRepository.DeleteAsync(fileId);
````

<hr>

## Note

- In order to run the tests, you must first run `docker compose up -d`.

- You can then manage the database at [http://localhost:8081](http://localhost:8081), with:
    - User: user
    - Password: pwd