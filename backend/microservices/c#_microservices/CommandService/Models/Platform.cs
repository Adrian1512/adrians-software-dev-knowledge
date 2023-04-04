using System.ComponentModel.DataAnnotations;

namespace CommandService.Models
{
    public class Platform
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int ExternalID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ICollection<Command> Commands { get; set; } = new List<Command>();

    }
}