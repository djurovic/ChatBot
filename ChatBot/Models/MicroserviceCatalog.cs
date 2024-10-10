using System.ComponentModel.DataAnnotations;

namespace ChatBot.Models
{
    public class MicroserviceCatalog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string MainLink { get; set; }

        public List<MicroserviceMethod> Methods { get; set; }
    }
}
