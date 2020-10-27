using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTrackerApi.Models
{
    public class Entry
    {
        [Key]
        public int EntryId { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        [Required]
        public string Activity { get; set; }
        [Required]
        public string Amount { get; set; }
        public DateTime Timestamp { get; set; }

        public Entry()
        {
            Timestamp = DateTime.Now;
        }
    }
}
