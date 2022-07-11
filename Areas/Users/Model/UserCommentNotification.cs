using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class UserCommentNotification
    {
        public string UserCommentFirstname { get; set; }
        public string UserCommentAvatar { get; set; }
        public string UserCommentMessage { get; set; }
        public string UserCommentNotMessage { get; set; }
        public string UserCommentBlog { get; set; }
    }
}
