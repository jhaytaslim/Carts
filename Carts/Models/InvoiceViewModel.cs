using System;
using System.ComponentModel.DataAnnotations;

namespace Carts.Models
{
    public partial class InvoiceViewModel
    {
        [Key]
        public Guid InvoiceId { get; set; }
        public decimal TotalOrderCost { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual UserViewModel  CreatedBy { get; set; }
        public virtual OrderViewModel Order { get; set; }
        public virtual ProductViewModel Product { get; set; }
    }
}
