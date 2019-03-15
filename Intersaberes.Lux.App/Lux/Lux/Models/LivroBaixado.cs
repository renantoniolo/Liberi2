using System;
using Realms;

namespace Lux.Models
{
    public class LivroBaixado : RealmObject
    {
        [PrimaryKey]
        public long Id { get; set; }

        public long IdUsuario { get; set; }

        public string Titulo { get; set; }

        public String LivroCrypto { get;set; }
    }
}
