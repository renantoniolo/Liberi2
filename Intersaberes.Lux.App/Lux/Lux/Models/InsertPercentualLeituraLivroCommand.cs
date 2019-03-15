using System;
namespace Lux.Models
{
    public class InsertPercentualLeituraLivroCommand
    {
        public int IdLivro { get; set; }

        public int IdUsuario { get; set; }

        public double Percentual { get; set; }
    }
}
