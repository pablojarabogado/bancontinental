using System.ComponentModel.DataAnnotations;

namespace bancontinental.Models
{
    public class Banco
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string banco { get; set; }
    }
}