
using Newtonsoft.Json;

namespace Lux.Models
{
    public class ItensMenu
    {
        public int id { get; set; }

        public int idPage { get; set; }//Id da pagina a qual o sumario deve levar 

        public string lblView { get; set; }

        public string sourceIcon { get; set; }

        [JsonIgnore]
        public string backgroundColor { get; set; }
    }
}
