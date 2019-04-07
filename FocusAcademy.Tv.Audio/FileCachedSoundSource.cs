using System.IO;
using CSCore;
using CSCore.Streams;

namespace FocusAcademy.Tv.Audio
{
    public class FileCachedSoundSource : CachedSoundSource
    {
        private string _filename;

        public FileCachedSoundSource(IWaveSource source) : base(source)
        {
        }

        protected override Stream CreateStream()
        {
            _filename = Path.GetTempFileName();

            return new FileStream(_filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose | FileOptions.Asynchronous);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (File.Exists(_filename))
            {
                File.Delete(_filename);
            }
        }
    }
}
