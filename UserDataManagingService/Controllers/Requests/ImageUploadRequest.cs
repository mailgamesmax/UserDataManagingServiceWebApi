﻿using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests
{
    public class ImageUploadRequest
    {
        [MaxFileSize(4 * 1024 * 1024)]
        [FileExtension(".jpg", ".jpeg", ".png", ".gif", ".tiff", ".bmp", ".raw", ".svg", ".webp", ".heif", ".psd")]
        //[FileExtension(".jpeg", ".png", ".gif", ".tiff", ".bmp", ".webp")]
        public IFormFile Image { get; set; }
    }
}
