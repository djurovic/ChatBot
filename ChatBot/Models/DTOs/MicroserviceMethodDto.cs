using System.ComponentModel.DataAnnotations;

namespace ChatBot.Models.DTOs
{
    public class MicroserviceMethodDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string MethodName { get; set; }

        [Required]
        [MaxLength(500)]
        public string MethodLink { get; set; }

        [MaxLength(1000)]
        public string QuestionExample { get; set; }

        public bool DateInterpretationNeeded { get; set; }

        [Required]
        public int MicroserviceCatalogId { get; set; }
    }
}
