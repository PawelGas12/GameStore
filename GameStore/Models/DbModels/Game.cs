using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models.DbModels
{
    public class Game
    {
        public int GameId { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        [StringLength(150)]
        [Display(Name = "Tytuł")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis jest wymagany.")]
        [StringLength(4000)]
        [Display(Name = "Opis")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 1000, ErrorMessage = "Cena musi mieścić się w zakresie 0–1000 zł.")]
        [Display(Name = "Cena (zł)")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data premiery")]
        public DateTime ReleaseDate { get; set; } = DateTime.Today;

        [Display(Name = "Okładka")]
        public string? CoverImagePath { get; set; }

        [Display(Name = "Gatunek")]
        public int GenreId { get; set; }
        public virtual Genre? Genre { get; set; }

        [Display(Name = "Wydawca")]
        public int PublisherId { get; set; }
        public virtual Publisher? Publisher { get; set; }

        public virtual List<Review> Reviews { get; set; } = new List<Review>();
        public virtual List<LibraryEntry> LibraryEntries { get; set; } = new List<LibraryEntry>();

        [NotMapped]
        public double AverageRating => Reviews.Count > 0 ? Math.Round(Reviews.Average(r => r.Rating), 1) : 0;
    }
}
