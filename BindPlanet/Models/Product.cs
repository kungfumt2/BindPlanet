using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BindPlanet.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
      
        public int SupplierId { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Supplier Supplier { get; set; }
        public Category Category { get; set; }
    }
}
