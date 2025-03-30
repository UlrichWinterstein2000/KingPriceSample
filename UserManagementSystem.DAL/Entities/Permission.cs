using System.Text.Json.Serialization;

namespace UserManagementSystem.DAL.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;

        [JsonIgnore] public ICollection<GroupPermission>? GroupPermissions { get; set; } = new List<GroupPermission>();
    }
}
