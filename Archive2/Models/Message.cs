using modeforview2.Models; // Importing the namespace for the associated model
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace modelfor_archive.Models
{
    public partial class Messages
    {
        public Messages()
        {
            Attachments = new HashSet<Attachment>();
            ListUserMes = new HashSet<ListUserMes>();
        }

        [Key]
        public int MessagesId { get; set; } // Represents the ID of the message
        [ForeignKey(nameof(UserAcc))]
        public int? UserId { get; set; } // Represents the ID of the user associated with the message
        public bool? Rcv { get; set; } // Represents the status of message reception

        public string Title { get; set; } // Represents the title of the message

        public string MessageText { get; set; } // Represents the content of the message
        public DateTime? RcvTime { get; set; } // Represents the time of message reception
        public bool? Seen { get; set; } // Represents the status of message being seen
        public DateTime? SendDate { get; set; } // Represents the date of message sending

        public virtual UserAcc User { get; set; } // Represents the navigation property for the associated user
        public virtual ICollection<Attachment> Attachments { get; set; } // Represents the collection of attachments associated with the message

        public string MessageAddress { get; set; } // Represents the message address

        public string MessageDestination { get; set; } // Represents the message destination
        public virtual ICollection<ListUserMes> ListUserMes { get; set; } // Represents the collection of users associated with the message
    }

    public class MessageSend
    {
        [Key]
        public int MessageId { get; set; } // Represents the ID of the message
        [ForeignKey(nameof(UserAcc))]
        public int UserId { get; set; } // Represents the ID of the user associated with the message

        public DateTime RcvTime { get; set; } // Represents the time of message reception

        public string Attachments { get; set; } // Represents the attachments of the message

        public bool Seen { get; set; } // Represents the status of message being seen

        public string Title { get; set; } // Represents the title of the message

        public string MessageText { get; set; } // Represents the content of the message

        public DateTime SendDate { get; set; } // Represents the date of message sending

        public string MessageAddress { get; set; } // Represents the message address

        public string MessageDestination { get; set; } // Represents the message destination
    }
}