using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace BlogProject.Areas.Users.Model
{
    public class RegisterEditViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="Boş alan bırakmayınız")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Boş alan bırakmayınız")]
        public string PersonalInfo { get; set; }

        [Required(ErrorMessage = "Boş alan bırakmayınız")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Boş alan bırakmayınız")]
        public string WebSite { get; set; }

        public string AvatarUrl { get; set; }

        [DataType(DataType.ImageUrl)]
        public IFormFile AvatarUpload { get; set; }


    }
}
