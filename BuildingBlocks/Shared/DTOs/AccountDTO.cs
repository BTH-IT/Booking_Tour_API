namespace Shared.DTOs
{
    public class AccountResponseDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

		public int RoleId { get; set; }

		public RoleResponseDTO Role { get; set; }
	}

	public class AccountRequestDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public int RoleId { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
