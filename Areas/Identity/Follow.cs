using BlogProject.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Identity
{
    public class Follow
    {


        public int Id { get; set; }
        public string FollowerId { get; set; }
        public string FolloweeId { get; set; }
        public User Follower { get; set; }
        public User Followee { get; set; }
        public bool IsFollowing { get; set; }
        public List<Notification> Notification { get; set; }


    }
}
