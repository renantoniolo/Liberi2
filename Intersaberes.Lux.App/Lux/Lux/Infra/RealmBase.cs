using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Lux.Models;
using Realms;

namespace Lux.Infra
{
    public class RealmBase
    {

        private RealmConfiguration config;

        public RealmBase() {

            // Nova base de dados.
            config = new RealmConfiguration("LiberiBd.realm");

        }

        public bool InsertUpdate(RealmObject obj)
        {

            try
            {
                var realm = Realm.GetInstance(config);

                // grava o valor na base de dados
                realm.Write(() => realm.Add(obj, update: true));

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }


        public bool DeleteRealm(RealmObject obj)
        {

            try
            {

                var realm = Realm.GetInstance(config);

                using (var trans = realm.BeginWrite())
                {
                    realm.Remove(obj);
                    trans.Commit();
                }
                return true;
            }
            catch { return false; }

        }

        public Usuario GetByCpf(string cpf)
        {
        
            var realm = Realm.GetInstance(config);

            try
            {
                var myUser = realm.All<Usuario>().First(d => d.Cpf == cpf);
                return myUser;
            }
            catch
            {
                return null;
            }
        }

        public long GetByConfigID(string cpf)
        {

            var realm = Realm.GetInstance(config);

            try
            {
                var myUser = realm.All<Usuario>().First(d => d.Cpf == cpf);
                return myUser.configID;
            }
            catch
            {
                return 0;
            }
        }

        public ConfigUsuario GetByIdConfUsuario(int id)
        {

            var realm = Realm.GetInstance(config);

            try
            {
                var confUsuario = realm.All<ConfigUsuario>().First(d => d.Id == id);
                return confUsuario;
            }
            catch
            {
                return null;
            }
        }


        public LivroBaixado GetByIdLivroBaixado(int id)
        {
    
            var realm = Realm.GetInstance(config);

            try
            {
                var livroBaixado = realm.All<LivroBaixado>().FirstOrDefault(d => d.Id == id);
                return livroBaixado;
            }
            catch
            {
                return null;
            }
        }

        public ObservableCollection<LivroBaixado> GetByLivroBaixadoAll()
        {

 
            var realm = Realm.GetInstance(config);

            try
            {
                var listBook = realm.All<LivroBaixado>().Where(l => l.IdUsuario == App.user.Id);
                return new ObservableCollection<LivroBaixado>(listBook);

            }
            catch
            {
                return null;
            }
        }

        public PercentLeitura GetByIdPercentLeitura(int id)
        {
           
            var realm = Realm.GetInstance(config);

            try
            {
                var percentLeitura = realm.All<PercentLeitura>().FirstOrDefault(d => d.Id == id);
                return percentLeitura;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteAllPercentLeitura()
        {
        
            var realm = Realm.GetInstance(config);

            try
            {
                realm.RemoveAll("PercentLeitura");
                return true;
            }
            catch
            {
                return false;
            }

        }

        public ObservableCollection<PercentLeitura> GetByPercentLeituraAll()
        {
        
            var realm = Realm.GetInstance(config);

            try
            {
                var listPercent = realm.All<PercentLeitura>();
                return new ObservableCollection<PercentLeitura>(listPercent);
            }
            catch
            {
                return null;
            }
        }

        public bool UpdatePerfilUsuario(string nome, string email, string cidade, string imgBase64)
        {

            var realm = Realm.GetInstance(config);

            try
            {
           
                var usuario = new Usuario();

                // o objeto so pode ser alterado dentro uma gravação
                using (var trans = realm.BeginWrite())
                {
                    usuario = App.user;
                    usuario.Nome = nome;
                    usuario.Email = email;
                    usuario.Cidade = cidade;
                    usuario.Foto = imgBase64;
                    App.user = usuario;

                    trans.Commit();
                }

                return true;

            }
            catch{

                return false;
            }

        }


        /// <summary>
        /// Modifica o obj usuario para ser enviado por json.
        /// </summary>
        /// <returns>The usuario json.</returns>
        /// <param name="usuario">Usuario.</param>
        public UsuarioJson ModificUsuarioJson(Usuario usuario){

            var realm = Realm.GetInstance(config);
            var userJson = new UsuarioJson();

            try
            {

                // o objeto so pode ser alterado dentro uma gravação no realm, por isso faço isso
                using (var trans = realm.BeginWrite())
                {
                    userJson.Id = usuario.Id;
                    userJson.Nome = usuario.Nome;
                    userJson.Senha = usuario.Senha;
                    userJson.Cpf = usuario.Cpf;
                    userJson.Email = usuario.Email;
                    userJson.Cidade = usuario.Cidade;
                    userJson.Foto = usuario.Foto;
                    userJson.DtUltimoLoginApp = usuario.DtUltimoLoginApp;

                    trans.Commit();
                }
            }
            catch(Exception ex){

                Debug.WriteLine("Error: " + ex.Message);
            }

            return userJson;

        }

        public UsuarioJson SendPercentLeitura()
        {

            var realm = Realm.GetInstance(config);
            var userJson = new UsuarioJson();

            try
            {




            }
            catch (Exception ex)
            {

                Debug.WriteLine("Error: " + ex.Message);
            }

            return userJson;

        }


    }
}
