using modelfor_archive.Models; // Importing the namespace for the associated model
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace modeforview2.Models
{
    public partial class ListUserMes
    {
        [Key]
        public int UserId { get; set; } // Represents the ID of the user associated with the message
        public int MessageId { get; set; } // Represents the ID of the message

        public virtual Messages Message { get; set; } // Represents the navigation property for the associated message
        public virtual UserAcc User { get; set; } // Represents the navigation property for the associated user
    }

    public class ListOperate
    {
        public int UserId { get; set; } // Represents the ID of the user
        public int MessageId { get; set; } // Represents the ID of the message
    }
}
