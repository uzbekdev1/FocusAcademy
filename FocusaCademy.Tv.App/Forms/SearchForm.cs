using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FocusAcademy.Tv.App.Properties;
using FocusAcademy.Tv.Audio;
using FocusAcademy.Tv.Player;
using FocusAcademy.Tv.Visualizer;

namespace FocusAcademy.Tv.App.Forms
{
    public partial class SearchForm : Form
    {
        public SearchForm()
        {
            InitializeComponent();
        }

        private void fetchBtn_Click(object sender, EventArgs e)
        {
            var files = Directory.GetFiles(Settings.Default.SourceFolder, textBox1.TextLength == 0 ? "*.mp3" : $"*{textBox1.Text}*.mp3");

            if (files.Length == 0)
                return;

            fetchBtn.Enabled = false;
            fetchBtn.Cursor = Cursors.WaitCursor;

            listView1.Items.Clear();

            for (var i = 0; i < files.Length; i++)
            {
                var fileName = files[i];

                if (WaveformData.Analysis(fileName))
                {
                    var fileInfo = new FileInfo(fileName);

                    listView1.Items.Add(new ListViewItem(new[]
                    {
                        $"{i+1}",
                        fileInfo.Name,
                        fileInfo.DirectoryName,
                        fileInfo.CreationTime.ToShortDateString(),
                        $"{(double)fileInfo.Length/ (1024 * 1024):F} mb"
                    }));
                }

                var percent = (int)Math.Ceiling((double)(i + 1) * 100 / files.Length);

                toolStripProgressBar1.ToolTipText = $"{percent}%";
                toolStripProgressBar1.Value = percent;

                Application.DoEvents();

            }

            fetchBtn.Enabled = true;
            fetchBtn.Cursor = Cursors.Default;

            toolStripProgressBar1.ToolTipText = "0%";
            toolStripProgressBar1.Value = 0;
        }

        private void visualizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var row = listView1.SelectedItems[0].SubItems.Cast<ListViewItem.ListViewSubItem>().ToArray();
            var form = new VisualizeForm(row[1].Text);
            form.Show();

            var fileName = Path.Combine(row[2].Text, row[1].Text);
            form.OpenFile(fileName);

        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var row = listView1.SelectedItems[0].SubItems.Cast<ListViewItem.ListViewSubItem>().ToArray();
            var form = new PlayForm(row[1].Text);
            form.Show();

            var fileName = Path.Combine(row[2].Text, row[1].Text);
            form.OpenFile(fileName);

        }
    }
}
