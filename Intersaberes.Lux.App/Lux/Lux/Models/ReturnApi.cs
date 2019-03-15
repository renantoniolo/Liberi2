using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lux.Models
{
    public class ReturnApi
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }
        
    }

    public partial class Error
    {
        [JsonProperty("property")]
        public string Property { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

	public class ReturnApiString : ReturnApi
	{
		[JsonProperty("data")]
        public String Data { get; set; }

	}

	public class ReturnApiUsuario : ReturnApi
    {
		[JsonProperty("data")]
        public Usuario Data { get; set; }
    }

    public class ReturnApiListLivro : ReturnApi
    {
        [JsonProperty("data")]
        public List<Livro> Data { get; set; }

    }

    public class ReturnApiComentario : ReturnApi
    {
        [JsonProperty("data")]
        public Comentario Data { get; set; }

    }

    public class ReturnApiListComentario : ReturnApi
    {
        [JsonProperty("data")]
        public List<Comentario> Data { get; set; }

    }

}
