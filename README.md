# MongoDB.BlobStorage

Library to store files using [MongoDB](https://www.mongodb.com)

[MIT License](LICENSE)

## Required environment variables

- `"MONGODB_CONNECTION_STRING"` (e.g., "mongodb://root:root@localhost:27017")

- `"MONGODB_DATABASE_NAME"` (e.g., "my_blob_storage")

- `"BUCKET_NAME"` (e.g., "my_bucket")

- `"CHUNK_SIZE_BYTES"` (e.g., "261120")

## Interface:

```
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
```

## Usage:

### 1) To upload a file

```
var fs = File.OpenRead(filePath);
var fileId = await this.blobStorageRepository.UploadAsync(fs);
```

### 2) To download a file

<span> By ID: </span>
```
MemoryStream file = await this.blobStorageRepository.DownloadAsync(fileId);
```

<span> By Name: </span>
```
MemoryStream file = await this.blobStorageRepository.DownloadAsync(fileName);
```

### 3) To get a file

<span> By ID: </span>
```
GridFSFileInfo<ObjectId> file = await this.blobStorageRepository.GetAsync(fileId);
```

<span> By Name: </span>
```
GridFSFileInfo file = await this.blobStorageRepository.GetAsync(fileName);
```

### 4) To delete a file

```
await this.blobStorageRepository.DeleteAsync(fileId);
````

## <b>Note</b>

- In order to run the tests, you must first run `docker compose up -d`.

- You can then manage the database at [http://localhost:8081](http://localhost:8081), with:
    - User: user
    - Password: pwd