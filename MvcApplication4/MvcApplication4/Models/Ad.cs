using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcApplication4.Models
{
    public class Ad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Info")]
        public string Info { get; set; }
        [Required]
        [Display(Name = "Price")]
        public int Price { get; set; }
        [Display(Name = "Date create")]
        public DateTime CreateDate { get; set; }

        public virtual UserProfile Creator { get; set; }
        public virtual IList<PostRate> PostRates { get; set; }
        public virtual IList<Photo>  Photos{ get; set; }
    }
}