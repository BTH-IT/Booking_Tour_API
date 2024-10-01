    
namespace Shared.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }

        public string Fullname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }
        public AccountResponseDTO Account { get; set; }
    }

    public class UserRequestDTO
    {
        public int? Id { get; set; }

        public string Fullname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }
        public int AccountId { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
