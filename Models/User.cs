using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace loginregister.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]

        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public List<Participant> Participants {get; set;}
        public User()
        {
            Participants = new List<Participant>();
        }

    }

    public class LoginUser : BaseEntity
    {


        [Key]
        public int Userid { get; set; }

        [Required]
        [EmailAddress]
        public string LogEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string LogPassword { get; set; }

        [Required]
        public string ConfirmLogPassword { get; set; }

    }

    public abstract class BaseEntity
    {

    }
}