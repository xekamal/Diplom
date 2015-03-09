using System;
using System.Drawing;
using System.Windows.Forms;

namespace Modeller.CustomControls
{
    public partial class Turn : ACrossroadControl
    {
        private const int NearSize = 4;
        private const int PinRadius = 4;
        private const int PaintMargin = 10;
        private bool _isDrawPin;
        private TurnType _type;

        public Turn()
        {
            InitializeComponent();
        }

        public TurnType Type
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
            return IsNearTo(e.X, Width / 2, NearSize) && IsNearTo(e.Y, Height / 2, NearSize);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Graphics graphics = CreateGraphics())
            {
                switch (_type)
                {
                    case TurnType.LeftToUp:
                        graphics.DrawLine(Pens.Black, PaintMargin, Height / 2, Width / 2, Height / 2);
                        graphics.DrawLine(Pens.Black, Width / 2, Height/2, Width/2, PaintMargin);
                        break;
                    case TurnType.UpToRight:
                        graphics.DrawLine(Pens.Black, Width/2, PaintMargin, Width/2, Height/2);
                        graphics.DrawLine(Pens.Black, Width/2, Height/2, Width-PaintMargin, Height/2);
                        break;
                    case TurnType.RightToDown:
                        graphics.DrawLine(Pens.Black, Width-PaintMargin, Height/2, Width/2, Height/2);
                        graphics.DrawLine(Pens.Black, Width/2, Height/2, Width/2, Height-PaintMargin);
                        break;
                    case TurnType.DownToLeft:
                        graphics.DrawLine(Pens.Black, Width/2, Height-PaintMargin, Width/2, Height/2);
                        graphics.DrawLine(Pens.Black, Width/2, Height/2, PaintMargin, Height/2);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (_isDrawPin)
                {
                    graphics.FillEllipse(Brushes.Blue, Width / 2 - PinRadius, Height / 2 - PinRadius, PinRadius * 2,
                        PinRadius * 2);
                }
            }
        }

        private void Turn_MouseMove(object sender, MouseEventArgs e)
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

        private void Turn_MouseClick(object sender, MouseEventArgs e)
        {
            if (IsNearToPin(e) && PinClick != null)
            {
                PinClick(this, EventArgs.Empty);
            }
        }
    }
}