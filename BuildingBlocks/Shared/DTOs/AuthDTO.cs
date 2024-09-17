namespace Shared.DTOs
{
    public class AuthLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthRegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }
    }
}
