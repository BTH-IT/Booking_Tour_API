using Shared.Enums;

namespace Shared.DTOs
{
    public class RoleDetailDTO
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public ActionType ActionName { get; set; }

        public bool Status { get; set; }
    }
}
