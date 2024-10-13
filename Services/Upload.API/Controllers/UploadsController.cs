
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Helper;
using System.Net;

namespace Upload.API.Controllers
{
    public class UploadsController : ControllerBase
    {
        private readonly Cloudinary cloudinary;
        public UploadsController(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }
        [HttpPost("cloud/multiple")]
        public async Task<IActionResult> UploadMultipleFilesAsync([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return Ok(new ApiResponse<bool>(200, false, "Không có file để upload"));
            }
            var data = new List<FileDTO>();
            foreach (var file in files)
            {
                using var stream = file.OpenReadStream();
                string fileType = file.ContentType.ToLower();
                UploadResult uploadResult = null;
                if (fileType.StartsWith("image"))
                {
                    // Upload image
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        PublicId = "your_folder_name/" + file.FileName, // Optional: Save files in a folder
                        Overwrite = true
                    };
                    uploadResult = await cloudinary.UploadAsync(uploadParams);
                }
                else if (fileType.StartsWith("video"))
                {
                    // Upload video
                    var uploadParams = new VideoUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        PublicId = "your_folder_name/" + file.FileName, // Optional: Save files in a folder
                        Overwrite = true
                    };
                    uploadResult = await cloudinary.UploadAsync(uploadParams);
                    if (uploadResult != null && uploadResult.StatusCode == HttpStatusCode.OK)
                    {
                        data.Add(new FileDTO(
                            uploadResult.SecureUrl.AbsoluteUri,
                            file.FileName,
                            file.ContentType
                        ));
                    }
                }
            }
            return Ok(new ApiResponse<List<FileDTO>>(200, data,"Kết quả upload"));
        }
        [HttpDelete("cloud/multiple")]
        public async Task<IActionResult> DeleteMultipleFilesAsync(string[] fileNames)
        {
            throw new NotImplementedException();
        }
        [HttpGet("cloud/files")]
        public async Task<IActionResult> GetFilesInAwsAsync()
        {
            throw new NotImplementedException();
        }
 
    }
}
