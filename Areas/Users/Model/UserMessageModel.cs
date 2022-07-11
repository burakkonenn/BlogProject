using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class UserMessageModel
    {
        public int MessageId { get; set; }
        public string SenderFirstname { get; set; }
        public string SenderAvatar { get; set; }
        public string SenderMessageText { get; set; }
        public DateTime MessageDate { get; set; }
        public string Subject { get; set; }

    }
}
