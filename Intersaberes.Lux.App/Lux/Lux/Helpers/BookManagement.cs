using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Lux.Infra;
using Lux.Interfaces;
using Lux.Models;
using Lux.Services;
using Microsoft.AppCenter.Analytics;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Lux.Helpers
{
    public static class BookManagement
    {

        public static string ReadBook(int idLivro){

            try
            {
                string caminho = string.Empty;
                var t = new Task(async () =>
                {
                    LivroBaixado livroCrypto;

                    var realm = new RealmBase();
                    livroCrypto = realm.GetByIdLivroBaixado(idLivro);

                    string livroCriptografado = App.Current.Properties[livroCrypto.LivroCrypto].ToString();

                    Debug.WriteLine("Lenght: " + livroCriptografado.Length);
                    var config = DependencyService.Get<IConfig>();
                    //caminho = await config.SalvaLivro(livroCrypto.livroCrypto, livroCrypto.titulo);
                    caminho = await config.SalvaLivro(livroCriptografado, livroCrypto.Titulo);
                    Analytics.TrackEvent("Lendo o Livro " + livroCrypto.Titulo);
                });

                t.Start();
                t.Wait();

                // salva o livro que esta sendo lido
                Application.Current.Properties["BookIdRead"] = idLivro;
                Application.Current.SavePropertiesAsync();

                return caminho;
            }
            catch(Exception ex){

                Debug.WriteLine("Error: " + ex.Message);
                return "";
            }

        }

        public static async Task<List<string>> DownloadBook(bool isAmostra, Livro item, string isImage, string isAudio, string isVideo)
        {

            var config = DependencyService.Get<IConfig>();
            List<String> livroCrypto = new List<string>();

            try
            {

                if (isAmostra)
                {
                    livroCrypto = await RestService.GetAmostra(config.DeviceID, item.Id.ToString());
                }
                else
                {
                    livroCrypto = await RestService.GetePubLivro(config.DeviceID, item.Id.ToString(), isImage, isAudio, isVideo);
                }

                // falha ao baixar o livro da webApi
                if (livroCrypto != null)
                {

                    if (livroCrypto[0].Contains("ERROR"))
                    {
                        // retorna o erro
                        return livroCrypto;
                    }
                    else
                    {

                        //Salva no banco que o livro foi baixado
                        var realm = new RealmBase();
                        LivroBaixado livroBaixado = realm.GetByIdLivroBaixado((int)item.Id);
                        if (livroBaixado != null) realm.DeleteRealm(livroBaixado);

                        // salva a criptografia do livro que esta sendo lido
                        Application.Current.Properties["BookDownload"+ item.Id.ToString()] = livroCrypto[1];
                        await Application.Current.SavePropertiesAsync();

                        var newLivroBaixado = new LivroBaixado();
                        newLivroBaixado.Id = Convert.ToInt64(item.Id.ToString());
                        newLivroBaixado.IdUsuario = App.user.Id;
                        newLivroBaixado.Titulo = item.Titulo;
                        newLivroBaixado.LivroCrypto = "BookDownload" + item.Id.ToString();//livroCrypto[1];


                        var realm2 = new RealmBase();
                        realm2.InsertUpdate(newLivroBaixado);

                        // salva o livro que esta sendo lido
                        Application.Current.Properties["BookIdRead"] = item.Id;
                        await Application.Current.SavePropertiesAsync();

                        // retorna o caminho
                        List<String> livroCryptoRetorno = new List<string>();
                        livroCryptoRetorno.Add("SUECESSO");
                        livroCryptoRetorno.Add(await config.SalvaLivro(livroCrypto[1], item.Titulo));

                        return livroCryptoRetorno;

                    }
                }

                // Arquivo veio null do servidor
                livroCrypto.Clear();
                livroCrypto.Add("ERROR");
                livroCrypto.Add("Arquivo veio vazio do servidor, tente novamente.");

                return livroCrypto;
            }
            catch(Exception ex){

                livroCrypto.Clear();
                livroCrypto.Add("ERROR");
                livroCrypto.Add("Error inesperado ao fazer o download. " + ex);

                return livroCrypto;
            }
            finally
            {
                // clear memory
                System.GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }

    

        public static async Task UpdateWebApiPercentualLeitura()
        {
            try
            {
                int idLivro = 0;

                if (Application.Current.Properties.ContainsKey("BookIdRead"))
                {
                    idLivro = Convert.ToInt32(Application.Current.Properties["BookIdRead"].ToString());
                }

                PercentLeitura perc = new PercentLeitura();
                perc.Id = idLivro;
                perc.IdUsuario = (int)App.user.Id;

                if (App.Current.Properties.ContainsKey("PercentualLidoLivro"))
                {

                    //pego percentual salvo pelo webview custon
                    string percentualLido = App.Current.Properties["PercentualLidoLivro"].ToString();
                    string percentuaTotal = App.Current.Properties["PercentualTotalLivro"].ToString();

                    // calculo para pegar o percentual
                    double result = Convert.ToDouble(percentualLido) / Convert.ToDouble(percentuaTotal);
                    perc.PercentualLeitura = Math.Round(result * 100, 1);

                }
                else perc.PercentualLeitura = 0;
                perc.DtUltimaLeitura = DateTime.Now.ToString();

                // temos internet, enTão atualiza o pertual no webApi
                if (CrossConnectivity.Current.IsConnected)
                {
                    // envio para a web Api atualizar o percentual do livro
                    await RestService.AtualizarPercent(perc.Id, perc.IdUsuario, perc.PercentualLeitura);

                    var realm = new RealmBase();

                    ObservableCollection<PercentLeitura> listPercentualLivro = new ObservableCollection<PercentLeitura>();

                    // busco todos percentuais...
                    listPercentualLivro = realm.GetByPercentLeituraAll();

                    // temos percentuais a enviar?
                    if (listPercentualLivro.Count > 0)
                    {
                        // atualiza a webApi com percentual de leitura
                        foreach (var percentLivro in listPercentualLivro)
                        {
                            var retorno = await RestService.AtualizarPercent(percentLivro.Id, percentLivro.IdUsuario, percentLivro.PercentualLeitura);

                            // enviou? então deleta o arquivo
                            if (retorno) realm.DeleteRealm(percentLivro);
                        }
                    }

                }
                else
                { // Não temos internet

                    // grava a percentual de leitura e quando foi a ultima vez lido o livro
                    var realm = new RealmBase();
                    realm.InsertUpdate(perc);
                }

            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "BookManagement", "UpdateWebApiPercentualLeitura()");
            }

        }




    }
}
