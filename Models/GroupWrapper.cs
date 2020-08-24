using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GeneralTemplate.Models
{
    public class GroupWrapper
    {
        public User LoggedUser { get; set; }
        public Group GroupForm { get; set; }
        public List<Group> AllGroups { get; set; }
        public List<User> AllUsers { get; set; }
        public List<RSVP> AllRSVPs { get; set; }
    }
}