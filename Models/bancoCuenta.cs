using System.ComponentModel.DataAnnotations;

namespace bancontinental.Models
{
    public class BancoCuenta
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int idBanco { get; set; }

        [Required]
        public int nroCuenta { get; set; }

        [Required]
        public int saldo { get; set; }
    }
}