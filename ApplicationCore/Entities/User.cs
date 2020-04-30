using System;

namespace ApplicationCore.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            CreatedAt = DateTime.Now;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Verified { get; set; }
        public bool Active { get; set; }
        public string Role { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
