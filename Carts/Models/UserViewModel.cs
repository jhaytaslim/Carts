using System;
using System.ComponentModel.DataAnnotations;

namespace Carts.Models
{
    public class LoginViewModel
    {
        [Required]
        public string username { get; set; }

        [Required, DataType(DataType.Password)]
        public string password { get; set; }
    }

    public class UserViewModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        //public int RoleId { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Enabled { get; set; }

        public string Fullname { get; set; }

    }

}
