using System.Collections.Generic;


namespace BlogProject.Context
{
    public class Entity_Type
    {
        public int Id { get; set; }
        public string Table { get; set; }
        public string Notification_Desc { get; set; }
        public List<Notification> Notifications { get; set; }

    }
}
