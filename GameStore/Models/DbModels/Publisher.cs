using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DbModels
{
    public class Publisher
    {
        public int PublisherId { get; set; }

        [Required(ErrorMessage = "Nazwa wydawcy jest wymagana.")]
        [StringLength(100)]
        [Display(Name = "Wydawca / studio")]
        public string Name { get; set; } = string.Empty;

        public virtual List<Game> Games { get; set; } = new List<Game>();
    }
}
