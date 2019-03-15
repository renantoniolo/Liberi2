using System;
using Lux.Interfaces;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Lux.iOS.Dependencias;
using Helpers;
using Lux.Helpers;

[assembly: Xamarin.Forms.Dependency(typeof(Config))]
namespace Lux.iOS.Dependencias
{
    public class Config : IConfig
    {
        private string versionApp;
        private string diretorio;
        private string deviceID;

        public string Diretorio
        {
            get
            {
                if (string.IsNullOrEmpty(diretorio)) diretorio = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);//Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

                return diretorio;
            }
        }
        public string Version
        {
            get
            {
                if (string.IsNullOrEmpty(versionApp))
                {
                    var pinfo = "0";//Xamarin.Forms.Forms.Context.PackageManager.GetPackageInfo("startupbit.lux", Android.Content.PM.PackageInfoFlags.Activities);
                    versionApp = null;//pinfo.VersionName;
                }
                return versionApp;
            }
        }
        public string DeviceID
        {
            get
            {
                if (string.IsNullOrEmpty(deviceID)) deviceID = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString();

                return deviceID;
            }

        }

        public async Task<string> SalvaLivro(string pub64, string livroNome)
        {
            try
            {
                string livro = Regex.Replace(livroNome.Replace(" ", ""), "[^0-9a-zA-Z]+", "");
                string caminho = Path.Combine(Diretorio, livro);
                string caminhCrip = caminho + ".crip";
                string caminhoZip = caminho + ".zip";

                App.listLivroDir.Add(caminho);
                Task task = new Task(() =>
                {

                    //Descriptografar o arquivo 
                    Crypto crypto = new Crypto();
                    byte[] Buffer = Convert.FromBase64String(crypto.DecriptString(pub64));

                    FileStream Stream = new FileStream(caminhoZip, FileMode.Create);//Crio o arquivo em disco e um fluxo
                    Stream.Write(Buffer, 0, Buffer.Length);//Escrevo arquivo no fluxo
                    Stream.Close(); //Fecho fluxo pra finalmente salvar em disco

                    //Descompactar arquivo
                    ExtractZipToDirectory.ExtractToDirectoryFile(caminhoZip, caminho);
                    //Compression.ImprovedExtractToDirectory(caminhoZip, caminho);

                });

                task.Start();
                task.Wait();

                return caminho;
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "Config(iOS)", "SalvaLivro()");
                return null;
            }
        }

        public async Task DirectoryDeleteAll()
        {
            try
            {
                new Task(() =>
                {
                    foreach (var dirLivro in App.listLivroDir)
                    {
                        if (Directory.Exists(dirLivro))
                            Directory.Delete(dirLivro, true);

                        if (File.Exists(dirLivro + ".zip"))
                            File.Delete(dirLivro + ".zip");
                    }

                    App.listLivroDir.Clear();
                }).Start();

            }
            catch(Exception exception)
            {
                Function.AppCenterCrashes(exception, "Config(iOS)", "DirectoryDeleteAll()");
            }

        }
    }
}

