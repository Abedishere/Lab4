using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InmindLab3_4part2.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadProfilePictureAsync(IFormFile file, string userId);
        Task DeleteProfilePictureAsync(string blobName);
        Task<Stream> DownloadProfilePictureAsync(string blobName);
    }

    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "profilepictures";

        public BlobStorageService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
            CreateContainerIfNotExistsAsync().GetAwaiter().GetResult();
        }

        private async Task CreateContainerIfNotExistsAsync()
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        }

        public async Task<string> UploadProfilePictureAsync(IFormFile file, string userId)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            
            // Create a unique blob name using the user ID and a timestamp
            string blobName = $"{userId}_{DateTime.UtcNow.Ticks}{Path.GetExtension(file.FileName)}";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Upload the file
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, true);
            
            // Return the URL of the blob
            return blobClient.Uri.ToString();
        }

        public async Task DeleteProfilePictureAsync(string blobName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<Stream> DownloadProfilePictureAsync(string blobName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }
    }
}