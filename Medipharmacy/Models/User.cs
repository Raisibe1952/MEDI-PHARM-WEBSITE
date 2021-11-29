using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class User
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public bool Super { get; set; } 
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public User(int userId, bool super, string firstname, string lastname, string address, string phoneNumber, string email, string password)
        {
            UserId = userId;
            Super = super;
            Firstname = firstname;
            Lastname = lastname;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            Password = password;
        }

        public User()
        {
        }
    }
}
