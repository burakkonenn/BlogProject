using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class ListModel
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
        public string UserFirstname { get; set; }
        public string UserLastname { get; set; }
        public string UserImage { get; set; }
    }
}
