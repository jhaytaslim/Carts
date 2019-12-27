using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carts.Data.POCO
{
    public partial class InvoiceTb
    {
        [Key]
        public Guid InvoiceId { get; set; }
        public decimal TotalOrderCost { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("CreatedById")]
        public virtual UsersTb CreatedBy { get; set; }
        [ForeignKey("OrderId")]
        public virtual OrderTb Order { get; set; }
        //[ForeignKey("PaymentId")]
        //public virtual PaymentTb Payment { get; set; }
        [ForeignKey("ProductId")]
        public virtual ProductTb Product { get; set; }
    }
}
