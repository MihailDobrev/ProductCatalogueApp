using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Problend.Models
{
    public class CartonBox
    {
        public CartonBox()
        {
            this.Products = new HashSet<Product>();
            this.PalletizationPictures = new HashSet<PalletizationPicture>();
        }
        [Key]
        public int CartonBoxId { get; set; }

        [Required]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Нето в кг.")]
        public double Weight { get; set; }

        [Required]
        [Display(Name = "Дължина")]
        public double Lenght { get; set; }

        [Required]
        [Display(Name = "Ширина")]
        public double Width { get; set; }

        [Required]
        [Display(Name = "Височина")]
        public double Height { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        [Display(Name = "Снимки палетизация")]
        public virtual ICollection<PalletizationPicture> PalletizationPictures { get; set; }


    }
}