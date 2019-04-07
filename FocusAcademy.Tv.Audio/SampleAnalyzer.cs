using System;
using System.Collections.Generic;
using System.Linq;

namespace FocusAcademy.Tv.Audio
{
    public class SampleAnalyzer
    {
        private readonly List<float> _neg = new List<float>();
        private readonly List<float> _pos = new List<float>();

        public float Min { get; private set; }

        public float Max { get; private set; }

        public float AvgMin => _neg.Any() ? _neg.Average() : 0;

        public float AvgMax => _pos.Any() ? _pos.Average() : 0;

        public int Counter { get; private set; }

        public void AddSample(float sample)
        {
            Min = Math.Min(Min, sample);
            Max = Math.Max(Max, sample);

            if (sample < 0)
                _neg.Add(sample);
            else if (sample > 0)
                _pos.Add(sample);

            Counter++;
        }

        public void Reset()
        {
            Counter = 0;
            Min = 0;
            Max = 0;
            _neg.Clear();
            _pos.Clear();
        }
    }
}