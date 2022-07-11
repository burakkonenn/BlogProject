using BlogProject.Areas.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Models
{
    public class Message
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }


        [Required]
        public string Text { get; set; }


        [Required]
        public DateTime When { get; set; }

        public string FromUserId { get; set; }
        public virtual User Sender { get; set; }

        public string ToUserId { get; set; }

        public Message()
        {
            When = DateTime.Now;
        }
    }
}
