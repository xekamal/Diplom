using System;
using System.Drawing;

namespace Modeller.Drawing
{
    public class Turn : IRoadElement
    {
        public Turn(int row, int column, TurnType turnType)
        {
            Row = row;
            Column = column;
            TurnType = turnType;
        }

        public TurnType TurnType { get; set; }

        public void Draw(int x, int y, Graphics graphics)
        {
            switch (TurnType)
            {
                case TurnType.LeftToUp:
                    graphics.DrawLine(Pens.Black, x + 10, y + 25, x + 25, y + 25);
                    graphics.DrawLine(Pens.Black, x + 25, y + 25, x + 25, y + 10);
                    break;
                case TurnType.UpToRight:
                    graphics.DrawLine(Pens.Black, x + 25, y + 10, x + 25, y + 25);
                    graphics.DrawLine(Pens.Black, x + 25, y + 25, x + 40, y + 25);
                    break;
                case TurnType.RightToDown:
                    graphics.DrawLine(Pens.Black, x + 40, y + 25, x + 25, y + 25);
                    graphics.DrawLine(Pens.Black, x + 25, y + 25, x + 25, y + 40);
                    break;
                case TurnType.DownToLeft:
                    graphics.DrawLine(Pens.Black, x + 25, y + 40, x + 25, y + 25);
                    graphics.DrawLine(Pens.Black, x + 25, y + 25, x + 10, y + 25);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public int Row { get; set; }
        public int Column { get; set; }
    }
}