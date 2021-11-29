using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class Booking
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        [Required]
        [MaxLength(length: 13)]
        [MinLength(length: 13)]
        public string SocialSecurityId { get; set; }
        [Required]
        [MaxLength(length: 5)]
        public string Title { get; set; }
        [Required]
        [MaxLength(length: 50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(length: 50)]
        public string LastName { get; set; }
        [MaxLength(length: 15)]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [MaxLength(length: 10)]
        [MinLength(length: 10)]
        public string PhoneNumber { get; set; }
        [MaxLength(length: 250)]
        public string Address { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateBooked { get; set; }
        [Required]
        public bool Vaccinated { get; set; }
        [Required]
        public string Pin { get; set; }

        public Booking(int bookingId, string socialSecurityId, string title, string firstName, string lastName, string gender, DateTime dateOfBirth, string phoneNumber, string address, DateTime dateCreated, DateTime dateBooked, bool vaccinated, string pin)
        {
            BookingId = bookingId;
            SocialSecurityId = socialSecurityId;
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            Address = address;
            DateCreated = dateCreated;
            DateBooked = dateBooked;
            Vaccinated = vaccinated;
            Pin = pin;
        }

        public Booking()
        {
        }
    }
}
