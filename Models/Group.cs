using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GeneralTemplate.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Bright Idea is required.")]
        [MinLength(5, ErrorMessage = "Bright Idea must be at least 5 characters long.")]
        [Display(Name = "Bright Idea: ")]
        public string GroupName { get; set; }
        public int UserId { get; set; }
        public User Planner { get; set; }
        public List<RSVP> GuestsAttending { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}