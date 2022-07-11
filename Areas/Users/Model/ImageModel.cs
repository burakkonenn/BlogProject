using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class ImageModel
    {
        [Required]
        public IFormFile Image { get; set; }
    }
}
