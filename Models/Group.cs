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

        [Required(ErrorMessage = "Group Name is required.")]
        [MinLength(2, ErrorMessage = "Group Name must be at least 2 characters long.")]
        [Display(Name = "Group Name: ")]
        public string GroupName { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [Display(Name = "Member One: ")]
        public string GroupMemberOne { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [Display(Name = "Member Two: ")]
        public string GroupMemberTwo { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [Display(Name = "Start Date: ")]
        [DataType(DataType.DateTime)]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [Display(Name = "End Date: ")]
        [DataType(DataType.DateTime)]
        public DateTime? Time { get; set; }

        // Duration = Group.Date - Group.Time <- When can you set this to add Duration to Db before Needing in GroupDetail?
        public int Duration { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [Display(Name = "Description: ")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [Display(Name = "Address: ")]
        public string Address { get; set; }
        public int UserId { get; set; }
        public User Planner { get; set; }
        public int SignedUpUserId { get; set; }
        public SignUp SignUpId { get; set; }
        public List<RSVP> GuestsAttending { get; set; }
        public List<Comment> Comments { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}