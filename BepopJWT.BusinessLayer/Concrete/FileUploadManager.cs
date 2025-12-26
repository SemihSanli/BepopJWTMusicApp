using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.BusinessLayer.Options.CloudinaryOptions;
using BepopJWT.DTOLayer.FileUploadDTOs;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class FileUploadManager : IFileUploadService
    {
        private readonly Cloudinary _cloudinary;

        public FileUploadManager(IOptions<CloudinarySettings> cloudinaryConfig)
        {
            var account = new Account(
                cloudinaryConfig.Value.CloudName,
                cloudinaryConfig.Value.ApiKey,
                cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(UploadImageDTO uploadImageDto)
        {
            var file = uploadImageDto.imageFile;

            // Dosya var mı kontrolü
            if (file == null || file.Length == 0)
                throw new Exception("Lütfen bir resim dosyası seçiniz.");

            //Uzantı kontrollerimiz
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception($"Geçersiz resim formatı! Sadece şunlar kabul edilir: {string.Join(", ", allowedExtensions)}");
            }

            // Cloudinary'ye gönderir
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "bepop_images"
                    
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    throw new Exception(uploadResult.Error.Message);

                return uploadResult.SecureUrl.ToString();
            }
        }

        public async Task<string> UploadMusicAsync(UploadMusicDTO uploadMusicDto)
        {
            var file = uploadMusicDto.musicFile;

            //  Dosya var mı kontrolü?
            if (file == null || file.Length == 0)
                throw new Exception("Lütfen bir müzik dosyası seçiniz.");

            // uzantı kontrolü
            var allowedExtensions = new[] { ".mp3", ".wav", ".m4a" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception($"Geçersiz müzik formatı! Sadece şunlar kabul edilir: {string.Join(", ", allowedExtensions)}");
            }

            // KURAL 3: Cloudinary'ye Gönder (VideoUploadParams ile)
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "bepop_songs"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    throw new Exception(uploadResult.Error.Message);

                return uploadResult.SecureUrl.ToString();
            }
        }
    }
}
