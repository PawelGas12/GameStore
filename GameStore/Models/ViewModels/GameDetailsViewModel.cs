using GameStore.Models.DbModels;

namespace GameStore.Models.ViewModels
{
    public class GameDetailsViewModel
    {
        public Game Game { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public Review NewReview { get; set; } = new();
        public bool IsInLibrary { get; set; }
        public bool HasReviewed { get; set; }
    }
}
