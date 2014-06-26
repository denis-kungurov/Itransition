using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SellYourTime.Models
{
    public class Tag
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string Value { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
    }
}