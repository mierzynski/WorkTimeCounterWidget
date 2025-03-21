using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WorkTimeCounterWidget
{
    public class RoundedButton_horizontal : Button
    {
        public enum ButtonLocation { Left, Mid, Right }

        private ButtonLocation _location;
        private Image _normalImage;
        private Image _hoverImage;
        private Image _clickImage;
        private readonly string _imageFolderPath;

        public ButtonLocation LocationState
        {
            get { return _location; }
            set
            {
                _location = value;
                UpdateImages(); // Zmienia obrazy w zależności od stanu
            }
        }

        public RoundedButton_horizontal()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.Transparent;
            this.LocationState = ButtonLocation.Mid; // Domyślnie ustawione na środek

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _imageFolderPath = Path.Combine(appDirectory, "Images");

            this.MouseEnter += (s, e) => this.Image = _hoverImage;
            this.MouseLeave += (s, e) => this.Image = _normalImage;
            this.MouseDown += (s, e) => this.Image = _clickImage;
            this.MouseUp += (s, e) => this.Image = _hoverImage;
        }

        private void UpdateImages()
        {
            try
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

                switch (LocationState)
                {
                    case ButtonLocation.Left:
                        _normalImage = Image.FromFile(Path.Combine(appDirectory, "Images", "mid_normal.png"));
                        _hoverImage = Image.FromFile(Path.Combine(appDirectory, "Images", "mid_hover.png"));
                        _clickImage = Image.FromFile(Path.Combine(appDirectory, "Images", "mid_normal.png"));
                        break;

                    case ButtonLocation.Mid:
                        _normalImage = Image.FromFile(Path.Combine(appDirectory, "Images", "mid_normal.png"));
                        _hoverImage = Image.FromFile(Path.Combine(appDirectory, "Images", "mid_hover.png"));
                        _clickImage = Image.FromFile(Path.Combine(appDirectory, "Images", "mid_normal.png"));
                        break;

                    case ButtonLocation.Right:
                        _normalImage = Image.FromFile(Path.Combine(appDirectory, "Images", "mid_normal.png"));
                        _hoverImage = Image.FromFile(Path.Combine(appDirectory, "Images", "mid_hover.png"));
                        _clickImage = Image.FromFile(Path.Combine(appDirectory, "Images", "mid_normal.png"));
                        break;
                }

                this.Image = _normalImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania obrazków: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Image LoadAndResizeImage(string imagePath)
        {
            if (!File.Exists(imagePath)) return null;

            using (Image originalImage = Image.FromFile(imagePath))
            {
                return new Bitmap(originalImage, this.Size);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateImages(); // Ponowne przeskalowanie obrazów przy zmianie rozmiaru
        }

    }
}