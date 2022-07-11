using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using BlogProject.Areas.Identity;
using Microsoft.AspNetCore.Authentication;

namespace BlogProject.Areas.Identity.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email alanı boş bırakılamaz")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

    }
}
