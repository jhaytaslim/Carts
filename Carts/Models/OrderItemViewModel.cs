using System;
using System.ComponentModel.DataAnnotations;

namespace Carts.Models
{
    public partial class OrderItemViewModel
    {
        [Key]
        public Guid OrderItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? PriceSold { get; set; }
        public DateTime DateCreated { get; set; }
        public string OrderReference { get; set; }

        public virtual UserViewModel CreatedBy { get; set; }
        public virtual OrderViewModel Order { get; set; }
        public virtual ProductViewModel Product { get; set; }
    }
}
