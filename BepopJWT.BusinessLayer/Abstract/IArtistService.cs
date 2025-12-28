using BepopJWT.DTOLayer.ArtistDTOs;
using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Abstract
{
    public interface IArtistService:IGenericService<Artist>
    {
        Task CreateArtistWithImageFileAsync(CreateArtistDTO createArtistDto);
        Task UpdateArtistWithImageFileAsync(UpdateArtistDTO updateArtistDto);
        Task DeleteArtistWithImageFileAsync(int id);
    }
}
