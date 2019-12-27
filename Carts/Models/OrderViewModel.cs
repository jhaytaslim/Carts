using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Carts.Models
{
    public partial class OrderViewModel
    {
        public OrderViewModel()
        {
            InvoiceViewModel = new HashSet<InvoiceViewModel>();
            OrderItemViewModel = new HashSet<OrderItemViewModel>();
        }

        [Key]
        public Guid OrderId { get; set; }
        public string OrderReference { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal? DeliveryCost { get; set; }
        public string DateCreated { get; set; }

        
        public virtual UserViewModel CreatedBy { get; set; }
        public virtual UserViewModel Customer { get; set; }
        public virtual ICollection<InvoiceViewModel> InvoiceViewModel { get; set; }
        public virtual ICollection<OrderItemViewModel> OrderItemViewModel { get; set; }
    }
}
