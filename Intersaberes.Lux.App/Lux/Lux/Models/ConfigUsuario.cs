using Realms;

namespace Lux.Models
{
    public class ConfigUsuario : RealmObject
    {
        [PrimaryKey]
        public long Id { get; set; }

        public bool ImageDownload { get; set; }

        public bool SoundDownload { get; set; }

        public bool VideoDownload { get; set; }
    }
}
