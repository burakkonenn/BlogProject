using BlogProject.Areas.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Context
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public string Text { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }
    }
}
