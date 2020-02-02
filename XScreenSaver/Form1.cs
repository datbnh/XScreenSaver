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
    public partial class Form1 : Form
    {
        public Form1(Screen screen)
        {
            InitializeComponent();
            Bounds = screen.Bounds;
            Cursor.Hide();
            InitialPosition = Cursor.Position;
            g = CreateGraphics();
            g.Clear(Color.Black);

            timer1.Enabled = true;
            timer1.Interval = 250;
            odd = true;
        }

        public Point InitialPosition { get; }
        public static readonly int X_THRESHOLD = 20;
        public static readonly int Y_THRESHOLD = 20;
        public Graphics g;
        public bool odd;


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((Math.Abs(Cursor.Position.X - InitialPosition.X) > X_THRESHOLD) ||
                    (Math.Abs(Cursor.Position.Y - InitialPosition.Y) > Y_THRESHOLD))
                Close();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle(5, 5, ClientSize.Width - 10, ClientSize.Height - 10);
            
            using (Font font = new Font("Segoe UI Light", 144, GraphicsUnit.Pixel))
            {
                using (StringFormat sf = new StringFormat())
                {
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    g.Clear(Color.Black);
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    if (odd)
                        g.DrawString(DateTime.Now.ToString("HH:mm"), font, Brushes.White, rect, sf);
                    else
                        g.DrawString(DateTime.Now.ToString("HH mm"), font, Brushes.White, rect, sf);

                    odd = !odd;
                }
            }
        }
    }
}
