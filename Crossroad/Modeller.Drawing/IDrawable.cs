using System.Drawing;

namespace Modeller.Drawing
{
    public interface IDrawable
    {
        /// <summary>
        /// Отрисовать фигуру.
        /// </summary>
        /// <param name="x">X координата верхнего левого угла.</param>
        /// <param name="y">Y координата верхнего левого угла.</param>
        /// <param name="graphics">Объект для отрисовки.</param>
        void Draw(int x, int y, Graphics graphics);
    }
}