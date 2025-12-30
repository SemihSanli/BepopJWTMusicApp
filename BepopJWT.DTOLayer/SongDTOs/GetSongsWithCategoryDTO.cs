namespace BepopJWT.DTOLayer.DTOs.SongDTOs
{
    public class GetSongsWithCategoryDTO
    {
        public int SongId { get; set; }
        public string SongTitle { get; set; }
        public string FileUrl { get; set; }
        public string ImageUrl { get; set; }
        public int MinLevelRequired { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
