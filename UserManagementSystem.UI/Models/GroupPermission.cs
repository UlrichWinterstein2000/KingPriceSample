using UserManagementSystem.UI.Models;

namespace UserManagementSystem.DAL.Entities
{
    public class GroupPermission
    {
        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
