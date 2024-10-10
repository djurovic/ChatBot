namespace ChatBot.Models.DTOs
{
    public class MicroserviceCatalogDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MainLink { get; set; }
        public List<MicroserviceMethodDto> Methods { get; set; }
    }
}
