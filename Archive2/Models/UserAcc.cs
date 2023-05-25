using modeforview2.Models; // Importing the namespace for the associated model
using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace modelfor_archive.Models
{
    public partial class UserAcc
    {
        public UserAcc()
        {
            Messages = new HashSet<Messages>();
            ListUserMes = new HashSet<ListUserMes>();
        }

        [Key]
        public int UserAccID { get; set; } // Represents the ID of the user account
        public string UserName { get; set; } // Represents the username
        [Unique] // Unique constraint on AcadMail property
        public string AcadMail { get; set; } // Represents the academic email address
        public string Pass { get; set; } // Represents the password
        public string JobTitle { get; set; } // Represents the job title
        public string Phone { get; set; } // Represents the phone number
        public bool IsAdmin { get; set; } // Represents whether the user is an admin or not
        [NotNull]
        [System.ComponentModel.DataAnnotations.Required]
        public bool Active { get; set; } // Represents whether the user account is active or not
        public string Department { get; set; } // Represents the department of the user
        public Gender userGender { get; set; } // Represents the gender of the user

        public virtual ICollection<Messages> Messages { get; set; } // Represents the collection of messages associated with the user
        public virtual ICollection<ListUserMes> ListUserMes { get; set; } // Represents the collection of user-message associations
    }

    public class Login
    {
        public string Email { get; set; } // Represents the email for login
        public string Password { get; set; } // Represents the password for login
    }

    public class User
    {
        public int UserAccID { get; set; } // Represents the ID of the user account
        public string UserName { get; set; } // Represents the username
        public string AcadMail { get; set; } // Represents the academic email address
        public string Pass { get; set; } // Represents the password
        public string JobTitle { get; set; } // Represents the job title
        public string Phone { get; set; } // Represents the phone number
        public bool IsAdmin { get; set; } // Represents whether the user is an admin or not
        public Gender userGender { get; set; } // Represents the gender of the user
    }

    public class UserViewModel
    {
        public string UserName { get; set; } // Represents the username
        public string Phone { get; set; } // Represents the phone number
    }

    public enum Gender
    {
        Male,
        Female
    }
}