using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Problend.Models
{
    public class PalletizationPicture
    {
        [Key]
        public int PictureID { get; set; }

        public string PictureName { get; set; }

        public string Extension { get; set; }

        public int? CartonBoxId { get; set; }

        public virtual CartonBox CartonBox { get; set; }

    }
}
