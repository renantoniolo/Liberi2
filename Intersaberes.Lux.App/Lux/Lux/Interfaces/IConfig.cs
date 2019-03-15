using System;
using System.Threading.Tasks;

namespace Lux.Interfaces
{
    public interface IConfig
    {
        string Diretorio { get; }
        string Version { get; }
        string DeviceID { get; }
        Task<string> SalvaLivro(string pub64, string livroNome);
        Task DirectoryDeleteAll();
    }
}
