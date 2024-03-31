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

namespace Text_Editor
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        string path = Application.StartupPath + "\\preferences.txt";
        private void Form2_Load(object sender, EventArgs e)
        {
            Forecolor = new List<string>(Preferences.theme_Forecolor.Split(','));
            Backcolor = new List<string>(Preferences.theme_Backcolor.Split(','));

            List<string> fonts = new List<string>();

            foreach (FontFamily font in FontFamily.Families)
            {
                fonts.Add(font.Name);
            }
            foreach (string item in fonts)
            {
                comboBox1.Items.Add(item);
            }
            comboBox1.SelectedIndex = Preferences.FontName;
            trackBar1.Value=Preferences.Fontsize-8;
            checkBox1.Checked = Preferences.Bold;
            richTextBox1.BackColor = Color.FromArgb(Convert.ToInt32(Backcolor[0]), Convert.ToInt32(Backcolor[1]), Convert.ToInt32(Backcolor[2]));
            richTextBox1.ForeColor = Color.FromArgb(Convert.ToInt32(Forecolor[0]), Convert.ToInt32(Forecolor[1]), Convert.ToInt32(Forecolor[2]));
            SetRichTextBox();
            richTextBox2.Font = new Font(FontFamily.Families[comboBox1.SelectedIndex], trackBar1.Value + 8, checkBox1.Checked ? FontStyle.Bold : FontStyle.Regular);
            richTextBox2.BackColor = Color.FromArgb(Convert.ToInt32(Backcolor[0]), Convert.ToInt32(Backcolor[1]), Convert.ToInt32(Backcolor[2]));
            richTextBox2.ForeColor = Color.FromArgb(Convert.ToInt32(Forecolor[0]), Convert.ToInt32(Forecolor[1]), Convert.ToInt32(Forecolor[2]));
            // Did you know that Meg from Family Guy's full name is Megatron Griffin?
            themes = new List<string>(File.ReadAllLines(Application.StartupPath + "\\themes.txt"));
            foreach (string item in themes) comboBox2.Items.Add(item.Split()[0].Replace('-',' '));
            comboBox2.SelectedIndex = comboBox2.FindString(Preferences.theme_Name.Replace('-',' '));
        }

        List<string> Forecolor;
        List<string> Backcolor;
        List<string> themes;
        private void SetRichTextBox()=>richTextBox1.Font = new Font(FontFamily.Families[comboBox1.SelectedIndex], trackBar1.Value+8, checkBox1.Checked ? FontStyle.Bold : FontStyle.Regular);


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRichTextBox();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SetRichTextBox();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetRichTextBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.Delete(path);
            File.WriteAllText(path, (trackBar1.Value + 8) + "\n" + comboBox1.SelectedIndex + "\n" + comboBox2.SelectedItem.ToString().Replace(' ', '-') + " " + Forecolor[0] + "," + Forecolor[1] + "," + Forecolor[2] + " " + Backcolor[0] + "," + Backcolor[1] + "," + Backcolor[2] + "\n" + (checkBox1.Checked ? "1" : "0"));
            File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
            Application.Restart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.Delete(path);
            File.WriteAllText(path, (trackBar1.Value + 8) + "\n" + comboBox1.SelectedIndex + "\n" + comboBox2.SelectedItem.ToString().Replace(' ','-') + " " + Forecolor[0] + "," + Forecolor[1] + "," + Forecolor[2] + " " + Backcolor[0] + "," + Backcolor[1] + "," + Backcolor[2] + "\n" + (checkBox1.Checked ? "1" : "0"));
            File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
            Application.Restart();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> list;
            foreach (string item in themes)
            {
                list = new List<string>(item.Split());
                if ((string)comboBox2.SelectedItem == list[0].Replace('-',' '))
                {
                    Forecolor = new List<string>(list[1].Split(','));
                    Backcolor = new List<string>(list[2].Split(','));
                }
            }
            richTextBox2.BackColor = Color.FromArgb(Convert.ToInt32(Backcolor[0]), Convert.ToInt32(Backcolor[1]), Convert.ToInt32(Backcolor[2]));
            richTextBox2.ForeColor = Color.FromArgb(Convert.ToInt32(Forecolor[0]), Convert.ToInt32(Forecolor[1]), Convert.ToInt32(Forecolor[2]));
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            richTextBox1.Font = new Font(FontFamily.Families[Preferences.FontName], Preferences.Fontsize, Preferences.Bold ? FontStyle.Bold : FontStyle.Regular);
            richTextBox1.BackColor = Color.FromArgb(Convert.ToInt32(Backcolor[0]), Convert.ToInt32(Backcolor[1]), Convert.ToInt32(Backcolor[2]));
            richTextBox1.ForeColor = Color.FromArgb(Convert.ToInt32(Forecolor[0]), Convert.ToInt32(Forecolor[1]), Convert.ToInt32(Forecolor[2]));
        }
    }
}
