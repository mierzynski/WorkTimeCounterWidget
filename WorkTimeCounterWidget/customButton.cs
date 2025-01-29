using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WorkTimeCounterWidget
{
    public class RoundedButton : Button
    {
        private Color _normalColor = Color.Black;
        private Color _pressedColor = Color.White;
        private bool _isPressed = false;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        public RoundedButton() : this(string.Empty) { }

        public RoundedButton(string text, int? width = null, int? height = null)
        {
            this.Text = text;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = _normalColor;
            this.ForeColor = Color.White;
            this.Font = new Font("Arial", 8, FontStyle.Bold);

            // Obliczenie domyślnych rozmiarów, jeśli nie podano
            UpdateSize(width, height);

            // Obsługa zdarzeń myszy
            this.MouseDown += (sender, e) => { _isPressed = true; this.Invalidate(); };
            this.MouseUp += (sender, e) => { _isPressed = false; this.Invalidate(); };
            this.MouseLeave += (sender, e) => { _isPressed = false; this.Invalidate(); };

            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));
        }

        private void UpdateSize(int? width, int? height)
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                SizeF textSize = g.MeasureString(this.Text, this.Font);
                this.Size = new Size(
                    width ?? (int)Math.Ceiling(textSize.Width) + 20,   // Szerokość = tekst + 20px
                    height ?? (int)Math.Ceiling(textSize.Height) + 4   // Wysokość = tekst + 4px
                );
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Ustal kolor tła
            Color backColor = _isPressed ? _pressedColor : _normalColor;

            using (SolidBrush brush = new SolidBrush(backColor))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }

            // Rysowanie tekstu
            using (SolidBrush textBrush = new SolidBrush(this.ForeColor))
            {
                SizeF textSize = g.MeasureString(this.Text, this.Font);
                PointF textLocation = new PointF(
                    (this.Width - textSize.Width) / 2,
                    (this.Height - textSize.Height) / 2
                );
                g.DrawString(this.Text, this.Font, textBrush, textLocation);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            UpdateSize(null, null); // Automatyczne dostosowanie przy zmianie tekstu
        }
    }
}
