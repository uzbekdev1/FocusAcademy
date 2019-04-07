using System;
using System.ComponentModel;
using System.Windows.Forms;
using CSCore.SoundOut;

namespace FocusAcademy.Tv.Player
{
    public partial class PlayForm : Form
    {
        private readonly MusicPlayer _musicPlayer = new MusicPlayer();
        private bool _stopSliderUpdate;

        public PlayForm()
        {
            InitializeComponent();
        }

        public PlayForm(string title) : this()
        {
            Text = title;

            components = new Container();
            components.Add(_musicPlayer);
            _musicPlayer.PlaybackStopped += (s, args) =>
            {
                //WasapiOut uses SynchronizationContext.Post to raise the event
                //There might be already a new WasapiOut-instance in the background when the async Post method brings the PlaybackStopped-Event to us.
                if (_musicPlayer.PlaybackState != PlaybackState.Stopped)
                    btnPlay.Enabled = btnStop.Enabled = btnPause.Enabled = false;
            };
        }

        public void OpenFile(string fileName)
        {
            _musicPlayer.Open(fileName);
            trackbarVolume.Value = _musicPlayer.Volume;

            btnPlay.Enabled = true;
            btnPause.Enabled = btnStop.Enabled = false;

            btnPlay_Click(null, null);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (_musicPlayer.PlaybackState != PlaybackState.Playing)
            {
                _musicPlayer.Play();
                btnPlay.Enabled = false;
                btnPause.Enabled = btnStop.Enabled = true;
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (_musicPlayer.PlaybackState == PlaybackState.Playing)
            {
                _musicPlayer.Pause();
                btnPause.Enabled = false;
                btnPlay.Enabled = btnStop.Enabled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_musicPlayer.PlaybackState != PlaybackState.Stopped)
            {
                _musicPlayer.Stop();

                btnPlay.Enabled = true;
                btnStop.Enabled = btnPause.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var position = _musicPlayer.Position;
            var length = _musicPlayer.Length;
            if (position > length)
                length = position;

            lblPosition.Text = string.Format(@"{0:mm\:ss} / {1:mm\:ss}", position, length);

            if (!_stopSliderUpdate &&
                length != TimeSpan.Zero && position != TimeSpan.Zero)
            {
                var perc = position.TotalMilliseconds / length.TotalMilliseconds * trackBar1.Maximum;
                trackBar1.Value = (int) perc;
            }
        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _stopSliderUpdate = true;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _stopSliderUpdate = false;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (_stopSliderUpdate)
            {
                var perc = trackBar1.Value / (double) trackBar1.Maximum;
                var position = TimeSpan.FromMilliseconds(_musicPlayer.Length.TotalMilliseconds * perc);
                _musicPlayer.Position = position;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void trackbarVolume_ValueChanged(object sender, EventArgs e)
        {
            _musicPlayer.Volume = trackbarVolume.Value;
        }
         
    }
}