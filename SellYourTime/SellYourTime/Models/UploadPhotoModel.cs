using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SellYourTime.Models
{
    public class UploadPhotoModel
    {
        [Required]
        public HttpPostedFileBase FirstPostedFile { get; set; }
        public HttpPostedFileBase SecondPostedFile { get; set; }
        public HttpPostedFileBase ThirdPostedFile { get; set; }
    }
}