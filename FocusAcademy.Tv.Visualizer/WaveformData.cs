using System;
using System.Linq;
using System.Threading.Tasks;
using CSCore;
using FocusAcademy.Tv.Audio;

namespace FocusAcademy.Tv.Waveform
{
    public static partial class WaveformData
    {
        private const int NumberOfPoints = 2000;
        public static long Length;

        public static async Task<float[][]> GetData(IWaveSource waveSource)
        { 
            return await Task.Run(() =>
            {
                var sampleSource = new InterruptDisposeChainSource(waveSource).ToSampleSource();

                var channels = sampleSource.WaveFormat.Channels;
                var blockSize = (int) (sampleSource.Length / channels / NumberOfPoints);
                var waveformDataChannels = new SampleDataChannel[channels];
                for (var i = 0; i < channels; i++)
                {
                    waveformDataChannels[i] = new SampleDataChannel(blockSize);
                }

                var buffer = new float[sampleSource.WaveFormat.BlockAlign * 5];
                var sampleCount = 0;

                var flag = true;
                while (flag)
                {
                    var samplesToRead = buffer.Length;
                    var read = sampleSource.Read(buffer, 0, samplesToRead);
                    for (var i = 0; i < read; i += channels)
                    {
                        for (var n = 0; n < channels; n++)
                        {
                            waveformDataChannels[n].AddSample(buffer[i + n]);
                            sampleCount++;
                        }
                    }

                    if (read == 0)
                        flag = false;
                }

                foreach (var waveformDataChannel in waveformDataChannels)
                {
                    waveformDataChannel.Finish();
                }

                Length = sampleCount;

                return waveformDataChannels.Select(x => x.GetData()).ToArray();
            });
        }
    }
}