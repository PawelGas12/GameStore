using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DbModels
{
    public class Review
    {
        public int ReviewId { get; set; }

        public int GameId { get; set; }
        public virtual Game? Game { get; set; }

        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Ocena musi być w zakresie 1–5.")]
        [Display(Name = "Ocena (1–5)")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Treść recenzji jest wymagana.")]
        [StringLength(1000)]
        [Display(Name = "Recenzja")]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
