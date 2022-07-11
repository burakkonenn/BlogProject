using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class UserNotifications
    {
        public string UserCommentFirstname { get; set; }
        public string UserCommentAvatar { get; set; }
        public string UserCommentMessage { get; set; }
        public string UserCommentNotMessage { get; set; }
        public string UserBlogFirstname { get; set; }
        public List<UserBlogNotification> UserBlogNotifications { get; set; }
        public string UserBlogMessage { get; set; }
        public string UserBlogNotMessage { get; set; }
    }
}
