using System;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Lux.Models
{
    public class Comentario
    {
        public long Id { get; set; }

		public string nome { get; set; }

        public string foto { get; set; }

        public string _Comentario { get; set; }

		public DateTime dtaPubli { get; set; }
        
        [JsonIgnore]
		public ImageSource imageSource { get; set; }

        public double avaliacao { get; set; }

        public string cpf { get; set; } //cpf do usuario que postou o comentario

        [JsonIgnore]
        public bool isUser { get; set; } = false;  //Usado para exibir os icones ou não dos comentarios 

        //Comandos 
        [JsonIgnore]
        public ICommand DeleteCommand { get; set; }
        public ICommand HistoricoCommand { get; set; }
        public ICommand EditCommand { get; set; }
    }
}
