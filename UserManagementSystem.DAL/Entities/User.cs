using System.Text.Json.Serialization;

namespace UserManagementSystem.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;

        [JsonIgnore] public ICollection<UserGroup>? UserGroups { get; set; } = new List<UserGroup>();
    }
}
