using System;
using System.Collections.Generic;
using System.Linq;

namespace FocusAcademy.Tv.Audio
{
    public class WaveformChannel
    {
        private readonly int _blockSize;
        private readonly List<float> _maxData = new List<float>();
        private readonly List<float> _minData = new List<float>();
        private readonly WaveformAnalyzer _waveformAnalyzer = new WaveformAnalyzer();

        public WaveformChannel(int blockSize)
        {
            _blockSize = blockSize;
        }

        public void AddSample(float sample)
        {
            _waveformAnalyzer.AddSample(sample);

            if (_waveformAnalyzer.Counter >= _blockSize)
            {
                _minData.Add(_waveformAnalyzer.AvgMin);
                _maxData.Add(_waveformAnalyzer.AvgMax);

                _waveformAnalyzer.Reset();
            }
        }

        public void Finish()
        {
            _minData.Add(_waveformAnalyzer.AvgMin);
            _minData.Add(_waveformAnalyzer.AvgMax);

            _waveformAnalyzer.Reset();
        }

        public float[] GetData()
        {
            _maxData.AddRange(_minData);
            var data = _maxData.ToArray();

            var z = 1 / data.Average(x => Math.Abs(x));

            z /= 2;

            for (var i = 0; i < data.Length; i++)
            {
                data[i] *= z;
                data[i] = Math.Min(1.5f, Math.Max(-1.5f, data[i]));
            }

            return data;
        }
    }
}