using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Modeller.CustomControls
{
    public partial class Road : ACrossroadControl
    {
        private const int NearSize = 4;
        private const int PinRadius = 4;
        private const int PaintMargin = 10;
        private const int LineThickness = 1;
        private const int LineMargin = 2;
        private bool _isDrawPin;
        private RoadType _type;

        public Road()
        {
            InitializeComponent();
        }

        public RoadType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                Invalidate();
            }
        }

        public event EventHandler PinClick;

        private bool IsNearTo(int val, int check, int eps)
        {
            if (Math.Abs(val - check) <= eps)
            {
                return true;
            }

            return false;
        }

        private bool IsNearToPin(MouseEventArgs e)
        {
            return IsNearTo(e.X, Width/2, NearSize) && IsNearTo(e.Y, Height/2, NearSize);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Graphics graphics = CreateGraphics())
            {
                switch (_type)
                {
                    case RoadType.Vertical:
                        DrawVerticalLines(graphics);
                        break;
                    case RoadType.Horizontal:
                        DrawHorizontalLines(graphics);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (_isDrawPin)
                {
                    graphics.FillEllipse(Brushes.Blue, Width/2 - PinRadius, Height/2 - PinRadius, PinRadius*2,
                        PinRadius*2);
                }
            }
        }

        private void DrawHorizontalLines(Graphics graphics)
        {
            if (_roadLines == null || _roadLines.Count == 0)
            {
                graphics.DrawLine(Pens.Black, PaintMargin, Height/2, Width - PaintMargin, Height/2);
            }
            else
            {
                int margin = (Height - _roadLines.Count*LineThickness - (_roadLines.Count - 1)*LineMargin)/2;
                for (int i = 0; i < _roadLines.Count; i++)
                {
                    int y = margin + (LineThickness + LineMargin)*i;
                    graphics.DrawLine(_roadLines[i].Color, PaintMargin, y, Width - PaintMargin, y);
                }
            }
        }

        private void DrawVerticalLines(Graphics graphics)
        {
            if (_roadLines == null || _roadLines.Count == 0)
            {
                graphics.DrawLine(Pens.Black, Width/2, PaintMargin, Width/2, Height - PaintMargin);
            }
            else
            {
                int margin = (Width - _roadLines.Count*LineThickness - (_roadLines.Count - 1)*LineMargin)/2;
                for (int i = 0; i < _roadLines.Count; i++)
                {
                    int x = margin + (LineThickness + LineMargin)*i;
                    graphics.DrawLine(_roadLines[i].Color, x, PaintMargin, x, Height - PaintMargin);
                }
            }
        }

        private void Road_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsNearToPin(e))
            {
                _isDrawPin = true;
                Invalidate();
            }
            else
            {
                if (_isDrawPin)
                {
                    _isDrawPin = false;
                    Invalidate();
                }
            }
        }

        private void Road_MouseClick(object sender, MouseEventArgs e)
        {
            if (IsNearToPin(e) && PinClick != null)
            {
                PinClick(this, EventArgs.Empty);
            }
        }
    }
}