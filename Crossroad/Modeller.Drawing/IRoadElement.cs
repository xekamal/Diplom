namespace Modeller.Drawing
{
    public interface IRoadElement : IDrawable
    {
         int Row { get; set; }
         int Column { get; set; }
    }
}