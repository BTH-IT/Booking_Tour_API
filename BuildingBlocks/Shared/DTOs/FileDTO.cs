namespace Shared.DTOs
{
    public class FileDTO
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public FileDTO(string Url, string Name, string Type)
        {
            this.Url = Url;
            this.Name = Name;
            this.Type = Type;
        }
    }
}
