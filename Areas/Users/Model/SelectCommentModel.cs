using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class SelectCommentModel
    {
        public string CommentFirstname { get; set; }
        public string CommentLastname { get; set; }
        public string CommentMessage { get; set; }
    }
}
