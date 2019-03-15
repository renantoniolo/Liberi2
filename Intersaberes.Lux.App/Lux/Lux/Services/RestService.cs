using System;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Lux.Models;
using Lux.Helpers;
using System.Collections.Generic;
using Plugin.Connectivity;
using MonkeyCache.FileStore;
using Realms;
using Lux.Infra;

namespace Lux.Services
{
    public static class RestService
    {

        //static string urlBase = AppConfig.urlBasePROD;
        static string urlBase = AppConfig.urlBaseQA;
        //static string urlBase = AppConfig.urlBaseDEV;

        //Usuario
        public static async Task<LoginModel> Login(string login, string senha)
        {
            try
            {
                LoginModel loginModel = new LoginModel();

                string url = urlBase + "usuario/login?login=" + login + "&senha=" + senha + "&plataforma=App";

                HttpResponseMessage response = await HttpUtil.PostAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    ReturnApiUsuario retorno = JsonConvert.DeserializeObject<ReturnApiUsuario>(response.Content.ReadAsStringAsync().Result);

                    loginModel.user = retorno.Data;
                    loginModel.mensagem = "Logado com sucesso.";
                }
                else
                { //erro

                    ReturnApiUsuario retorno = JsonConvert.DeserializeObject<ReturnApiUsuario>(response.Content.ReadAsStringAsync().Result);

                    loginModel.user = null;
                    loginModel.mensagem = retorno.Errors[0].Message.Replace("(", "").Replace(")", "");
                }

                return loginModel;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return null;
            }
        }
        public static async Task<string> TrocarSenha(string email, string senha, string newSenha)
        {
            try
            {
                string url = urlBase + "usuario/novaSenha?email=" + email + "&senha=" + senha.Replace("#","%23") + "&senhaNova=" + newSenha.Replace("#", "%23");

                HttpResponseMessage response = await HttpUtil.PostAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    ReturnApiString retorno = JsonConvert.DeserializeObject<ReturnApiString>(response.Content.ReadAsStringAsync().Result);

                    return retorno.Data;
                }
                else
                { //erro
                    return "Ocorreu algo inesperado. Por favor, tente novamente mais tarde ou contate a equipe de suporte!";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return null;
            }
        }
        public static async Task<string> PostUsuario(Usuario usuario)
        {
            try
            {
                string url = urlBase + "usuario/cadastro";
                string retornoStr = "Falha ao cadastar";

                HttpResponseMessage response = await HttpUtil.PostAsync(url, usuario);

                ReturnApi retorno = JsonConvert.DeserializeObject<ReturnApi>(response.Content.ReadAsStringAsync().Result);

                if (retorno.Success)
                {
                    retornoStr = "usuário cadastro com sucesso!";
                }
                else
                { // erro
                    retornoStr = retorno.Errors[0].Message.Replace("(", "").Replace(")", "");
                }

                return retornoStr;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return null;
            }
        }
        public static async Task<bool> PutUsuario(Usuario usuario)
        {
            try
            {

                var realm = new RealmBase();

                // modifica o usuarioJson
                var userJson = realm.ModificUsuarioJson(usuario);

                string url = urlBase + "usuario/atualizar";

                HttpResponseMessage response = await HttpUtil.PutAsync(url, userJson);

                bool result = false;

                switch ((int)response.StatusCode)
                {
                    case 200:
                        Debug.WriteLine("Cadastro atualizado com sucesso!");
                        result = true;
                        break;
                    default:
                        Debug.WriteLine("Falha ao atualizar seu Cadastro");
                        result = false;
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return false;
            }
        }
        public static async Task<string> PostRecuperarSenha(String email)
        {
            try
            {
                string url = urlBase + "usuario/recuperarSenha?email=" + email;

                HttpResponseMessage response = await HttpUtil.PostAsync(url);


                ReturnApiString retorno = JsonConvert.DeserializeObject<ReturnApiString>(response.Content.ReadAsStringAsync().Result);

                return retorno.Data;


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return "Falha ao recuprar senha!";
            }
        }
        public static async Task<bool> AtualizarPercent(int IdLivro, int IdUsuario, double valor)
        {
            try
            {

                // cria o objeto a ser enviado
                InsertPercentualLeituraLivroCommand insertPercentual = new InsertPercentualLeituraLivroCommand();

                insertPercentual.IdLivro = IdLivro;
                insertPercentual.IdUsuario = IdUsuario;
                insertPercentual.Percentual = valor;

                string url = urlBase + "livro/percentualleituralivroweb";

                HttpResponseMessage response = await HttpUtil.PostAsync(url, insertPercentual);

                if (response.IsSuccessStatusCode) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return false;
            }
        }

        //Livros
        public static async Task<List<Livro>> GetAllLivros()
        {

            ReturnApiListLivro restlivro = null;
            var json = "";
            string url = urlBase + "livro/allPublished?IdUsuario=" + App.user.Id;

            try
            {
                //check if we are connected
                if (!CrossConnectivity.Current.IsConnected)
                    json = Barrel.Current.Get(url);

                if (string.IsNullOrWhiteSpace(json))
                {
                    HttpResponseMessage resp = await HttpUtil.GetAsync(url);
                    var resparray = await resp.Content.ReadAsStringAsync();

                    Barrel.Current.Add(url, resparray, TimeSpan.FromDays(1));
                    json = resparray;

                }

                restlivro = JsonConvert.DeserializeObject<ReturnApiListLivro>(json);
                return restlivro.Data;

            }
            catch(Exception ex){
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return null;
            }
        }
        public static async Task<List<String>> GetePubLivro(string deviceID, string codigo, string isImage, string isAudio, string isVideo )
        {
            List<String> listaRetorno = new List<string>();

            try
            {
                string uri = urlBase + "livro/livro/" + deviceID + "/" + codigo + "/" + isImage + "/" + isAudio + "/" + isVideo;

                var resp = await HttpUtil.GetAsync(uri);
                String resparray = await resp.Content.ReadAsStringAsync();

                Debug.WriteLine("Lenght: " + resparray.Length);
                ReturnApiString livroApi = JsonConvert.DeserializeObject<ReturnApiString>(resparray);
                if (livroApi.Success)
                {
                    listaRetorno.Add("SUCESSO");
                    listaRetorno.Add(livroApi.Data);
                    return listaRetorno;
                }
                else
                {
                    listaRetorno.Add("ERROR");
                    listaRetorno.Add("Falha ao receber resposta do servidor.");
                    return listaRetorno;
                }
                    
            }
            catch (TimeoutException timeEx)
            {
                listaRetorno.Add("ERROR");
                listaRetorno.Add("Falha de Timeout: " + timeEx.Message);

                // The request timed out
                return listaRetorno;
            }
            catch (Exception ex)
            {
                listaRetorno.Add("ERROR");
                listaRetorno.Add("Erro ao fazer o download: " + ex.Message);

                return listaRetorno;
            }
        }

        public static async Task<List<string>> GetAmostra(string deviceID, string codigo)
        {
            List<String> listaRetorno = new List<string>();

            try
            {
                string uri = urlBase + "livro/livroAmostra/" + deviceID + "/" + codigo;

                var resp = await HttpUtil.GetAsync(uri);
                var resparray = await resp.Content.ReadAsStringAsync();

                ReturnApiString livroApi = JsonConvert.DeserializeObject<ReturnApiString>(resparray);
                if (livroApi.Success)
                {
                    listaRetorno.Add("SUCESSO");
                    listaRetorno.Add(livroApi.Data);
                    return listaRetorno;
                }
                else
                {
                    listaRetorno.Add("ERROR");
                    listaRetorno.Add("Falha ao receber resposta do servidor.");
                    return listaRetorno;
                }

            }
            catch (TimeoutException timeEx)
            {
                listaRetorno.Add("ERROR");
                listaRetorno.Add("Falha de Timeout: " + timeEx.Message);

                // The request timed out
                return listaRetorno;
            }
            catch (Exception ex)
            {
                listaRetorno.Add("ERROR");
                listaRetorno.Add("Erro ao fazer o download: " + ex.Message);

                return listaRetorno;
            }
        }

        //Comentarios
        public static async Task<Comentario> GetComentario(int id_livro)
        {
            try
            {
                string url = urlBase + "comentario/GetFirst?idLivro=" + id_livro;

                var resp = await HttpUtil.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    var resparray = await resp.Content.ReadAsStringAsync();
                    ReturnApiComentario restlivro = JsonConvert.DeserializeObject<ReturnApiComentario>(resparray);

                    return restlivro.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return null;
            }
        }
        public static async Task<List<Comentario>> GetAllComentarios(int id_livro)
        {
            try
            {
                string url = urlBase + "comentario/GetAll?idLivro=" + id_livro;

                var resp = await HttpUtil.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    var resparray = await resp.Content.ReadAsStringAsync();
                    ReturnApiListComentario restComentario = JsonConvert.DeserializeObject<ReturnApiListComentario>(resparray);

                    return restComentario.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return null;
            }
        }
		public static async Task<bool> PostComentario(int idLivro, Usuario user, string comentario,double aval)
        {
            try
            {
                string url = urlBase + "comentario/Publicar";

                var postComentario = new { 
                    id_livro = idLivro, 
                    cpf_usuario = user.Cpf, 
                    _Comentario = comentario,
                    avaliacao = aval.ToString().Replace(",",".")
                };  
				
                HttpResponseMessage response = await HttpUtil.PostAsync(url, postComentario);

                return response.IsSuccessStatusCode;
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return false;
            }

        }
        public static async Task<string> DeleteComentario(long idComentario)
        {
            try
            {
                string url = urlBase + "comentario/Deletar";

                HttpResponseMessage response = await HttpUtil.PostAsync(url, idComentario);
                var resparray = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return "Comentario excluido com sucesso";
                else
                {
                    ReturnApiString mensagem = JsonConvert.DeserializeObject<ReturnApiString>(resparray);
                    if (mensagem.Success)
                        return "Comentario excluido com sucesso";
                    else return mensagem.Errors[0].ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return "Falha ao excluir seu comentario";
            }

        }
        public static async Task<List<Comentario>> GetHistoricoComentario(string cpf,long idLivro)
        {
            try
            {
                string url = urlBase + "comentario/GetHistorico?cpf="+ cpf + "&idLivro=" + idLivro;

                var resp = await HttpUtil.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    var resparray = await resp.Content.ReadAsStringAsync();
                    ReturnApiListComentario restComentario = JsonConvert.DeserializeObject<ReturnApiListComentario>(resparray);

                    return restComentario.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return null;
            }

        }
        public static async Task<bool> UpdateComentario(string idComentario, string cometario, double aval)
        {
            try
            {
                string url = urlBase + "comentario/Update?idComentario=" + idComentario + "&novoComentario=" + cometario + "&Aval=" + aval.ToString().Replace(",", ".");

                HttpResponseMessage response = await HttpUtil.PutAsync(url);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no processamento e/ou no post " + ex.Message);
                return false;
            }

        }
    }
}