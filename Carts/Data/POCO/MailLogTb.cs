using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carts.Data.POCO
{
    public partial class MailLogTb
    {
        public MailLogTb()
        {
            CreatedFor = new UsersTb();
        }

        [Key]
        public Guid MailId { get; set; }
        public string MailReference { get; set; }
        public string OtherReference { get; set; }
        public DateTime? DateCreated { get; set; }

        [ForeignKey("CreatedForId")]
        public virtual UsersTb CreatedFor { get; set; }
        
    }
}
