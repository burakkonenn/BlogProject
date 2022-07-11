using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class ContactView
    {
        public string Id { get; set; }

        public string NickName { get; set; }

        public string Avatar { get; set; }
        public ContactView Contact { get; set; }
    }
}
