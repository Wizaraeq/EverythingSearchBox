using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuizoPlugins
{
    internal sealed class SearchBoxToolbarControl : UserControl
    {
        private readonly PictureBox iconBox;
        private readonly CustomFilterBox searchBox;
        private readonly int iconSize;

        public SearchBoxToolbarControl(string placeholderText, Image iconImage, bool fLarge)
        {
            iconSize = fLarge ? 24 : 16;

            iconBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.CenterImage,
                Width = iconSize,
                Height = iconSize,
                Left = 2
            };

            searchBox = new CustomFilterBox(placeholderText)
            {
                Left = iconBox.Right + 4
            };

            Width = searchBox.Right + 2;
            Height = Math.Max(searchBox.Height, iconSize) + 2;

            iconBox.Top = (Height - iconBox.Height) / 2;
            searchBox.Top = (Height - searchBox.Height) / 2;

            Controls.Add(iconBox);
            Controls.Add(searchBox);

            UpdateIcon(iconImage);

            searchBox.KeyDown += SearchBox_KeyDown;
        }

        public event EventHandler<string> QuerySubmitted;

        public bool IsLargeLayout => iconSize > 16;

        public void UpdatePlaceholderText(string placeholderText)
        {
            searchBox.UpdatePlaceholderText(placeholderText);
        }

        public void UpdateIcon(Image iconImage)
        {
            Image previousImage = iconBox.Image;
            iconBox.Image = iconImage != null ? new Bitmap(iconImage) : null;
            previousImage?.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                searchBox.KeyDown -= SearchBox_KeyDown;
                iconBox.Image?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            QuerySubmitted?.Invoke(this, searchBox.Text);
            searchBox.Clear();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }
}
