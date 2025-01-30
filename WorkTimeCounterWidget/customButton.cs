using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WorkTimeCounterWidget
{
    //public class RoundedButton : Button
    //{
    //    private Color _normalColor = Color.Black;
    //    private Color _pressedColor = Color.White;
    //    private bool _isPressed = false;

    //    [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
    //    private static extern IntPtr CreateRoundRectRgn(
    //        int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
    //        int nWidthEllipse, int nHeightEllipse
    //    );

    //    public RoundedButton() : this(string.Empty) { }

    //    public RoundedButton(string text, int? width = null, int? height = null)
    //    {
    //        this.Text = text;
    //        this.FlatStyle = FlatStyle.Flat;
    //        this.FlatAppearance.BorderSize = 0;
    //        this.BackColor = _normalColor;
    //        this.ForeColor = Color.White;
    //        this.Font = new Font("Arial", 8, FontStyle.Bold);

    //        // Obliczenie domyślnych rozmiarów, jeśli nie podano
    //        UpdateSize(width, height);

    //        // Obsługa zdarzeń myszy
    //        this.MouseDown += (sender, e) => { _isPressed = true; this.Invalidate(); };
    //        this.MouseUp += (sender, e) => { _isPressed = false; this.Invalidate(); };
    //        this.MouseLeave += (sender, e) => { _isPressed = false; this.Invalidate(); };

    //        this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));
    //    }

    //    private void UpdateSize(int? width, int? height)
    //    {
    //        using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
    //        {
    //            SizeF textSize = g.MeasureString(this.Text, this.Font);
    //            this.Size = new Size(
    //                width ?? (int)Math.Ceiling(textSize.Width) + 20,   // Szerokość = tekst + 20px
    //                height ?? (int)Math.Ceiling(textSize.Height) + 4   // Wysokość = tekst + 4px
    //            );
    //        }
    //    }

    //    protected override void OnPaint(PaintEventArgs e)
    //    {
    //        base.OnPaint(e);
    //        Graphics g = e.Graphics;
    //        g.SmoothingMode = SmoothingMode.AntiAlias;

    //        // Ustal kolor tła
    //        Color backColor = _isPressed ? _pressedColor : _normalColor;

    //        using (SolidBrush brush = new SolidBrush(backColor))
    //        {
    //            g.FillRectangle(brush, this.ClientRectangle);
    //        }

    //        // Rysowanie tekstu
    //        using (SolidBrush textBrush = new SolidBrush(this.ForeColor))
    //        {
    //            SizeF textSize = g.MeasureString(this.Text, this.Font);
    //            PointF textLocation = new PointF(
    //                (this.Width - textSize.Width) / 2,
    //                (this.Height - textSize.Height) / 2
    //            );
    //            g.DrawString(this.Text, this.Font, textBrush, textLocation);
    //        }
    //    }

    //    protected override void OnResize(EventArgs e)
    //    {
    //        base.OnResize(e);
    //        this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));
    //    }

    //    protected override void OnTextChanged(EventArgs e)
    //    {
    //        base.OnTextChanged(e);
    //        UpdateSize(null, null); // Automatyczne dostosowanie przy zmianie tekstu
    //    }
    //}

    public class RoundedButton : Button
    {
        //Fields
        private int borderSize = 0;
    private int borderRadius = 0;
    private Color borderColor = Color.PaleVioletRed;

    //Properties
    [Category("Properties RoundedButton")]
    public int BorderSize
    {
        get { return borderSize; }
        set
        {
            borderSize = value;
            this.Invalidate();
        }
    }

    [Category("Properties RoundedButton")]
    public int BorderRadius
    {
        get { return borderRadius; }
        set
        {
            borderRadius = value;
            this.Invalidate();
        }
    }

    [Category("Properties RoundedButton")]
    public Color BorderColor
    {
        get { return borderColor; }
        set
        {
            borderColor = value;
            this.Invalidate();
        }
    }

    [Category("Properties RoundedButton")]
    public Color BackgroundColor
    {
        get { return this.BackColor; }
        set { this.BackColor = value; }
    }

    [Category("Properties RoundedButton")]
    public Color TextColor
    {
        get { return this.ForeColor; }
        set { this.ForeColor = value; }
    }

    //Constructor
    public RoundedButton()
    {
        this.FlatStyle = FlatStyle.Flat;
        this.FlatAppearance.BorderSize = 0;
        this.Size = new Size(150, 40);
        this.BackColor = Color.MediumSlateBlue;
        this.ForeColor = Color.White;
        this.Resize += new EventHandler(Button_Resize);
    }

    //Methods
    private GraphicsPath GetFigurePath(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        float curveSize = radius * 2F;

        path.StartFigure();
        path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
        path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
        path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
        path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
        path.CloseFigure();
        return path;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);


        Rectangle rectSurface = this.ClientRectangle;
        Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
        int smoothSize = 2;
        if (borderSize > 0)
            smoothSize = borderSize;

        if (borderRadius > 2) //Rounded button
        {
            using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
            using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
            using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //Button surface
                this.Region = new Region(pathSurface);
                //Draw surface border for HD result
                pevent.Graphics.DrawPath(penSurface, pathSurface);

                //Button border                    
                if (borderSize >= 1)
                    //Draw control border
                    pevent.Graphics.DrawPath(penBorder, pathBorder);
            }
        }
        else //Normal button
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.None;
            //Button surface
            this.Region = new Region(rectSurface);
            //Button border
            if (borderSize >= 1)
            {
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    penBorder.Alignment = PenAlignment.Inset;
                    pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                }
            }
        }
    }
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
    }

    private void Container_BackColorChanged(object sender, EventArgs e)
    {
        this.Invalidate();
    }
    private void Button_Resize(object sender, EventArgs e)
    {
        if (borderRadius > this.Height)
            borderRadius = this.Height;
    }
}
}
