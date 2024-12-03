namespace Shared.DTOs
{
    public class DestinationRequestDTO
    {
        public DestinationRequestDTO()
        {
        }

<<<<<<< HEAD
        public DestinationRequestDTO(string name, string description, string url)
        {
=======
        public DestinationRequestDTO(int? id, string name, string description, string url)
        {
            Id = id;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
            Name = name;
            Description = description;
            Url = url;
        }

<<<<<<< HEAD
=======
        public int? Id { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }


    }

    public class DestinationResponseDTO
    {
        public DestinationResponseDTO(int id, string name, string description, string url)
        {
            Id = id;
            Name = name;
            Description = description;
            Url = url;
        }
<<<<<<< HEAD

=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public DestinationResponseDTO()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

    }
}
