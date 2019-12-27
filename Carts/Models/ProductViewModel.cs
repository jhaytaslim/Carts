using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Carts.Models
{
    public partial class ProductViewModel
    {
        public ProductViewModel()
        {
            InvoiceViewModel = new HashSet<InvoiceViewModel>();
            OrderItemViewModel = new HashSet<OrderItemViewModel>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FeatureImage { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<InvoiceViewModel> InvoiceViewModel { get; set; }
        public virtual ICollection<OrderItemViewModel> OrderItemViewModel { get; set; }
    }
}
