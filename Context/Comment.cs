using BlogProject.Areas.Identity;
using BlogProject.Areas.Users.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Context
{

    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }


        [ForeignKey("Blog")]
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public DateTime Date { get; set; }

    }
}
