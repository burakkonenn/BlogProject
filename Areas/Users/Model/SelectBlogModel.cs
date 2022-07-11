using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class SelectBlogModel
    {
        public int Id { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public string BlogImage { get; set; }
        public DateTime BlogDate { get; set; }
        public string BlogUserFirstName { get; set; }
        public string BlogUserLastName { get; set; }
        public string BlogUserAvatar { get; set; }
    }

}
