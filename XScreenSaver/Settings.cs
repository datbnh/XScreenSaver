using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XScreenSaver
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = button3.Font;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                button3.Text = fontDialog1.Font.FontFamily.Name;
                var font = new Font(fontDialog1.Font.FontFamily.Name, button3.Font.Size, fontDialog1.Font.Style);
                button3.Font = font;
                              
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
