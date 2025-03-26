using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WorkTimeCounterWidget
{
    public class RoundedButton_horizontal : PictureBox
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
                UpdateImages();
            }
        }

        public RoundedButton_horizontal()
        {
            this.SizeMode = PictureBoxSizeMode.StretchImage; // Obrazek dopasowuje się do rozmiaru
            this.BackColor = Color.Transparent;
            this.LocationState = ButtonLocation.Mid;

            _imageFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

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
                        _normalImage = LoadAndResizeImage(Path.Combine(appDirectory, "Images", "left_normal.png"));
                        _hoverImage = LoadAndResizeImage(Path.Combine(appDirectory, "Images", "left_hover.png"));
                        _clickImage = LoadAndResizeImage(Path.Combine(appDirectory, "Images", "left_click.png"));
                        break;

                    case ButtonLocation.Mid:
                        _normalImage = LoadAndResizeImage(Path.Combine(appDirectory, "Images", "mid_normal.png"));
                        _hoverImage = LoadAndResizeImage(Path.Combine(appDirectory, "Images", "mid_hover.png"));
                        _clickImage = LoadAndResizeImage(Path.Combine(appDirectory, "Images", "mid_click.png"));
                        break;

                    case ButtonLocation.Right:
                        _normalImage = LoadAndResizeImage(Path.Combine(appDirectory, "Images", "right_normal.png"));
                        _hoverImage = LoadAndResizeImage(Path.Combine(appDirectory, "Images", "right_hover.png"));
                        _clickImage = LoadAndResizeImage(Path.Combine(appDirectory, "Images", "right_click.png"));
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
            UpdateImages();
        }
    }
}
