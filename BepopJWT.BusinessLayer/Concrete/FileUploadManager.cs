using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.BusinessLayer.Options.CloudinaryOptions;
using BepopJWT.DTOLayer.FileUploadDTOs;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
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

        public async Task DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string publicId = GetPublicIdFromUrl(imageUrl);
            if (!string.IsNullOrEmpty(publicId))
            {
                var deletionParams = new DeletionParams(publicId);
                await _cloudinary.DestroyAsync(deletionParams);
            }
        }

        public async Task DeleteMusicAsync(string musicUrl)
        {
            if (string.IsNullOrEmpty(musicUrl)) return;

            string publicId = GetPublicIdFromUrl(musicUrl);

            if (!string.IsNullOrEmpty(publicId))
            {
                var deletionParams = new DeletionParams(publicId)
                {
                    // İsim çakışmasını böyle önlüyoruz:
                    ResourceType = CloudinaryDotNet.Actions.ResourceType.Video
                };

                await _cloudinary.DestroyAsync(deletionParams);
            }
        }

        public async Task<string> UpdateImageAsync(IFormFile newFile, string oldImageUrl, string folderName)
        {
            if (newFile == null || newFile.Length == 0)
            {
                return oldImageUrl;
            }
            await DeleteImageAsync(oldImageUrl);
            return await UploadImageAsync(new UploadImageDTO { imageFile = newFile }, folderName);



        }

        public async Task<string> UpdateMusicAsync(IFormFile newFile, string oldMusicUrl)
        {
            // Yeni dosya yoksa eskisini koru
            if (newFile == null || newFile.Length == 0)
                return oldMusicUrl;

            // Varsa eskiyi sil, yeniyi yükle
            await DeleteMusicAsync(oldMusicUrl);

            return await UploadMusicAsync(new UploadMusicDTO { musicFile = newFile });
        }

        public async Task<string> UploadImageAsync(UploadImageDTO uploadImageDto, string folderName)
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
                    Folder = folderName

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

        private string GetPublicIdFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            try
            {

                int lastDotIndex = url.LastIndexOf('.');
                if (lastDotIndex == -1) return null;
                string urlNoExt = url.Substring(0, lastDotIndex);


                int uploadIndex = urlNoExt.IndexOf("upload/");
                if (uploadIndex == -1) return null;

                string afterUpload = urlNoExt.Substring(uploadIndex + 7);


                if (afterUpload.StartsWith("v") && afterUpload.IndexOf('/') > -1)
                {
                    int slashIndex = afterUpload.IndexOf('/');

                    return afterUpload.Substring(slashIndex + 1);
                }


                return afterUpload;
            }
            catch
            {
                return null;
            }
        }
    }
}
