namespace Shared.DTOs
{
    public class AccountResponseDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

<<<<<<< HEAD
		public int RoleId { get; set; }

		public RoleResponseDTO Role { get; set; }
	}

=======
		//public string Password { get; set; }

		public int RoleId { get; set; }

		public RoleResponseDTO Role { get; set; }
	}

>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
	public class AccountRequestDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public int RoleId { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
