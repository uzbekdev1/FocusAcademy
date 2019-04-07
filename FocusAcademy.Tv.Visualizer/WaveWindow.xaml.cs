﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using CSCore;
using CSCore.Codecs;
using CSCore.MediaFoundation;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCoreWaveform.Annotations;
using FocusAcademy.Tv.Audio;
using Microsoft.Win32;

namespace FocusAcademy.Tv.Waveform
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WaveWindow : INotifyPropertyChanged
    {
        private readonly ISoundOut _soundOut;
        private ObservableCollection<WaveformDataModel> _channels = new ObservableCollection<WaveformDataModel>();
        private NotificationSource _notificationSource;
        private ISampleSource _sampleSource;

        public WaveWindow(string title)
        {
            InitializeComponent();

            Title = title;
            DataContext = this;
            _soundOut = new WasapiOut();
        }

        public ObservableCollection<WaveformDataModel> Channels
        {
            get { return _channels; }
            private set
            {
                if (Equals(value, _channels))
                    return;
                _channels = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task OpenFile(string fileName)
        { 
            var source = CodecFactory.Instance.GetCodec(fileName);

            //since the mediafoundationdecoder isn't really accurate what position and length concerns
            //read the whole file into a cache
            if (source is MediaFoundationDecoder)
            {
                if (source.Length < 10485760) //10MB
                {
                    source = new CachedSoundSource(source);
                }
                else
                {
                    var stopwatch = Stopwatch.StartNew();
                    source = new FileCachedSoundSource(source);
                    stopwatch.Stop();
                    Debug.WriteLine(stopwatch.Elapsed.ToString());
                }
            }

            source.Position = 0;
            //load the waveform
            await LoadWaveformsAsync(source);
            source.Position = 0;

            _sampleSource = source.ToSampleSource();
            _notificationSource = new NotificationSource(_sampleSource) { Interval = 100 };
            _notificationSource.BlockRead += (o, args) => { UpdatePosition(); };
            _soundOut.Initialize(_notificationSource.ToWaveSource());
            _soundOut.Play();
        }

        private async Task LoadWaveformsAsync(IWaveSource waveSource)
        {
            Channels = null;
            
            //read the specified waveSource into n arrays of samples where n is the number of channels of the waveSource
            var channelData = await WaveformData.GetData(waveSource);
            
            //by setting the Channels property, the Waveform Control automatically renders the waveform
            Channels = new ObservableCollection<WaveformDataModel>(channelData.Select(x => new WaveformDataModel { Data = x }));
        }

        private void UpdatePosition()
        { 
            Dispatcher.InvokeAsync(() =>
            { 
                var x = (float)_sampleSource.Position / WaveformData.Length;

                foreach (var waveformData in Channels)
                    waveformData.PositionInPerc = x;
            });
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (_soundOut != null)
                _soundOut.Dispose();
            if (_notificationSource != null)
                _notificationSource.Dispose();
        }

        private void Waveform_OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            if (_notificationSource != null)
            {
                var position = (long)(e.Percentage * WaveformData.Length);
                position -= position % _notificationSource.WaveFormat.BlockAlign;
                _notificationSource.Position = position;
            }
        }
    }
}