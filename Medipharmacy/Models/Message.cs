using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class Message
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        [Required]
        [MaxLength(length: 50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(length: 50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(length: 250)]
        public string Email { get; set; }
        [Required]
        [MaxLength(length: 150)]
        public string Subject { get; set; }
        [Required]
        [MaxLength(length: 1000)]
        public string Text { get; set; }

        public Message(int messageId, string firstName, string lastName, string email, string subject, string text)
        {
            MessageId = messageId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Subject = subject;
            Text = text;
        }

        public Message()
        {
        }
    }
}
