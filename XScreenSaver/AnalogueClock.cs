using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XScreenSaver
{
    public class AnalogueClock
    {
        public AnalogueClock(float diameterInPixel)
        {
            DiameterInPixel = diameterInPixel;
            secondHandLength = 0.95f * diameterInPixel / 2;
            minuteHandLength = 0.9f * diameterInPixel / 2;
            hourHandLenght = 0.75f * diameterInPixel / 2;
        }

        public float CentreDiameter => 2 * HourHandThichness;

        public float DiameterInPixel { get; }
        private float secondHandLength;
        private float minuteHandLength;
        private float hourHandLenght;

        private float secondHandExtendedLengthRatio = 0.15f;
        public float SecondHandThickness = 0.2f;
        public float MinuteHandThickness = 2f;
        public float HourHandThichness = 3f;

        public bool ShowSecondHand = true;

        public PointF[] GetSecondHand(DateTime time)
        {
            var angle = (time.Second) / 60f * 2 * Math.PI - Math.PI / 2;
            var endPoint = new PointF((float)(secondHandLength * Math.Cos(angle)),
                (float)(secondHandLength * Math.Sin(angle)));
            var startPoint = new PointF(-endPoint.X * secondHandExtendedLengthRatio,
                -endPoint.Y * secondHandExtendedLengthRatio);


            return new PointF[] { startPoint, endPoint };
        }

        public PointF[] GetMinuteHand(DateTime time)
        {
            var angle = (time.Second / 60f + time.Minute) / 60f * 2 * Math.PI - Math.PI / 2;

            var startPoint = new PointF();
            var endPoint = new PointF((float)(minuteHandLength * Math.Cos(angle)),
               (float)(minuteHandLength * Math.Sin(angle)));

            return new PointF[] { startPoint, endPoint };
        }

        public PointF[] GetHourHand(DateTime time)
        {
            var angle = ((time.Second / 60f + time.Minute) / 60f + time.Hour) / 12f * 2 * Math.PI - Math.PI / 2;

            var startPoint = new PointF();
            var endPoint = new PointF((float)(hourHandLenght * Math.Cos(angle)),
                (float)(hourHandLenght * Math.Sin(angle)));

            return new PointF[] { startPoint, endPoint };
        }
    }
}
