using BepopJWT.BusinessLayer.Abstract; // Interface burada
using BepopJWT.BusinessLayer.MLModels.SongRatingML; // SongRating ve SongPrediction burada
using BepopJWT.DataAccessLayer.Context; // AppDbContext burada
using BepopJWT.DTOLayer.SongDTOs;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BepopJWT.BusinessLayer.Concrete
{
    // : IMLRecommendationService eklendi 👇
    public class MLRecommendationManager : IMLRecommendationService
    {
        private static string _modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SongRecommenderModel.zip");
        private readonly AppDbContext _context;
        private MLContext _mlContext;
        public MLRecommendationManager(AppDbContext context)
        {
            _context = context;
            _mlContext = new MLContext();
        }
        public async Task<List<int>> GetRecommendedSongIds(int userId, int count = 5)
        {
            if (!File.Exists(_modelPath))
            {
                TrainModel();
                if (!File.Exists(_modelPath)) return new List<int>();
            }

            ITransformer model = _mlContext.Model.Load(_modelPath, out var schema);
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<SongRating, SongPrediction>(model);

        
            var allSongIds = _context.Songs.Select(s => s.SongId).ToList();
            var likedSongIds = _context.Favorites.Where(x => x.UserId == userId).Select(x => x.SongId).ToList();
            var candidateSongs = allSongIds.Except(likedSongIds).ToList();

            var predictions = new List<(int SongId, float Score)>();

            foreach (var songId in candidateSongs)
            {
                var prediction = predictionEngine.Predict(new SongRating { UserId = (float)userId, SongId = (float)songId });
                predictions.Add((songId, prediction.Score));
            }

            return predictions.OrderByDescending(x => x.Score).Take(count).Select(x => x.SongId).ToList();
        }

        public void TrainModel()
        {
            var favoriteData = _context.Favorites.Select(x => new SongRating
            {
                UserId = (float)x.UserId,
                SongId = (float)x.SongId,
                Label = 1
            }).ToList();

            if (!favoriteData.Any()) return;

            IDataView trainingDataView = _mlContext.Data.LoadFromEnumerable(favoriteData);

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "UserIdEncoded",
                MatrixRowIndexColumnName = "SongIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("UserIdEncoded", "UserId")
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("SongIdEncoded", "SongId"))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(options));

            var model = pipeline.Fit(trainingDataView);
            _mlContext.Model.Save(model, trainingDataView.Schema, _modelPath);
        }
    }
}