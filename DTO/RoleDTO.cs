using BookingApi.Models;

namespace BookingApi.DTO
{
    public class RoleResponseDTO
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public bool Status { get; set; }

        public List<RoleDetailDTO> RoleDetails { get; set; }
    }

    public class RoleRequestDTO
    {
        public int? Id { get; set; }

        public string RoleName { get; set; }

        public bool Status { get; set; }
    }
}
