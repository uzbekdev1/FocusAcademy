using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.Codecs;

namespace FocusAcademy.Tv.Audio
{
    public static class WaveformData
    {
        private const int NumberOfPoints = 2000;
        private const int LimitOfWaveBlockChannels = 60;

        public static bool Analysis(string file)
        {
            var waveSource = CodecFactory.Instance.GetCodec(file);
            var length = 0L;
            var percent = (float)LimitOfWaveBlockChannels / 100;
            var data = GetData(waveSource, ref length);

            //when mp3 if not wave source
            if (length == 0)
                return false;

            //validate tack only 60% bounce
            return data.Count(item1 => item1.Any(a => a >= percent)) > 0;
        }

        public static float[][] GetData(IWaveSource waveSource, ref long length)
        {
            if (waveSource == null)
                throw new ArgumentNullException("waveSource");

            var sampleSource = new DisposeChainSource(waveSource).ToSampleSource();
            var channels = sampleSource.WaveFormat.Channels;
            var blockSize = (int)(sampleSource.Length / channels / NumberOfPoints);
            var waveformDataChannels = new WaveformChannel[channels];

            for (var i = 0; i < channels; i++)
            {
                waveformDataChannels[i] = new WaveformChannel(blockSize);
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

            length = sampleCount;

            return waveformDataChannels.Select(x => x.GetData()).ToArray();
        }

    }
}
