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

namespace TheProject
{
    public partial class ChooseAlpha_Beta : Form
    {
        public Form1 ParentForm1 { get; set; }
        public ChooseAlpha_Beta(Form1 parent)
        {
            InitializeComponent();
            ParentForm1 = parent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    textBox1.Text = text.Remove(text.IndexOf("\r\nsecond"));

                    textBox2.Text = text.Remove(0, text.IndexOf("\r\nsecond\r\n") + 10);
                }
                catch (IOException)
                {
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            try
            {
                string name = saveFileDialog1.FileName;
                string content = textBox1.Text;
                content += "\r\nsecond\r\n";
                content += textBox2.Text;

                File.WriteAllText(name, content);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطایی پیش آمده است");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] lines = textBox1.Text.Replace("\r", "").Split('\n');
            for (int i = 0; i < 5; i++)
            {
                string[] values = lines[i].Split(' ');
                for (int j = 0; j < 5; j++)
                {
                    string[] alpha_beta = values[j].Split(',');
                    ParentForm1.alphas[i, j] = Double.Parse(alpha_beta[0]);
                    ParentForm1.betas[i, j] = Double.Parse(alpha_beta[1]);
                }
            }

            string[] bArray = textBox2.Text.Split(' ');
            for (int i = 0; i < 5; i++)
            {
                string[] values = bArray[0].Split(',');
                ParentForm1.B[i,0] = Double.Parse(values[0]);
                ParentForm1.B[i, 1] = Double.Parse(values[1]);
            }

            this.Close();
        }
    }
}
