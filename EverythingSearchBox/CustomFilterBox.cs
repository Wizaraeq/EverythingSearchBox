using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

public class CustomFilterBox : TextBox
{
    private string placeholderText;
    private readonly Color placeholderColor = SystemColors.GrayText;
    private bool isDarkMode;

    public CustomFilterBox(string placeholderText)
    {
        this.placeholderText = placeholderText ?? string.Empty;

        // Set default width and prevent resizing
        this.Width = 85;
        this.Multiline = false; // Prevent multi-line input
        this.AutoSize = false;  // Disable automatic resizing
        this.Height = 20;       // Set a fixed height

        // Set initial placeholder text
        SetPlaceholder(); // Call the parameterless version

        // Handle focus events for placeholder text
        this.GotFocus += RemovePlaceholder;
        this.LostFocus += SetPlaceholder;

        // Listen for system theme changes
        SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;

        // Set the initial theme
        AdaptToCurrentTheme();
    }

    public void UpdatePlaceholderText(string placeholderText)
    {
        bool wasShowingPlaceholder = HasPlaceholderText && this.Text == this.placeholderText;
        this.placeholderText = placeholderText ?? string.Empty;

        if (wasShowingPlaceholder || string.IsNullOrWhiteSpace(this.Text))
        {
            this.Text = string.Empty;
            SetPlaceholder();
        }
        else
        {
            this.ForeColor = isDarkMode ? Color.White : SystemColors.WindowText;
            this.Invalidate();
        }
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

        if (isDarkMode)
        {
            this.BackColor = Color.FromArgb(30, 30, 30); // Dark background color
        }
        else
        {
            this.BackColor = SystemColors.Window; // Light background color
        }

        // Update the placeholder or text color based on the theme
        if (HasPlaceholderText && this.Text == placeholderText)
        {
            this.ForeColor = placeholderColor;
        }
        else
        {
            this.ForeColor = isDarkMode ? Color.White : SystemColors.WindowText;
        }

        // Force a repaint to apply new theme colors
        this.Invalidate();
    }

    private bool IsWindowsDarkMode()
    {
        try
        {
            var registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if (registryKey != null)
            {
                var value = registryKey.GetValue("AppsUseLightTheme");
                if (value != null && (int)value == 0)
                {
                    return true; // Dark mode is enabled
                }
            }
        }
        catch
        {
            // Ignore errors and assume light mode if anything goes wrong
        }
        return false;
    }

    private void RemovePlaceholder(object sender, EventArgs e)
    {
        if (HasPlaceholderText && this.Text == placeholderText)
        {
            this.Text = "";
            this.ForeColor = isDarkMode ? Color.White : SystemColors.WindowText;
        }

        // Redraw to remove placeholder
        this.Invalidate();
    }

    // Overloaded method without parameters
    private void SetPlaceholder()
    {
        if (HasPlaceholderText && string.IsNullOrWhiteSpace(this.Text))
        {
            this.Text = placeholderText;
            this.ForeColor = placeholderColor;
            this.Invalidate(); // Redraw to display the placeholder
        }
        else if (!HasPlaceholderText && string.IsNullOrEmpty(this.Text))
        {
            this.ForeColor = isDarkMode ? Color.White : SystemColors.WindowText;
        }
    }

    // Original method with parameters
    private void SetPlaceholder(object sender, EventArgs e)
    {
        SetPlaceholder(); // Call the parameterless version
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;
        }
        base.Dispose(disposing);
    }

    private bool HasPlaceholderText => !string.IsNullOrEmpty(placeholderText);
}
