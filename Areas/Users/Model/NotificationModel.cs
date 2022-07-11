using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class NotificationModel
    {

        public List<UserCommentNotification> UserCommentNotifications { get; set; }
        public List<UserBlogNotification> UserBlogNotifications { get; set; }

    }
}
