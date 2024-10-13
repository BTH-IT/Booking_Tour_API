namespace Shared.DTOs
{
    public class DestinationRequestDTO
    {
        public DestinationRequestDTO()
        {
        }

        public DestinationRequestDTO(string name, string description, string url)
        {
            Name = name;
            Description = description;
            Url = url;
        }

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
        public DestinationResponseDTO()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

    }
}
