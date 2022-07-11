using BlogProject.Areas.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Context
{
    public class Likes
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public bool IsLike { get; set; }
        public DateTime Date { get; set; }


    }
}
