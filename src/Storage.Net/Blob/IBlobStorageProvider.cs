﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Net.Blob
{
   /// <summary>
   /// Slim interface providing access to blob storage.
   /// </summary>
   public interface IBlobStorageProvider : IDisposable
   {
      /// <summary>
      /// Returns the list of available blobs
      /// </summary>
      /// <param name="folderPath">Path to the folder. When null works with a root folder.</param>
      /// <param name="prefix">Blob prefix to filter by. When null returns all blobs.
      /// Cannot be longer than 50 characters.</param>
      /// <param name="recurse">When true returns files recursively</param>
      /// <param name="cancellationToken"></param>
      /// <returns>List of blob IDs</returns>
      Task<IEnumerable<BlobId>> ListAsync(string folderPath,
         string prefix,
         bool recurse,
         CancellationToken cancellationToken = default(CancellationToken));

      /// <summary>
      /// Creates a new blob and returns a writeable stream to it. If the blob already exists it will be
      /// overwritten.
      /// </summary>
      /// <param name="id">Blob ID</param>
      /// <param name="sourceStream">Source stream, must be readable and support Length</param>
      /// <param name="append">When true, appends to the file instead of writing a new one.</param>
      /// <returns>Writeable stream</returns>
      /// <exception cref="ArgumentNullException">Thrown when any parameter is null</exception>
      /// <exception cref="ArgumentException">Thrown when ID is too long. Long IDs are the ones longer than 50 characters.</exception>
      Task WriteAsync(string id, Stream sourceStream, bool append);

      /// <summary>
      /// Opens the blob stream to read.
      /// </summary>
      /// <param name="id">Blob ID, required</param>
      /// <returns>Stream in an open state, or null if blob doesn't exist by this ID. It is your responsibility to close and dispose this
      /// stream after use.</returns>
      /// <exception cref="ArgumentNullException">Thrown when any parameter is null</exception>
      /// <exception cref="ArgumentException">Thrown when ID is too long. Long IDs are the ones longer than 50 characters.</exception>
      Task<Stream> OpenReadAsync(string id);

      /// <summary>
      /// Deletes a blob by id
      /// </summary>
      /// <param name="ids">Blob IDs to delete.</param>
      /// <exception cref="ArgumentNullException">Thrown when ID is null.</exception>
      /// <exception cref="ArgumentException">Thrown when ID is too long. Long IDs are the ones longer than 50 characters.</exception>
      Task DeleteAsync(IEnumerable<string> ids);

      Task<IEnumerable<bool>> ExistsAsync(IEnumerable<string> ids);

      /// <summary>
      /// Gets basic blob metadata
      /// </summary>
      /// <param name="id">Blob id</param>
      /// <returns>Blob metadata or null if blob doesn't exist</returns>
      Task<IEnumerable<BlobMeta>> GetMetaAsync(IEnumerable<string> id);
   }
}
