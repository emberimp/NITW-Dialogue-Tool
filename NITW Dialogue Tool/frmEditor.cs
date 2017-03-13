using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NITW_Dialogue_Tool.Resources
{
    public partial class frmEditor : Form
    {
        public string yarnFile = "";
        public string yarnPath = "";

        public frmEditor()
        {
            InitializeComponent();
        }

        private void frmEditor_Load(object sender, EventArgs e)
        {
            rtbEditor.Text = System.IO.File.ReadAllText(yarnPath);
            this.Text = "Text Editor - " + yarnFile;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.Text = "Text Editor - " + yarnFile + "*";
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(yarnPath, rtbEditor.Text);
            this.Text = "Text Editor - " + yarnFile;
        }
    }
}
