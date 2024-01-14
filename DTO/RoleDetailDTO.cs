using BookingApi.Models;

namespace BookingApi.DTO
{
    public class RoleDetailDTO
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public ActionType ActionName { get; set; }

        public bool Status { get; set; }
    }
}
