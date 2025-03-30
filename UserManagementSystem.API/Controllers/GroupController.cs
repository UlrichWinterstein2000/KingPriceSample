using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using UserManagementSystem.DAL.Data;
using UserManagementSystem.DAL.DTO;
using UserManagementSystem.DAL.Entities;

namespace UserManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GroupController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            return await _context.Groups
            .Include(g => g.GroupPermissions)
                .ThenInclude(gp => gp.Permission)
            .Include(g => g.UserGroups)
                .ThenInclude(ug => ug.User)
            .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UpdateGroupDTO>> GetGroup(int id)
        {
            UpdateGroupDTO updateGroupDTO = new UpdateGroupDTO();
            var group = await _context.Groups.FindAsync(id);
            updateGroupDTO.Name = group?.Name;
            updateGroupDTO.Id = group!.Id;
            updateGroupDTO.SelectedPermissions = _context.GroupPermissions.Where(x => x.GroupId == group.Id).Select(x => x.PermissionId).ToList();
            updateGroupDTO.SelectedUsers = _context.UserGroups.Where(x => x.GroupId == group.Id).Select(x => x.UserId).ToList();
            if (group == null)
            {
                return NotFound();
            }

            return updateGroupDTO;
        }

        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] UpdateGroupDTO updateGroupDTO)
        {
            if (id != updateGroupDTO.Id)
            {
                return BadRequest();
            }

            var existingGroup = await _context.Groups
                .Include(g => g.UserGroups)
                .Include(g => g.GroupPermissions)
                .FirstOrDefaultAsync(g => g.Id == id);

            existingGroup!.Name = updateGroupDTO.Name;

            var currentUsers = existingGroup.UserGroups
                .Select(ug => ug.UserId)
                .ToList();

            var usersToAdd = updateGroupDTO.SelectedUsers
                .Except(currentUsers)
                .Select(userId => new UserGroup { GroupId = id, UserId = userId })
                .ToList();

            var usersToRemove = existingGroup.UserGroups
                .Where(ug => !updateGroupDTO.SelectedUsers
                .Contains(ug.UserId))
                .ToList();

            _context.UserGroups
                .RemoveRange(usersToRemove);

            _context.UserGroups
                .AddRange(usersToAdd);

            var currentPermissions = existingGroup.GroupPermissions
                .Select(gp => gp.PermissionId)
                .ToList();

            var permissionsToAdd = updateGroupDTO.SelectedPermissions
                .Except(currentPermissions)
                .Select(permissionId => new GroupPermission { GroupId = id, PermissionId = permissionId })
                .ToList();

            var permissionsToRemove = existingGroup.GroupPermissions
                .Where(gp => !updateGroupDTO.SelectedPermissions
                .Contains(gp.PermissionId))
                .ToList();

            _context.GroupPermissions
                .RemoveRange(permissionsToRemove);

            _context.GroupPermissions
                .AddRange(permissionsToAdd);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Groups.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetGroupCount()
        {
            int count = await _context.Groups.CountAsync();
            return Ok(count);
        }
    }
}
