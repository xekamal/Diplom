using System.Drawing;

namespace Modeller.Drawing
{
    public class Crossroad : IRoadElement
    {
        public Crossroad(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public void Draw(int x, int y, Graphics graphics)
        {
            graphics.DrawLine(Pens.Black, x + 10, y + 25, x + 40, y + 25);
            graphics.DrawLine(Pens.Black, x + 25, y + 10, x + 25, y + 40);
        }

        public int Row { get; set; }
        public int Column { get; set; }
    }
}