using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CSCoreWaveform.Annotations;

namespace FocusAcademy.Tv.Waveform
{
    public class WaveformDataModel : INotifyPropertyChanged
    {
        private float[] _data;
        private double _positionInPerc;

        public IList<float> Data
        {
            get { return _data; }
            set
            {
                if (Equals(value, _data))
                    return;

                _data = value.ToArray();
                OnPropertyChanged();
            }
        }

        public double PositionInPerc
        {
            get { return _positionInPerc; }
            set
            {
                if (value.Equals(_positionInPerc))
                    return;
                _positionInPerc = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}