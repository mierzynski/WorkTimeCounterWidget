using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WorkTimeCounterWidget
{
    public class CustomButton : PictureBox
    {
        public enum ButtonLocation { Left, Mid, Right, Up, Down }
        public enum ButtonOrientation { Horizontal, Vertical }

        private ButtonLocation _location;
        private ButtonOrientation _orientation;
        private Image _normalImage;
        private Image _hoverImage;
        private Image _clickImage;

        public ButtonLocation LocationState
        {
            get { return _location; }
            set
            {
                _location = value;
                UpdateImages();
            }
        }

        public ButtonOrientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                UpdateImages();
            }
        }

        public CustomButton()
        {
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.BackColor = Color.Transparent;
            this.LocationState = ButtonLocation.Mid;
            this.Orientation = ButtonOrientation.Horizontal; // Domyślnie poziomy

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
                string imageFolder = Path.Combine(appDirectory, "Images");

                if (Orientation == ButtonOrientation.Horizontal)
                {
                    switch (LocationState)
                    {
                        case ButtonLocation.Left:
                            _normalImage = LoadAndResizeImage(Path.Combine(imageFolder, "left_normal.png"));
                            _hoverImage = LoadAndResizeImage(Path.Combine(imageFolder, "left_hover.png"));
                            _clickImage = LoadAndResizeImage(Path.Combine(imageFolder, "left_click.png"));
                            break;

                        case ButtonLocation.Mid:
                            _normalImage = LoadAndResizeImage(Path.Combine(imageFolder, "mid_normal.png"));
                            _hoverImage = LoadAndResizeImage(Path.Combine(imageFolder, "mid_hover.png"));
                            _clickImage = LoadAndResizeImage(Path.Combine(imageFolder, "mid_click.png"));
                            break;

                        case ButtonLocation.Right:
                            _normalImage = LoadAndResizeImage(Path.Combine(imageFolder, "right_normal.png"));
                            _hoverImage = LoadAndResizeImage(Path.Combine(imageFolder, "right_hover.png"));
                            _clickImage = LoadAndResizeImage(Path.Combine(imageFolder, "right_click.png"));
                            break;
                    }
                }
                else if (Orientation == ButtonOrientation.Vertical)
                {
                    switch (LocationState)
                    {
                        case ButtonLocation.Up:
                            _normalImage = LoadAndResizeImage(Path.Combine(imageFolder, "up_normal.png"));
                            _hoverImage = LoadAndResizeImage(Path.Combine(imageFolder, "up_hover.png"));
                            _clickImage = LoadAndResizeImage(Path.Combine(imageFolder, "up_click.png"));
                            break;

                        case ButtonLocation.Down:
                            _normalImage = LoadAndResizeImage(Path.Combine(imageFolder, "down_normal.png"));
                            _hoverImage = LoadAndResizeImage(Path.Combine(imageFolder, "down_hover.png"));
                            _clickImage = LoadAndResizeImage(Path.Combine(imageFolder, "down_click.png"));
                            break;
                    }
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
