using BlogProject.Areas.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class BlogViewModel
    {
        [Required(ErrorMessage = "Yayınınız için lütfen bir başlık belirleyin")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Yayınınız için lütfen bir içerik oluşturun")]
        public string Content { get; set; }


        [Required(ErrorMessage = "Yayınınız için lütfen bir açıklama oluşturun")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Yayınız için bi resim ekleyin")]
        public IFormFile BlgImage { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Yayınız için bi etiket ekleyin")]
        public string Tag { get; set; }
        public string Image { get; set; }
        //
        public User User { get; set; }
        public string UserAvatar { get; set; }
        public string UserId { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string PersonalInfo { get; set; }
        public string Location { get; set; }
        public string WebSite { get; set; }
    }
}
