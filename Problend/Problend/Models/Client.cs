using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Problend.Models
{
    public class Client
    {
        public Client()
        {
            this.Products = new HashSet<Product>();
        }

        [Key]
        public int ClientId { get; set; }

        [Required]
        [Display(Name="Име на клиент")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}