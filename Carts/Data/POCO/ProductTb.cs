using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carts.Data.POCO
{
    public partial class ProductTb
    {
        public ProductTb()
        {
            InvoiceTb = new HashSet<InvoiceTb>();
            OrderItemTb = new HashSet<OrderItemTb>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FeatureImage { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public DateTime DateCreated { get; set; }


        [ForeignKey("CreatedById")]
        public virtual UsersTb CreatedBy { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<InvoiceTb> InvoiceTb { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<OrderItemTb> OrderItemTb { get; set; }
    }
}
