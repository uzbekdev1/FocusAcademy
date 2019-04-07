using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FocusAcademy.Tv.Player;
using FocusAcademy.Tv.Waveform;

namespace FocusAcademy.Tv.App
{
    public partial class SearchForm : Form
    {
        public SearchForm()
        {
            InitializeComponent();
        }

        private void fetchBtn_Click(object sender, EventArgs e)
        {

        }

        private async void visualizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new WaveWindow("file1.mp3");
            var fileName = Path.Combine(Environment.CurrentDirectory, "test", "file1.mp3");
            form.Show();

            await form.OpenFile(fileName);
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new PlayForm("file1.mp3");
            var fileName = Path.Combine(Environment.CurrentDirectory, "test", "file1.mp3");
            form.Show();

            form.OpenFile(fileName);
        }
    }
}
