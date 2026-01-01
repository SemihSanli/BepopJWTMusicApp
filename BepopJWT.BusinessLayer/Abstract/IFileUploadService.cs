using BepopJWT.DTOLayer.FileUploadDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Abstract
{
    public interface IFileUploadService
    {
        Task<string> UploadImageAsync(UploadImageDTO uploadImageDto,string folderName);
        Task<string> UploadMusicAsync(UploadMusicDTO uploadMusicDto);


      
        Task DeleteImageAsync(string imageUrl);
        Task DeleteMusicAsync(string musicUrl);

        Task<string> UpdateImageAsync(IFormFile newFile, string oldImageUrl, string folderName);
        Task<string> UpdateMusicAsync(IFormFile newFile, string oldMusicUrl);
    }
}
