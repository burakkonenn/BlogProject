using BlogProject.Areas.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Context
{

    [Table("Blog")]
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yayınınız için lütfen bir başlık belirleyin" )]
        public string Title { get; set; }

        
        [Required(ErrorMessage = "Yayınınız için lütfen bir içerik oluşturun")]
        public string Content { get; set; }


        [Required(ErrorMessage = "Yayınınız için lütfen bir açıklama oluşturun")]
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Likes> Likes { get; set; }
        public ICollection<List> List { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Notification> Notification { get; set; }

    }
}
