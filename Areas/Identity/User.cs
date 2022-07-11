using BlogProject.Context;
using BlogProject.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Identity
{
    public class User: IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Title { get; set; }
        public string Personelİnfo { get; set; }
        public string Location { get; set; }
        public string WebSite { get; set; }
        public DateTime Date { get; set; }
        public List<Blog> Blogs { get; set; }


        public ICollection<Follow> Follower { get; set; }
        public ICollection<Follow> Followee { get; set; }


        public ICollection<BlogProject.Context.Message> Receiver { get; set; }
        public ICollection<BlogProject.Context.Message> Sender { get; set; }


        public ICollection<Notification> Actor { get; set; }
        public ICollection<Notification> Notifier { get; set; }


        public ICollection<Likes> Likes { get; set; }
        public ICollection<List> List { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
