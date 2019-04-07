using System;
using System.Collections.Generic;
using System.Linq;

namespace FocusAcademy.Tv.Audio
{
    public class SampleDataChannel
    {
        private readonly int _blockSize;
        private readonly List<float> _maxData = new List<float>();
        private readonly List<float> _minData = new List<float>();
        private readonly SampleAnalyzer _sampleAnalyzer = new SampleAnalyzer();

        public SampleDataChannel(int blockSize)
        {
            _blockSize = blockSize;
        }

        public void AddSample(float sample)
        {
            _sampleAnalyzer.AddSample(sample);

            if (_sampleAnalyzer.Counter >= _blockSize)
            {
                _minData.Add(_sampleAnalyzer.AvgMin);
                _maxData.Add(_sampleAnalyzer.AvgMax);

                _sampleAnalyzer.Reset();
            }
        }

        public void Finish()
        {
            _minData.Add(_sampleAnalyzer.AvgMin);
            _minData.Add(_sampleAnalyzer.AvgMax);

            _sampleAnalyzer.Reset();
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