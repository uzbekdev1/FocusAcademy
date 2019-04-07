using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FocusAcademy.Tv.App.Forms;

namespace FocusAcademy.Tv.App
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog(this);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new OptionsForm
            {
                MdiParent = this
            };
            form.Show();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SearchForm()
            {
                MdiParent = this
            };
            form.Show();
        }
    }
}
