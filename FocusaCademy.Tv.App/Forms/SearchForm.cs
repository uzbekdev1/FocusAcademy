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
        private string[] _files;

        public SearchForm()
        {
            InitializeComponent();
        }

        private void fetchBtn_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 0)
            {
                MessageBox.Show("Enter mask", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            _files = Directory.GetFiles(Settings.Default.SourceFolder, $"*{textBox1.Text}*.mp3");

            if (_files.Length == 0)
            {
                MessageBox.Show("No found", "File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            fetchBtn.Enabled = false;
            btnCancel.Enabled = true;
            listView1.Items.Clear();

            backgroundWorker1.RunWorkerAsync();

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

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var flag = false;

            for (var i = 0; i < _files.Length; i++)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;

                    break;
                }

                var fileName = _files[i];

                if (WaveformData.Analysis(fileName))
                {
                    flag = true;

                    backgroundWorker1.ReportProgress(i, fileName);
                }
                else
                {
                    backgroundWorker1.ReportProgress(i);
                }
            }

            if (!flag)
            {
                throw new Exception("Mismatched files!");
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            var counter = e.ProgressPercentage + 1;
            var percent = counter * 100 / _files.Length;
            var fileName = (string)e.UserState;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var fileInfo = new FileInfo(fileName);

                listView1.Items.Add(new ListViewItem(new[]
                {
                    $"{counter}",
                    fileInfo.Name,
                    fileInfo.DirectoryName,
                    fileInfo.CreationTime.ToShortDateString(),
                    $"{(double)fileInfo.Length/ (1024 * 1024):F} mb"
                }));

            }

            toolStripProgressBar1.ToolTipText = $"{percent}%";
            toolStripProgressBar1.Value = percent;

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Cancelled fetching", "Cancel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            fetchBtn.Enabled = true;
            btnCancel.Enabled = false;

            toolStripProgressBar1.ToolTipText = "0%";
            toolStripProgressBar1.Value = 0;
            textBox1.Clear();
            _files = new string[0];
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }
    }
}
