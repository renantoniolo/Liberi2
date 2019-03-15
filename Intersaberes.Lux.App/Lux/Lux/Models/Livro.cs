using System;
using Newtonsoft.Json;

namespace Lux.Models
{
    public class Livro
    {
        public long Id { get; set; }

        public string IsbnDigital { get; set; }

        public string Titulo { get; set; }

        public string SubTitulo { get; set; }

        public string Autor { get; set; }

        public string Capa { get; set; }

        public string Sinopse { get; set; }

        public string DtaCadastro { get; set; }

        public string PrecoDigital { get; set; }

        public string PrecoImpresso { get; set; }

        public double Avaliacao { get; set; }

        public double Percentual { get; set; }

        public int Paginas { get; set; }

        [JsonIgnore]
        public string isBuy { get; set; }

        public DateTime? Dta_UltimaLeitura { get; set; }
    }
}