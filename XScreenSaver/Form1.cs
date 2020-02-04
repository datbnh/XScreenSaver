using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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


            timer1.Enabled = true;
            timer1.Interval = 1000;

            timer2.Enabled = true;

            odd = true;
            panel1.Size = new Size(Bounds.Width, Bounds.Height);
            g = panel1.CreateGraphics();
            g.Clear(Color.Green);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            font = new Font(new FontFamily("Segoe UI Light"), size, FontStyle.Regular, GraphicsUnit.Pixel);
            dateFont = new Font(new FontFamily("Segoe UI Light"), dateSize, FontStyle.Regular, GraphicsUnit.Pixel);

            InitFigureSize();
            InitDelimiterSize();
            InitFigureOffsets();
            UpdateDate();

            clockWidth = figureOffsets.Last() + figureSize.Width;
            clockHeight = figureSize.Height;
        }

        public Point InitialPosition { get; }
        public static readonly int X_THRESHOLD = 20;
        public static readonly int Y_THRESHOLD = 20;
        public Graphics g;
        public bool odd;

        public Font font;
        public Font dateFont;

        public int size = 192;

        public int dateSize = 48;

        private Size figureSize;
        private Size delimSize;

        private bool isDisplaySecond = false;
        private string timeFormatter => isDisplaySecond ? "HH:mm:ss" : "HH:mm";
        private string dateFormatter = "dddd, d MMMM, yyyy";

        private int[] figureOffsets;

        private int dateWidth;
        private int dateHeight;
        private int clockWidth;
        private int clockHeight;

        private int totalWidth { get { return Math.Max(dateWidth, clockWidth); } }
        private int totalHeight { get { return dateHeight + clockHeight; } }

        private Point clockTopLeftPoint { get { return new Point((Bounds.Width - clockWidth) / 2, (Bounds.Height - totalHeight) / 2); } }

        private Point dateTopLeftPoint { get { return new Point((Bounds.Width - dateWidth) / 2, clockTopLeftPoint.Y + clockHeight); } }


        private int maxOffsetX { get { return (Bounds.Width - totalWidth) / 2; } }
        private int maxOffsetY { get { return (Bounds.Height - totalHeight) / 2; } }

        private int velocityX = 1;
        private int velocityY = 1;

        private int currentOffsetX = 0;
        private int currentOffsetY = 0;

        private Brush backgroundBrush = Brushes.Black;


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            MousedMoved();
        }

        private void MousedMoved()
        {
            if ((Math.Abs(Cursor.Position.X - InitialPosition.X) > X_THRESHOLD) ||
                    (Math.Abs(Cursor.Position.Y - InitialPosition.Y) > Y_THRESHOLD))
                Close();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            DrawTime(false);
        }

        private void DrawTime(bool updateAllDigits)
        {
            var currentTime = DateTime.Now;

            UpdateSecond(currentTime, updateAllDigits);
            if (currentTime.Second == 0 || updateAllDigits)
            {
                UpdateMinute(currentTime, updateAllDigits);
                if (currentTime.Minute == 0 || updateAllDigits)
                {
                    UpdateHour(currentTime, updateAllDigits);
                    if (currentTime.Hour == 0)
                        UpdateDate();
                }
            }


            g.FillRectangle(backgroundBrush, clockTopLeftPoint.X + figureOffsets[2], clockTopLeftPoint.Y, delimSize.Width, delimSize.Height);
            DrawTextNoPaddingAtCentre(g, odd ? "." : "", font, new Rectangle(new Point(clockTopLeftPoint.X + figureOffsets[2], clockTopLeftPoint.Y), delimSize));

            if (isDisplaySecond)
            {
                g.FillRectangle(backgroundBrush, clockTopLeftPoint.X + figureOffsets[5], clockTopLeftPoint.Y, delimSize.Width, delimSize.Height);
                DrawTextNoPaddingAtCentre(g, odd ? "." : "", font, new Rectangle(new Point(clockTopLeftPoint.X + figureOffsets[5], clockTopLeftPoint.Y), delimSize));
            }
            odd = !odd;
        }

        private void UpdateSecond(DateTime time, bool updateAllDigits)
        {
            if (!isDisplaySecond)
                return;

            Point location = clockTopLeftPoint;
            if (time.Second % 10 == 0 || updateAllDigits)
            {
                location.X = clockTopLeftPoint.X + figureOffsets[6];
                ClearFigure(location);
                DrawTextNoPaddingAtCentre(g, time.Second / 10 + "", font,
                    new Rectangle(location, figureSize));
            }

            location.X = clockTopLeftPoint.X + figureOffsets[7];
            ClearFigure(location);
            DrawTextNoPaddingAtCentre(g, time.Second % 10 + "", font,
                new Rectangle(location, figureSize));
        }

        private void ClearFigure(Point location)
        {
            g.FillRectangle(backgroundBrush, location.X, location.Y, figureSize.Width, figureSize.Height);
        }

        private void UpdateHour(DateTime time, bool updateAllDigits)
        {
            Point location = clockTopLeftPoint;
            if (time.Hour % 10 == 0 || updateAllDigits)
            {
                location.X = clockTopLeftPoint.X + figureOffsets[0];
                ClearFigure(location);
                DrawTextNoPaddingAtCentre(g, time.Hour / 10 + "", font,
                    new Rectangle(location, figureSize));
            }

            location.X = clockTopLeftPoint.X + figureOffsets[1];
            ClearFigure(location);
            DrawTextNoPaddingAtCentre(g, time.Hour % 10 + "", font,
                new Rectangle(location, figureSize));
        }

        private void UpdateMinute(DateTime time, bool updateAllDigits)
        {
            Point location = clockTopLeftPoint;
            if (time.Minute % 10 == 0 || updateAllDigits)
            {
                location.X = clockTopLeftPoint.X + figureOffsets[3];
                ClearFigure(location);
                DrawTextNoPaddingAtCentre(g, time.Minute / 10 + "", font,
                    new Rectangle(location, figureSize));
            }

            location.X = clockTopLeftPoint.X + figureOffsets[4];
            ClearFigure(location);
            DrawTextNoPaddingAtCentre(g, time.Minute % 10 + "", font,
                new Rectangle(location, figureSize));
        }

        private void InitFigureOffsets()
        {
            figureOffsets = new int[timeFormatter.Length];
            figureOffsets[0] = 0;
            for (int i = 1; i < timeFormatter.Length; i++)
            {
                if (timeFormatter[i - 1].Equals('H') ||
                    timeFormatter[i - 1].Equals('h') ||
                    timeFormatter[i - 1].Equals('m') ||
                    timeFormatter[i - 1].Equals('s'))
                    figureOffsets[i] = figureOffsets[i - 1] + figureSize.Width - 1;
                else
                    figureOffsets[i] = figureOffsets[i - 1] + delimSize.Width - 1;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Close();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            DrawTime(true);
            UpdateDate();
        }

        private void UpdateDate()
        {
            var size = TextRenderer.MeasureText(g, DateTime.Today.ToString(dateFormatter),
                                    dateFont, new Size(dateFont.Height, dateFont.Height),
                                    TextFormatFlags.Default);

            dateWidth = size.Width;
            dateHeight = size.Height;

            g.FillRectangle(backgroundBrush,
                            dateTopLeftPoint.X, dateTopLeftPoint.Y, dateWidth, dateHeight);

            //dateTopLeftPoint.X = (Bounds.Width - dateWidth) / 2;
            //dateTopLeftPoint.Y = clockTopLeftPoint.Y + figureSize.Height;

            TextRenderer.DrawText(g, DateTime.Today.ToString(dateFormatter), dateFont,
                      dateTopLeftPoint,
                      Color.White,
                      TextFormatFlags.Default);
        }

        private void InitFigureSize()
        {
            int maxW = 0;
            int maxH = 0;

            for (int i = 0; i <= 9; i++)
            {
                var size = GetTextSizeNoPadding(g, i.ToString("0"), font);
                maxW = Math.Max(maxW, size.Width);
                maxH = Math.Max(maxH, size.Height);
            }

            figureSize = new Size(maxW, maxH);
        }

        private void InitDelimiterSize()
        {
            string[] delimiters = { "." };

            int maxW = 0;
            int maxH = 0;

            for (int i = 0; i < delimiters.Length; i++)
            {
                var size = GetTextSizeNoPadding(g, delimiters[i], font);
                maxW = Math.Max(maxW, size.Width);
                maxH = Math.Max(maxH, size.Height);
            }

            delimSize = new Size(maxW, maxH);
        }

        private void DrawTextNoPaddingAtCentre(Graphics graphics, string text, Font font, Rectangle rectangle)
        {
            var size = GetTextSizeNoPadding(graphics, text, font);

            var X = (int)((rectangle.Width - size.Width) / 2.0);
            var Y = (int)((rectangle.Height - size.Height) / 2.0);

            var startingPoint = new Point(rectangle.X + X, rectangle.Y + Y);

            DrawTextNoPadding(graphics, text, font, startingPoint);

        }

        private Size GetTextSizeNoPadding(Graphics graphics, string text, Font font)
        {
            return TextRenderer.MeasureText(graphics,
                                    text,
                                    font,
                                    new Size(font.Height, font.Height),
                                    TextFormatFlags.NoPadding);
        }
        private void DrawTextNoPadding(Graphics graphics, string text, Font font, Point location)
        {
            //Size size = GetTextSizeNoPadding(graphics, text, font);
            //graphics.FillRectangle(Brushes.Black, new Rectangle(location, size));
            //graphics.DrawRectangle(Pens.Red, new Rectangle(location, size));
            TextRenderer.DrawText(graphics, text, font, location, Color.White, TextFormatFlags.NoPadding);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (currentOffsetX < -maxOffsetX && velocityX < 0)
                velocityX = -velocityX;
            if (currentOffsetX > maxOffsetX && velocityX > 0)
                velocityX = -velocityX;

            if (currentOffsetY < -maxOffsetY && velocityY < 0)
                velocityY = -velocityY;
            if (currentOffsetY > maxOffsetY && velocityY > 0)
                velocityY = -velocityY;

            currentOffsetX += velocityX;
            currentOffsetY += velocityY;

            panel1.Location = new Point(currentOffsetX, currentOffsetY);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            MousedMoved();
        }
    }
}
