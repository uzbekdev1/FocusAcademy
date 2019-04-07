using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FocusAcademy.Tv.App.Properties;
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

        private void SearchForm_Load(object sender, EventArgs e)
        {
            var files = Directory.GetFiles(Settings.Default.SourceFolder, "*.mp3");

            for (var i = 0; i < files.Length; i++)
            {
                var file = new FileInfo(files[i]);

                listView1.Items.Add(new ListViewItem(new[]
                {
                   $"{i+1}",
                    file.Name,
                    file.DirectoryName,
                    file.CreationTime.ToShortDateString(),
                    $"{(double)file.Length/ (1024 * 1024):F} mb"
                }));
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var row = listView1.SelectedItems[0].SubItems.Cast<ListViewItem.ListViewSubItem>().ToArray();
            var fileName = Path.Combine(row[2].Text, row[1].Text);

            Process.Start(fileName);
        }
    }
}
