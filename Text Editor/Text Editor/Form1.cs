/***************************************************************
 *if you have any questions, here's my instagram: @guven.daghan*
 ***************************************************************/
using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Text_Editor
{
    public partial class Form1 : Form
    {
        private const float RichTextBoxHeightPercentage = 0.941f; // Adjust as needed

        public Form1()
        {
            InitializeComponent();
            HandleCommandLineArguments(Environment.GetCommandLineArgs());
        }
        private void HandleCommandLineArguments(string[] args)
        {
            if (args.Length > 1)
            {
                string filePath = args[1]; // The first argument (index 0) is the path to the executable itself
                OpenFile(filePath);
            }
        }

        private void OpenFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                richTextBox1.Text = File.ReadAllText(filePath);
                file.Path = filePath;
                file.Saved = true;
                SetText();
            }
            else
            {
                MessageBox.Show("File does not exist: " + filePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        readonly string path1= Application.StartupPath + "\\preferences.txt";
        readonly string path2 = Application.StartupPath + "\\themes.txt";
        private void Form1_Load(object sender, EventArgs e)
        {
            // Set the sizes of controls
            richTextBox1.Height = (int)(ClientSize.Height * RichTextBoxHeightPercentage);
            preferencesToolStripMenuItem.Font =
                fileToolStripMenuItem.Font =
                    new Font(FontFamily.GenericSerif, Convert.ToInt32(Size.Height / 40));

            // Create preferences.txt
            if (!File.Exists(path1))
            {
                File.WriteAllText(path1, "10\n59\nNormal 0,0,0 255,255,255\n1");
                File.SetAttributes(path1, File.GetAttributes(path1) | FileAttributes.Hidden);
            }
            if (!File.Exists(path2))
            {
                File.WriteAllText(path2, "Normal 0,0,0 255,255,255\nSerenity-Whisper 200,200,240 51,51,51\nCandy-World 235,52,177 52,235,235" +
                    "\nArctic-Aurora 0,51,102 102,204,204");
                File.SetAttributes(path2, File.GetAttributes(path2) | FileAttributes.Hidden);
            }
            List<string> daList = new List<string>(File.ReadAllLines(path1));
            List<string> theme = new List<string>(daList[2].Split());
            Preferences.Fontsize = Convert.ToInt32(daList[0]);
            Preferences.FontName = Convert.ToInt32(daList[1]);
            Preferences.theme_Name = theme[0];
            Preferences.theme_Forecolor = theme[1];
            Preferences.theme_Backcolor= theme[2];
            Preferences.Bold = daList[3] =="1";

            Forecolor = new List<string>(Preferences.theme_Forecolor.Split(','));
            Backcolor = new List<string>(Preferences.theme_Backcolor.Split(','));

            SetRichTextBox();

            SetText();
        }
        List<string> Forecolor;
        List<string> Backcolor;
        private void SetRichTextBox()
        {
            richTextBox1.Font = new Font(FontFamily.Families[Preferences.FontName], Preferences.Fontsize, Preferences.Bold ? FontStyle.Bold : FontStyle.Regular);
            richTextBox1.BackColor = Color.FromArgb(Convert.ToInt32(Backcolor[0]), Convert.ToInt32(Backcolor[1]), Convert.ToInt32(Backcolor[2]));
            richTextBox1.ForeColor = Color.FromArgb(Convert.ToInt32(Forecolor[0]), Convert.ToInt32(Forecolor[1]), Convert.ToInt32(Forecolor[2]));
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            richTextBox1.Height = (int)(ClientSize.Height * RichTextBoxHeightPercentage);
            preferencesToolStripMenuItem.Font = 
                fileToolStripMenuItem.Font = 
                    new Font(FontFamily.GenericSerif, Convert.ToInt32(Size.Height / 40));
        }
        Form2 form2 = new Form2();
        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // if Form2 is already opened, make user unable to open another one.
            start:
            try
            {
                form2.Show();
            }
            catch
            {
                form2 = new Form2();
                goto start;
            }
            
        }
        _File file = new _File();
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (file.Path)
            {
                case "Unknown":
                    openFileToolStripMenuItem_Click(1,e);
                    break;
                default:
                    File.WriteAllText(file.Path, richTextBox1.Text);
                    file.Saved = true;
                    SetText();
                    break;
            }
            
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            file.Path = openFileDialog1.FileName;

            richTextBox1.Text=File.ReadAllText(file.Path);
            SetText();
        }
        private void SetText()=>Text = "Text Editor - " + file.Path.Split('\\').Reverse().First()+" - "+(file.Saved?"Saved":"Unsaved");

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            file.Saved = false;
            SetText();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                saveToolStripMenuItem_Click(1, e);
            else if (e.Control && e.Shift && e.KeyCode == Keys.S)
                saveAllToolStripMenuItem_Click(1, e);
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(1, e);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of the SaveFileDialog class
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Set properties of the SaveFileDialog
            saveFileDialog.Title = "Save As";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                File.WriteAllText(filePath, richTextBox1.Text);
                file.Saved = false;
                file.Path = filePath;
                SetText();
                try
                {
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while saving the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
