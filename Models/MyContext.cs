using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GeneralTemplate.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options)
        { }
        public DbSet<User> DbUsers { get; set; }
        public DbSet<Group> DbGroups { get; set; }
        public DbSet<RSVP> DbRSVPs { get; set; }
        public DbSet<Comment> DbComments { get; set; }
        public DbSet<SignUp> DbSignUpUsers { get; set; }
    }
}