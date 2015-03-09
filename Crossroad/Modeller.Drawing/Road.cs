using System;
using System.Drawing;

namespace Modeller.Drawing
{
    public class Road : IRoadElement
    {
        public Road(int row, int column, RoadDirection roadDirection)
        {
            Row = row;
            Column = column;
            RoadDirection = roadDirection;
        }

        public RoadDirection RoadDirection { get; set; }

        public void Draw(int x, int y, Graphics graphics)
        {
            switch (RoadDirection)
            {
                case RoadDirection.Horizontal:
                    graphics.DrawLine(Pens.Black, x + 10, y + 25, x + 40, y + 25);
                    break;
                case RoadDirection.Vertical:
                    graphics.DrawLine(Pens.Black, x + 25, y + 10, x + 25, y + 40);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public int Row { get; set; }
        public int Column { get; set; }
    }
}