using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using System.IO;

namespace NITW_Dialogue_Tool.Resources
{
    public partial class frmEditor : Form
    {
        public string yarnFile = "";
        public string yarnPath = "";

        ScintillaNET.Scintilla TextArea;

        public frmEditor()
        {
            InitializeComponent();
        }

        private void frmEditor_Load(object sender, EventArgs e)
        {
            this.Text = "Text Editor - " + yarnFile;
            TextArea = new ScintillaNET.Scintilla();
            pnlEditor.Controls.Add(TextArea);
            InitTextarea();
            LoadDataFromFile(yarnPath);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.Text = "Text Editor - " + yarnFile + "*";
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(yarnPath, TextArea.Text);
            this.Text = "Text Editor - " + yarnFile;
        }

        private void InitTextarea()
        {
            TextArea.Top = 26;
            TextArea.Left = 0;
            TextArea.Width = 810;
            TextArea.Height = 449;
            TextArea.Dock = DockStyle.Fill;
            TextArea.WrapMode = WrapMode.None;
            TextArea.IndentationGuides = IndentView.LookBoth;

            TextArea.CaretForeColor = hexColor("#f8f8f2");
            TextArea.SetSelectionBackColor(true, hexColor("#f8f8f2"));
            TextArea.StyleResetDefault();

            TextArea.Styles[Style.Default].Font = "Consolas";
            TextArea.Styles[Style.Default].Size = 10;
            TextArea.Styles[Style.Default].BackColor = hexColor("#272822");
            TextArea.Styles[Style.Default].ForeColor = hexColor("#f8f8f2");
            TextArea.StyleClearAll();

            TextArea.Styles[Style.LineNumber].BackColor = hexColor("#272822");
            TextArea.Styles[Style.LineNumber].ForeColor = hexColor("#909080");
            TextArea.Styles[Style.IndentGuide].ForeColor = hexColor("#f8f8f2");
            TextArea.Styles[Style.IndentGuide].BackColor = hexColor("#272822");

            var nums = TextArea.Margins[0];
            TextArea.Margins[0].Type = MarginType.RightText;
            nums.Width = 40;
            nums.Type = MarginType.Number;
            nums.Mask = 0;
        }
                
        private void LoadDataFromFile(string path)
        {
            if (File.Exists(path))
            {
                TextArea.Text = File.ReadAllText(path);
            }
        }

        public static Color hexColor(string hex)
        {
            return ColorTranslator.FromHtml(hex);
        }
    }
}
