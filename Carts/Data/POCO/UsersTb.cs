using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carts.Data.POCO
{
    public partial class UsersTb
    {
        public UsersTb()
        {
            InvoiceTb = new HashSet<InvoiceTb>();
            OrderItemTb = new HashSet<OrderItemTb>();
            OrdersCreated = new HashSet<OrderTb>();
            OrdersOwned = new HashSet<OrderTb>();
            FullName = $"{FirstName} {LastName}";
        }

        [Key]
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Profileimage { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? DateCreated { get; set; }

        [ForeignKey("RoleId")]
        public virtual RoleTb Role { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual ICollection<InvoiceTb> InvoiceTb { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual ICollection<OrderItemTb> OrderItemTb { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual ICollection<OrderTb> OrdersCreated { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<OrderTb> OrdersOwned { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual ICollection<ProductTb> Products { get; set; }
        [InverseProperty("CreatedFor")]
        public virtual ICollection<MailLogTb> MailLogs { get; set; }
    }
}
