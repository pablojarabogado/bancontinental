using System.ComponentModel.DataAnnotations;

namespace bancontinental.Models
{
    public class BancoCuentaTrnsaccion
    {
        [Key]
        public int idNroTransaccion { get; set; }

        [Required]
        public int nroCuentaOrigen { get; set; }
        [Required]
        public int idBancoOrigen { get; set; }
        [Required]
        public int nroCuentaDestino { get; set; }
        [Required]
        public int idBancoDestino { get; set; }
        [Required]
        public int monto { get; set; }

        public string estado { get; set; }

        public bool envio { get; set; }

    }
}