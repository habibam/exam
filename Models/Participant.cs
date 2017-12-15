using System;
namespace loginregister.Models
{
    public class Participant : BaseEntity
    {
        public int ParticipantId { get; set; }
        public int ActivityId { get; set; }
        public Activity ActivityDetail { get; set; }
        public int UserId { get; set; }
        public User UserDetail { get; set; }
    }
}