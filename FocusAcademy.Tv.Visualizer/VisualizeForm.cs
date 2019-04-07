using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CSCore;
using CSCore.Codecs;
using CSCore.DSP;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCore.Streams.Effects;
using FocusAcademy.Tv.Visualizer.Visualization;

namespace FocusAcademy.Tv.Visualizer
{
    public partial class VisualizeForm : Form
    {
        private readonly Bitmap _bitmap = new Bitmap(2000, 600);
        private LineSpectrum _lineSpectrum;
        private PitchShifter _pitchShifter;
        private ISoundOut _soundOut;
        private IWaveSource _source;
        private VoicePrint3DSpectrum _voicePrint3DSpectrum;
        private int _xpos;

        public VisualizeForm()
        {
            InitializeComponent();
        }

        public VisualizeForm(string title) : this()
        {
            Text = title;
        }

        public void OpenFile(string fileName)
        {
            //open the selected file
            ISampleSource source = CodecFactory.Instance.GetCodec(fileName)
                .ToSampleSource()
                .AppendSource(x => new PitchShifter(x), out _pitchShifter);

            SetupSampleSource(source);

            //play the audio
            if (WasapiOut.IsSupportedOnCurrentPlatform)
                _soundOut = new WasapiOut();
            else
                _soundOut = new DirectSoundOut();

            _soundOut.Initialize(_source);
            _soundOut.Play();

            timer1.Start();

            propertyGridTop.SelectedObject = _lineSpectrum;
            propertyGridBottom.SelectedObject = _voicePrint3DSpectrum;
        }

        /// <summary>
        /// </summary>
        /// <param name="aSampleSource"></param>
        private void SetupSampleSource(ISampleSource aSampleSource)
        {
            const FftSize fftSize = FftSize.Fft4096;
            //create a spectrum provider which provides fft data based on some input
            var spectrumProvider = new BasicSpectrumProvider(aSampleSource.WaveFormat.Channels,
                aSampleSource.WaveFormat.SampleRate, fftSize);

            //linespectrum and voiceprint3dspectrum used for rendering some fft data
            //in oder to get some fft data, set the previously created spectrumprovider 
            _lineSpectrum = new LineSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = true,
                BarCount = 50,
                BarSpacing = 2,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Sqrt
            };
            _voicePrint3DSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = true,
                PointCount = 200,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Sqrt
            };

            //the SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(aSampleSource);
            //pass the intercepted samples as input data to the spectrumprovider (which will calculate a fft based on them)
            notificationSource.SingleBlockRead += (s, a) => spectrumProvider.Add(a.Left, a.Right);

            _source = notificationSource.ToWaveSource(16);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Stop();
        }

        private void Stop()
        {
            timer1.Stop();

            if (_soundOut != null)
            {
                _soundOut.Stop();
                _soundOut.Dispose();
                _soundOut = null;
            }

            if (_source != null)
            {
                _source.Dispose();
                _source = null;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //render the spectrum
            GenerateLineSpectrum();
            GenerateVoice3DPrintSpectrum();
        }

        private void GenerateLineSpectrum()
        {
            var image = pictureBoxTop.Image;
            var newImage =
                _lineSpectrum.CreateSpectrumLine(pictureBoxTop.Size, Color.Green, Color.Red, Color.Black, true);
            if (newImage != null)
            {
                pictureBoxTop.Image = newImage;
                if (image != null)
                    image.Dispose();
            }
        }

        private void GenerateVoice3DPrintSpectrum()
        {
            using (var g = Graphics.FromImage(_bitmap))
            {
                pictureBoxBottom.Image = null;
                if (_voicePrint3DSpectrum.CreateVoicePrint3D(g, new RectangleF(0, 0, _bitmap.Width, _bitmap.Height),
                    _xpos, Color.Black, 3))
                {
                    _xpos += 3;
                    if (_xpos >= _bitmap.Width)
                        _xpos = 0;
                }

                pictureBoxBottom.Image = _bitmap;
            }
        }

        private void pitchShiftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var value = (int) (_pitchShifter != null
                ? Math.Log10(_pitchShifter.PitchShiftFactor) / Math.Log10(2) * 120
                : 0);
            var form = new Form
            {
                Width = 300,
                Height = 60,
                Text = value.ToString(),
                MaximizeBox = false,
                MinimizeBox = false,
                SizeGripStyle = SizeGripStyle.Hide,
                ShowIcon = false,
                FormBorderStyle = FormBorderStyle.FixedToolWindow
            };
            var trackBar = new TrackBar
            {
                TickStyle = TickStyle.None,
                Minimum = -100,
                Maximum = 100,
                Value = value,
                Dock = DockStyle.Fill
            };
            trackBar.ValueChanged += (s, args) =>
            {
                if (_pitchShifter != null)
                {
                    _pitchShifter.PitchShiftFactor = (float) Math.Pow(2, trackBar.Value / 120.0);

                    form.Text = trackBar.Value.ToString();
                }
            };
            form.Controls.Add(trackBar);

            form.ShowDialog();

            form.Dispose();
        }
    }
}