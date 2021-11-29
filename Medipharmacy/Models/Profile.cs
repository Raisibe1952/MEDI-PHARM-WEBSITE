using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class Profile
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileId { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        [MaxLength(length: 50)]
        public string FirstName { get; set; }
        [MaxLength(length: 50)]
        public string MiddleName { get; set; }
        [Required]
        [MaxLength(length: 50)]
        public string LastName { get; set; }
        [MaxLength(length: 10)]
        [MinLength(length: 10)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(length: 250)]
        public string Address { get; set; }

        public Profile(int profileId, int userId, string firstName, string middleName, string lastName, string phoneNumber, string address)
        {
            ProfileId = profileId;
            UserId = userId;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Address = address;
        }

        public Profile()
        {
        }
    }
}
