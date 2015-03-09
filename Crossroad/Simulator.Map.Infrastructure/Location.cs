namespace Simulator.Map.Infrastructure
{
    public class Location : ILocation
    {
        public Location(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; set; }
        public int Column { get; set; }
    }
}