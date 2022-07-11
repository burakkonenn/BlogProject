using BlogProject.Areas.Identity;
using BlogProject.Context;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Users.Model
{
    public class UserModel
    {

        [Required]
        public string Id { get; set; }
        public int ContentLengthCount { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonelInfo { get; set; }
        public string Location { get; set; }
        public string WebSite { get; set; }

       
        [DataType(DataType.Upload)]
        public string AvatarUrl { get; set; }

       
        
        [DataType(DataType.Upload)]
        public IFormFile AvatarUpload { get; set; }


        public List<UserModel> Users { get; set; }

        //FORBLOGDETAİLS
        public List<BlogViewModel> Blogs { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
        public DateTime Date { get; set; }
        public List<SelectCommentModel> Comments { get; set; }
        
        public ICollection<Follow> Followee { get; set; }
        public IEnumerable<UserMessageModel> SenderUser { get; set; }
    }
}
