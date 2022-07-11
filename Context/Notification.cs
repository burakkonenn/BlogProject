using BlogProject.Areas.Identity;
using System;


namespace BlogProject.Context
{
    public class Notification
    {
        public int Id { get; set; }
        public string NotifierId { get; set; }
        public string ActorId { get; set; }
        public int? BlogId { get; set; }
        public int? FollowId { get; set; }
        public int Entity_TypeId { get; set; }
        public Entity_Type Entity_Type { get; set; }
        public Blog Blog { get; set; }
        public Follow Follow { get; set; }
        public User Notifier { get; set; }
        public User Actor { get; set; }
        public bool Status { get; set; }
        public DateTime Date { get; set; }
    }
}
