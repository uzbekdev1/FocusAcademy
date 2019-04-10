using System.Windows.Forms;
using FocusAcademy.Tv.App.Properties;

namespace FocusAcademy.Tv.App.Forms
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog()!=DialogResult.OK)
                return;

            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Settings.Default.SourceFolder = textBox1.Text;
            Settings.Default.Save();

            Close();
        }

        private void OptionsForm_Load(object sender, System.EventArgs e)
        {
            textBox1.Text = Settings.Default.SourceFolder;
        }
    }
}
