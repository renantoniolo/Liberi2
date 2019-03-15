using Realms;

namespace Lux.Models
{
    public class PercentLeitura : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public double PercentualLeitura { get; set; }

        public string DtUltimaLeitura { get; set; }

    }
}
