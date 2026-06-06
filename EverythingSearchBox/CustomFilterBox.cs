using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace QuizoPlugins
{
    public class CustomFilterBox : TextBox
    {
        private const int EM_SETCUEBANNER = 0x1501;

        private string placeholderText;
        private bool isDarkMode;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        public CustomFilterBox(string placeholderText)
        {
            this.placeholderText = placeholderText ?? string.Empty;

            Width = 85;
            Multiline = false;
            AutoSize = false;
            Height = 20;

            SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
            AdaptToCurrentTheme();
        }

        public void UpdatePlaceholderText(string placeholderText)
        {
            this.placeholderText = placeholderText ?? string.Empty;
            ApplyCueBanner();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApplyCueBanner();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;
            }

            base.Dispose(disposing);
        }

        private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General)
            {
                AdaptToCurrentTheme();
            }
        }

        private void AdaptToCurrentTheme()
        {
            isDarkMode = IsWindowsDarkMode();
            BackColor = isDarkMode ? Color.FromArgb(30, 30, 30) : SystemColors.Window;
            ForeColor = isDarkMode ? Color.White : SystemColors.WindowText;
            Invalidate();
        }

        private void ApplyCueBanner()
        {
            if (!IsHandleCreated)
            {
                return;
            }

            SendMessage(Handle, EM_SETCUEBANNER, IntPtr.Zero, placeholderText);
        }

        private static bool IsWindowsDarkMode()
        {
            try
            {
                using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    object value = registryKey?.GetValue("AppsUseLightTheme");
                    if (value is int intValue && intValue == 0)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // Ignore errors and assume light mode if anything goes wrong.
            }

            return false;
        }
    }
}
