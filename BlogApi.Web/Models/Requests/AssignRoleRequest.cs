using System.ComponentModel.DataAnnotations;

namespace BlogApi.Web.Models.Requests
{
    public class AssignRoleRequest
    {
        [Required(ErrorMessage = "Role ID is required")]
        public int RoleId { get; set; }
    }
}