using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wba.StovePalace.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Stove")]
        public int StoveId { get; set; }
        [Display(Name = "Kachel")]
        public Stove Stove { get; set; }

        [Display(Name = "Aantal")]
        public int Count { get; set; }

        [Display(Name = "Prijs")]
        [DisplayFormat(DataFormatString = "€ {0:#,##0.00}", ApplyFormatInEditMode = false)]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal SalesPrice { get; set; }

    }
}
