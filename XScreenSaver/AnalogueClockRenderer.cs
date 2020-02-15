using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XScreenSaver
{
    public static class AnalogueClockRenderer
    {
        public static void Render(Graphics g, AnalogueClock analogueClock, DateTime dateTime)
        {
            DrawClockFace(g, analogueClock);
            DrawClockHands(g, analogueClock, dateTime);
        }

        public static int GetClockWidth(AnalogueClock analogueClock)
        {
            return (int)(analogueClock.DiameterInPixel + 0.5f);
        }

        public static int GetClockHeight(AnalogueClock analogueClock)
        {
            return (int)(analogueClock.DiameterInPixel + 0.5f);
        }

        private static void DrawClockHands(Graphics g, AnalogueClock analogueClock, DateTime dateTime)
        {
            g.FillEllipse(Brushes.LightGray,
                            -analogueClock.CentreDiameter / 2, -analogueClock.CentreDiameter / 2,
                            analogueClock.CentreDiameter, analogueClock.CentreDiameter);


            g.DrawLine(new Pen(Brushes.LightYellow, analogueClock.HourHandThichness),
                analogueClock.GetHourHand(dateTime)[0], analogueClock.GetHourHand(dateTime)[1]);
            g.DrawLine(new Pen(Brushes.LightYellow, analogueClock.MinuteHandThickness),
                analogueClock.GetMinuteHand(dateTime)[0], analogueClock.GetMinuteHand(dateTime)[1]);

            if (analogueClock.ShowSecondHand)
            {
                g.DrawLine(new Pen(Brushes.LightSalmon, analogueClock.SecondHandThickness),
                    analogueClock.GetSecondHand(dateTime)[0], analogueClock.GetSecondHand(dateTime)[1]);
            }

            g.FillEllipse(Brushes.Black,
                -analogueClock.CentreDiameter / 8, -analogueClock.CentreDiameter / 8,
                analogueClock.CentreDiameter / 4, analogueClock.CentreDiameter / 4);
        }

        private static void DrawClockFace(Graphics g, AnalogueClock analogueClock)
        {
            g.FillEllipse(Brushes.Black,
                -analogueClock.DiameterInPixel / 2, -analogueClock.DiameterInPixel / 2,
                analogueClock.DiameterInPixel, analogueClock.DiameterInPixel);

            g.DrawEllipse(new Pen(Brushes.DarkGray, 1f),
                -analogueClock.DiameterInPixel / 2, -analogueClock.DiameterInPixel / 2,
                analogueClock.DiameterInPixel, analogueClock.DiameterInPixel);
        }
    }
}
