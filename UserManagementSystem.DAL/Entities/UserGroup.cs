using System.Text.Json.Serialization;

namespace UserManagementSystem.DAL.Entities
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int GroupId { get; set; }
        [JsonIgnore] public Group? Group { get; set; }
    }
}
