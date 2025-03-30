using System.ComponentModel.DataAnnotations;
using UserManagementSystem.DAL.Entities;

namespace UserManagementSystem.UI.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public ICollection<UserGroup>? UserGroups { get; set; } = new List<UserGroup>();
        public ICollection<GroupPermission>? GroupPermissions { get; set; } = new List<GroupPermission>();

        public List<User>? AvailableUsers { get; set; } = new();
        public List<int>? SelectedUsers { get; set; } = new();

        public List<Permission>? AvailablePermissions { get; set; } = new();
        public List<int>? SelectedPermissions { get; set; } = new();
    }
}
