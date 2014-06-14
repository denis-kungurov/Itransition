using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcApplication4.Models
{
    public class PostRateContext : DbContext
    {
        public PostRateContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<PostRate> PostRates { get; set; }
    }

    public class PostRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int Rating { get; set; }

        public virtual Ad Ad{ get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}