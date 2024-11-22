# Mongo.FileStorage

## Release 8.2.1

- Update nuget packages

## Release 8.2.0

- Remove obsolete code

## Release 8.1.7

- Add the property `Bucket` to `IFileStorageRepository`.
- Make the repository methods `virtual` so that they can be overridden in derived classes.

## Release 8.1.6

- Add method `DownloadAsStreamAsync(ObjectId fileId)`;
- Add method `DownloadAsByteArrayAsync(ObjectId fileId)`;
- Add method `GetFileInfoAsync(ObjectId fileId)`;

## Release 8.1.5

- Add method `DownloadAsStreamAsync`.
- Add method `DownloadAsByteArrayAsync`.
- Add method `GetFileInfoAsync`.
- Mark method `DownloadAsync` as Obsolete
- Mark method `GetAsync` as Obsolete

## Release 8.1.4

- Minor fixes

## Release 8.1.3

- Add method `DeleteAsync(string fileId)`

## Release 8.1.2

- Add namespace Mongo.FileStorage.DependencyInjection
