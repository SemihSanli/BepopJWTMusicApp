using BepopJWT.DTOLayer.FileUploadDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Abstract
{
    public interface IFileUploadService
    {
        Task<string> UploadImageAsync(UploadImageDTO uploadImageDto);
        Task<string> UploadMusicAsync(UploadMusicDTO uploadMusicDto);
    }
}
