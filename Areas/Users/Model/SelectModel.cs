using BlogProject.Areas.Identity;
using BlogProject.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class SelectModel
    {
        public string UserId { get; set; }
        public string TagName { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public string BlogImage { get; set; }
        public DateTime BlogDate { get; set; }



        //user
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string PersonelInfo { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }

        ///
        public List<User> Users { get; set; }
        public List<SelectModel> BlogSelectModel { get; set; }
        public bool IsFollowing { get; set; }
    }
    

}
