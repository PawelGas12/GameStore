using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DbModels
{
    public class Genre
    {
        public int GenreId { get; set; }

        [Required(ErrorMessage = "Nazwa gatunku jest wymagana.")]
        [StringLength(60)]
        [Display(Name = "Nazwa gatunku")]
        public string Name { get; set; } = string.Empty;

        public virtual List<Game> Games { get; set; } = new List<Game>();
    }
}
