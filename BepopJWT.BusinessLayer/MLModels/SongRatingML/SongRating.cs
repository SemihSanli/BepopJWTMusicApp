using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.MLModels.SongRatingML
{
    public class SongRating
    {
        [LoadColumn(0)]
        public float UserId { get; set; } // ML.NET float sever

        [LoadColumn(1)]
        public float SongId { get; set; }

        [LoadColumn(2)]
        public float Label { get; set; } // 1 = Beğendi
    }
}
