using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessTrackerApi.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool CurrentlySubscribed { get; set; }

        public User()
        {
            // TODO: currentlySubscribed = ?
            CurrentlySubscribed = true; // REMOVE
        }
    }
}
