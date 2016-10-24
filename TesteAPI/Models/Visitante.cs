using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesteAPI.Models
{
    public class Visitante
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Mensagem { get; set; }
        public string Endereco { get; set; }
        public string Localizacao { get; set; }
    }
}