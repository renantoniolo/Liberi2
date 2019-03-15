using System;
using Newtonsoft.Json;
using Realms;

namespace Lux.Models
{
    public class Usuario : RealmObject
    {

        public long Id { get; set; }

        public string Nome { get; set; }

        [PrimaryKey]
        public string Cpf { get; set; }

        public string Email { get; set; }

        public string Foto { get; set; }

        public string Senha { get; set; }

        public string Cidade { get; set; }

        public string DtUltimoLoginApp { get; set; }

        [JsonIgnore]
        public long configID { get; set; }

    }

    public class UsuarioJson 
    {

        public long Id { get; set; }

        public string Nome { get; set; }
    
        public string Cpf { get; set; }

        public string Email { get; set; }

        public string Foto { get; set; }

        public string Senha { get; set; }

        public string Cidade { get; set; }

        public string DtUltimoLoginApp { get; set; }


    }



}
