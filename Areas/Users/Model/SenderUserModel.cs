using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class SenderUserModel
    {
        public string SenderFirstName { get; set; }
        public string SenderAvatar { get; set; }
        public string SenderMessage { get; set; }
        public DateTime Date { get; set; }
    }
}
