using GameStore.Models.DbModels;

namespace GameStore.Models.ViewModels
{
    public class StoreIndexViewModel
    {
        public List<Game> Games { get; set; } = new();
        public List<Genre> Genres { get; set; } = new();
        public string? SearchTerm { get; set; }
        public int? SelectedGenreId { get; set; }
        public string? SortOrder { get; set; }
    }
}
