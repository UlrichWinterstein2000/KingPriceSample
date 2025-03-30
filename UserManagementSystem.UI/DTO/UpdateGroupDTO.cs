namespace UserManagementSystem.UI.DTO
{
    public class UpdateGroupDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<int>? SelectedUsers { get; set; } = new();
        public List<int>? SelectedPermissions { get; set; } = new();
    }
}
