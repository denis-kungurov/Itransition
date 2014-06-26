using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SellYourTime.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        public virtual UserProfile RaterProfile { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}