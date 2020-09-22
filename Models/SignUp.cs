using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GeneralTemplate.Models
{
    public class SignUp
    {
        [Key]
        public int SignUpId { get; set; }

        [Required(ErrorMessage = "Must Select Hours within Group Timeframe")]
        public string SignUpTimeLength { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
