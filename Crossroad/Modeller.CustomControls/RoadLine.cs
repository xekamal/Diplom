using System.Drawing;

namespace Modeller.CustomControls
{
    public class RoadLine
    {
        public Pen Color { get; set; }

        protected bool Equals(RoadLine other)
        {
            return Equals(Color, other.Color);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RoadLine) obj);
        }

        public override int GetHashCode()
        {
            return (Color != null ? Color.GetHashCode() : 0);
        }
    }
}