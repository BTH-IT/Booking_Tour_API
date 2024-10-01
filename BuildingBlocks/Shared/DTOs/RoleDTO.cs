namespace Shared.DTOs
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
		public DateTime? DeletedAt { get; set; }
	}
}
