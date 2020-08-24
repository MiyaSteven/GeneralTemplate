using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GeneralTemplate.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]
        [RegularExpression("^[0-9A-Za-z ]+$", ErrorMessage = "Name must only contain letters and spaces")]
        [MaxLength(20, ErrorMessage = "Please keep name to 20 characters.")]
        [Display(Name = "Name: ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Alias is required.")]
        [MinLength(2, ErrorMessage = "Alias must be at least 2 characters.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Alias must only contain letters and numbers")]
        [MaxLength(40, ErrorMessage = "Please keep Alias to 40 characters.")]
        [Display(Name = "Alias: ")]
        public string Alias { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email: ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$", ErrorMessage = "Password must contain at least 1 letter and 1 number")]
        [Display(Name = "Password: ")]
        [DataType(DataType.Password)]
        [Compare("Confirm", ErrorMessage = "Passwords do not match.")]
        public string Password { get; set; }

        [NotMapped]
        [Display(Name = "Confirm Password: ")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
        public List<Group> ExistingGroups { get; set; }
        public List<RSVP> RSVPs { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
