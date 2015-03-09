using System;
using System.Drawing;
using System.Windows.Forms;

namespace Modeller.CustomControls
{
    public partial class Crossroad : ACrossroadControl
    {
        private const int NearSize = 4;
        private const int PinRadius = 4;
        private bool _isDrawDownPin;
        private bool _isDrawLeftPin;
        private bool _isDrawRightPin;
        private bool _isDrawUpPin;

        public Crossroad()
        {
            InitializeComponent();
        }

        public event EventHandler LeftPinClick;
        public event EventHandler UpPinClick;
        public event EventHandler RightPinClick;
        public event EventHandler DownPinClick;

        private bool IsNearTo(int val, int check, int eps)
        {
            if (Math.Abs(val - check) <= eps)
            {
                return true;
            }

            return false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Graphics graphics = CreateGraphics())
            {
                graphics.DrawLine(Pens.Black, 10, Height/2, Width - 10, Height/2);
                graphics.DrawLine(Pens.Black, Width/2, 10, Width/2, Height - 10);

                if (_isDrawLeftPin)
                {
                    graphics.FillEllipse(Brushes.Blue, 10 - PinRadius, Height/2 - PinRadius, PinRadius*2, PinRadius*2);
                }
                else if (_isDrawUpPin)
                {
                    graphics.FillEllipse(Brushes.Blue, Width/2 - PinRadius, 10 - PinRadius, PinRadius*2, PinRadius*2);
                }
                else if (_isDrawRightPin)
                {
                    graphics.FillEllipse(Brushes.Blue, Width - 10 - PinRadius, Height/2 - PinRadius, PinRadius*2,
                        PinRadius*2);
                }
                else if (_isDrawDownPin)
                {
                    graphics.FillEllipse(Brushes.Blue, Width/2 - PinRadius, Height - 10 - PinRadius, PinRadius*2,
                        PinRadius*2);
                }
            }
        }

        private void Crossroad_MouseMove(object sender, MouseEventArgs e)
        {
            /*if (IsNearToLeftPin(e))
            {
                _isDrawLeftPin = true;
                Invalidate();
            }
            else if (IsNearToUpPin(e))
            {
                _isDrawUpPin = true;
                Invalidate();
            }
            else if (IsNearToRightPin(e))
            {
                _isDrawRightPin = true;
                Invalidate();
            }
            else if (IsNearToDownPin(e))
            {
                _isDrawDownPin = true;
                Invalidate();
            }
            else
            {
                if (_isDrawLeftPin || _isDrawUpPin || _isDrawRightPin || _isDrawDownPin)
                {
                    _isDrawLeftPin = false;
                    _isDrawUpPin = false;
                    _isDrawRightPin = false;
                    _isDrawDownPin = false;
                    Invalidate();
                }
            }*/
        }

        private bool IsNearToDownPin(MouseEventArgs e)
        {
            return IsNearTo(e.X, Width/2, NearSize) && IsNearTo(e.Y, Height - 10, NearSize);
        }

        private bool IsNearToRightPin(MouseEventArgs e)
        {
            return IsNearTo(e.X, Width - 10, NearSize) && IsNearTo(e.Y, Height/2, NearSize);
        }

        private bool IsNearToUpPin(MouseEventArgs e)
        {
            return IsNearTo(e.X, Width/2, NearSize) && IsNearTo(e.Y, 10, NearSize);
        }

        private bool IsNearToLeftPin(MouseEventArgs e)
        {
            return IsNearTo(e.X, 10, NearSize) && IsNearTo(e.Y, Height/2, NearSize);
        }

        private void Crossroad_MouseClick(object sender, MouseEventArgs e)
        {
            /*if (IsNearToLeftPin(e) && LeftPinClick != null)
            {
                LeftPinClick(this, EventArgs.Empty);
            }
            else if (IsNearToUpPin(e) && UpPinClick != null)
            {
                UpPinClick(this, EventArgs.Empty);
            }
            else if (IsNearToRightPin(e) && RightPinClick != null)
            {
                RightPinClick(this, EventArgs.Empty);
            }
            else if (IsNearToDownPin(e) && DownPinClick != null)
            {
                DownPinClick(this, EventArgs.Empty);
            }*/
        }
    }
}