using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Problend.Models
{
    public class EditUserViewModel
    {
        [Display(Name ="Имейл")]
        public ApplicationUser User { get; set; }

        [Display(Name = "Парола")]
        public string Password { get; set; }

        [Display(Name ="Потвърди паролата")]
        [Compare("Password",ErrorMessage ="Паролите не съвпадат!")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Потребителски роли")]
        public IList<Role> Roles { get; set; }
    }
}