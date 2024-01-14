namespace BookingApi.DTO
{
    public class PermissionRequestDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }

    public class PermissionResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
