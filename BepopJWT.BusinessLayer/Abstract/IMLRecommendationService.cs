using BepopJWT.DTOLayer.SongDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Abstract
{
    public interface IMLRecommendationService
    {
        void TrainModel();  
        Task<List<int>> GetRecommendedSongIds(int userId,int count=5);
    }
}
