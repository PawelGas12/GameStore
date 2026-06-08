using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DbModels
{
    public class LibraryEntry
    {
        public int LibraryEntryId { get; set; }

        public int GameId { get; set; }
        public virtual Game? Game { get; set; }

        public string UserId { get; set; } = string.Empty;

        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}
