using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SellYourTime.Models
{
    public class Offer
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual string Description { get; set; }
        public virtual int? Price { get; set; }
        public virtual string FirstPhotoPath { get; set; }
        public virtual string SecondPhotoPath { get; set; }
        public virtual string ThirdPhotoPath { get; set; }
        public virtual DateTime DateAdded { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual double? Rating { get; set; }
        public virtual double? SumRating { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
    }
}