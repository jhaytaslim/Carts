using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carts.Data.POCO
{
    public partial class OrderItemTb
    {
        [Key]
        public Guid OrderItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? PriceSold { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("CreatedById")]
        public virtual UsersTb CreatedBy { get; set; }
        [ForeignKey("OrderId")]
        public virtual OrderTb Order { get; set; }
        [ForeignKey("ProductId")]
        public virtual ProductTb Product { get; set; }
    }
}
