using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BlogProject.Areas.Identity.Model
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="İsim alanı boş geçilemez")]
        public string FirstName { get; set; }

        
        
        [Required(ErrorMessage ="Soyisim alanı boş geçilemez")]
        public string LastName { get; set; }

        
        
        [Required(ErrorMessage ="Kullanıcı adı boş geçilemez")]
        public string UserName { get; set; }


        
        [Required(ErrorMessage = "Başlık alanı boş geçilemez")]
        public string Title { get; set; }



      
        public IFormFile Avatar { get; set; }



        [Required(ErrorMessage = "Email boş geçilemez")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }



        [Required(ErrorMessage ="Şifre boş geçilemez")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    
    
    
    
    }
}
