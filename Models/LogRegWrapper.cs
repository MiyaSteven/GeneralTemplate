using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GeneralTemplate.Models
{
    public class LogRegWrapper
    {
        public User Register { get; set; }
        public LogUser Login { get; set; }
    }
}