using CSCore;

namespace FocusAcademy.Tv.Audio
{
    public class InterruptDisposeChainSource : IWaveAggregator
    {
        public InterruptDisposeChainSource(IWaveSource audioSource)
        {
            BaseSource = audioSource;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return BaseSource.Read(buffer, 0, count);
        }

        public void Dispose()
        {
            //do nothing
        }

        public bool CanSeek => BaseSource.CanSeek;

        public WaveFormat WaveFormat => BaseSource.WaveFormat;

        public long Position
        {
            get { return BaseSource.Position; }
            set { BaseSource.Position = value; }
        }

        public long Length => BaseSource.Length;

        public IWaveSource BaseSource { get; }
    }
}