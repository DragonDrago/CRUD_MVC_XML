using System;
using System.ComponentModel.DataAnnotations;
namespace CRUD_MVC_XML.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string ItemSupplier { get; set; }
        [Required]
        public string QuantityAvailable { get; set; }
        [Required]
        public decimal PriceCost { get; set; }
        [Required]
        public decimal PriceSale { get; set; }
        [Required]
        public decimal OrederPoint { get; set; }    
        public bool isEdit { get; set; }    

    }
}
