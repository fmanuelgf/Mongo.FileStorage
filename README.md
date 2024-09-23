# MongoDB.BlobStorage

Library to store files using MongoDB

[MIT License](LICENSE)

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
GridFSFileInfo<ObjectId> file = await this.blobStorageRepository.GetAsync(fileName);
```

### 4) To delete a file

```
await this.blobStorageRepository.DeleteAsync(fileId);
````
