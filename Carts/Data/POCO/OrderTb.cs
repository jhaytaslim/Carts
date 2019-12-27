using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carts.Data.POCO
{
    public partial class OrderTb
    {
        public OrderTb()
        {
            InvoiceTb = new InvoiceTb();
            OrderItemTb = new HashSet<OrderItemTb>();
        }

        [Key]
        public Guid OrderId { get; set; }
        public string OrderReference { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal? DeliveryCost { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateDeleted { get; set; }

        [ForeignKey("CreatedById")]
        public virtual UsersTb CreatedBy { get; set; }
        [ForeignKey("CustomerId")]
        public virtual UsersTb Customer { get; set; }

        [InverseProperty("Order")]
        public virtual InvoiceTb InvoiceTb { get; set; }
        [InverseProperty("Order")]
        public virtual ICollection<OrderItemTb> OrderItemTb { get; set; }
    }
}
