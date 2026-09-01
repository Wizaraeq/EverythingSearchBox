using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuizoPlugins
{
    internal sealed class SearchBoxToolbarControl : UserControl
    {
        private readonly CustomFilterBox searchBox;

        public SearchBoxToolbarControl(string placeholderText, Image iconImage, bool fLarge)
        {
            searchBox = new CustomFilterBox(placeholderText)
            {
                Left = 2
            };

            Width = searchBox.Right + 2;
            Height = searchBox.Height + 2;
            searchBox.Top = (Height - searchBox.Height) / 2;

            Controls.Add(searchBox);

            searchBox.KeyDown += SearchBox_KeyDown;
        }

        public event EventHandler<string> QuerySubmitted;

        public bool IsLargeLayout => false;

        public void UpdatePlaceholderText(string placeholderText)
        {
            searchBox.UpdatePlaceholderText(placeholderText);
        }

        public void UpdateIcon(Image iconImage)
        {
            // アイコンは表示しない
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                searchBox.KeyDown -= SearchBox_KeyDown;
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
