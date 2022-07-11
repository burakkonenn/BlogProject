using BlogProject.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class UserBlogNotification
    {
        public string UserBlogFirstname { get; set; }
        public string UserBlogAvatar { get; set; }
        public string UserBlogMessage { get; set; }
        public string UserBlogNotMessage { get; set; }
        public string UserBlog { get; set; }
    }
}
