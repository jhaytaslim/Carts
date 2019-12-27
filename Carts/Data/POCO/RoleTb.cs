using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carts.Data.POCO
{
    public partial class RoleTb
    {
        public RoleTb()
        {
            UsersTb = new HashSet<UsersTb>();
        }

        [Key]
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<UsersTb> UsersTb { get; set; }
    }
}
