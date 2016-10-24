using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteWeb.Models
{
    public class Visitante
    {
        public int Id { get; set; }

        [StringLength(150, MinimumLength = 10)]
        public string Nome { get; set; }

        [StringLength(500, MinimumLength = 10)]
        public string Mensagem { get; set; }

        [Display(Name = "Endereço")]
        [StringLength(100, MinimumLength = 10)]
        public string Endereco { get; set; }

        public string Localizacao { get; set; }
    }
}