using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.MLModels.SongRatingML
{
    public class SongPrediction
    {
        public float Label { get; set; } 
        public float Score { get; set; } 
    }
}
