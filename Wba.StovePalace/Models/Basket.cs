using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wba.StovePalace.Models
{
    public class Basket
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [Display(Name = "Gebruiker")]
        public User User { get; set; }

        [ForeignKey("Stove")]
        public int StoveId { get; set; }
        [Display(Name = "Kachel")]
        public Stove Stove { get; set; }

        [Display(Name = "Aantal")]
        [Range(1, 100, ErrorMessage = "Kies een waarde tussen 1 en 100")]
        public int Count { get; set; } = 1;

    }
}
