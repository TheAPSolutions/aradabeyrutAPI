using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AradaAPI.Models.DTO.AzureDTO;
using Microsoft.AspNetCore.Http;

public class BlobService
{
    private readonly string _storagePath;

    public BlobService()
    {
        // Define a custom folder for file storage
        _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "images");

        // Ensure the folder exists
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public async Task<List<BlobDTO>> ListAsync()
    {
        List<BlobDTO> files = new();

        // Get all files in the storage path
        var fileInfos = Directory.GetFiles(_storagePath);
        foreach (var filePath in fileInfos)
        {
            var fileName = Path.GetFileName(filePath);
            files.Add(new BlobDTO
            {
                Uri = $"/images/{fileName}", // Relative path
                Name = fileName,
                ContentType = GetMimeType(fileName)
            });
        }

        return files;
    }

    public async Task<BlobResponseDTO> UploadAsync(IFormFile blob)
    {
        BlobResponseDTO response = new();

        try
        {
            // Save the file locally (existing logic)
            string fileName = Guid.NewGuid() + Path.GetExtension(blob.FileName);
            string filePath = Path.Combine(_storagePath, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await blob.CopyToAsync(stream);
            }

            // Generate the full URL with the base URL
            string baseUrl = "https://apsolutionsapi.online";
            string relativeUrl = $"/images/{fileName}";
            response.Blob.Uri = $"{baseUrl}{relativeUrl}";
            response.Blob.Name = fileName;
            response.Status = "File uploaded successfully.";
            response.Error = false;
        }
        catch (Exception ex)
        {
            response.Status = $"File upload failed: {ex.Message}";
            response.Error = true;
        }

        return response;
    }


    public async Task<BlobDTO?> DownloadAsync(string blobFilename)
    {
        string filePath = Path.Combine(_storagePath, blobFilename);

        if (File.Exists(filePath))
        {
            var content = await File.ReadAllBytesAsync(filePath);

            return new BlobDTO
            {
                Content = new MemoryStream(content),
                Name = blobFilename,
                ContentType = GetMimeType(blobFilename)
            };
        }

        return null;
    }

    public async Task<BlobResponseDTO> DeleteAsync(string blobFilename)
    {
        BlobResponseDTO response = new();

        try
        {
            string filePath = Path.Combine(_storagePath, blobFilename);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                response.Status = $"File: {blobFilename} has been successfully deleted.";
                response.Error = false;
            }
            else
            {
                response.Status = $"File: {blobFilename} does not exist.";
                response.Error = true;
            }
        }
        catch (Exception ex)
        {
            response.Status = $"File deletion failed: {ex.Message}";
            response.Error = true;
        }

        return response;
    }

    private string GetMimeType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".pdf" => "application/pdf",
            ".txt" => "text/plain",
            ".csv" => "text/csv",
            _ => "application/octet-stream", // Default for unknown types
        };
    }
}
