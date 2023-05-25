using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace modelfor_archive.Models
{
    public partial class Attachment
    {
        [Key] // Specifies that the property below is the primary key
        public int AttachmentId { get; set; }

        [ForeignKey(nameof(Messages))] // Specifies the foreign key relationship with the Messages entity
        public int? MessageId { get; set; }

        public string AttDesc { get; set; } // Represents the description of the attachment
        public string AttFile { get; set; } // Represents the file name or path of the attachment (photo)

        public virtual Messages Message { get; set; } // Navigation property representing the associated Messages entity
    }
}