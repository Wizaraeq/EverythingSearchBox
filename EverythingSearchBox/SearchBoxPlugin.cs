using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using QTPlugin;
using QTPlugin.Interop;

namespace QuizoPlugins
{
    [Plugin(PluginType.BackgroundMultiple, typeof(Localizer), Version = "1.0.0.0")]
    public class SearchBoxPlugin : IBarMultipleCustomItems
    {
        private const int SW_SHOWNORMAL = 1;
        private const int WM_COPYDATA = 0x004A;
        private const int SMTO_ABORTIFHUNG = 0x0002;
        private const string SettingsRegistryPath = @"Software\QuizoPlugins\SearchBoxPlugin";
        private const string EverythingPathValueName = "EverythingPath";
        private const string PlaceholderTextValueName = "PlaceholderText";
        private const string DefaultPlaceholderText = "Search...";
        private static readonly IntPtr EverythingCommandLineCopyData = new IntPtr(0);
        private const string EverythingTaskbarNotificationWindowClass = "EVERYTHING_TASKBAR_NOTIFICATION";

        private List<ToolStripItem> searchBoxes = new List<ToolStripItem>();

        private IPluginServer pluginServer;
        private string placeholderText = DefaultPlaceholderText;

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessageTimeout(
            IntPtr hWnd,
            uint msg,
            IntPtr wParam,
            ref COPYDATASTRUCT lParam,
            uint fuFlags,
            uint uTimeout,
            out IntPtr lpdwResult);

        [StructLayout(LayoutKind.Sequential)]
        private struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        #region IPluginClient members

        public void Open(IPluginServer pluginServer, IShellBrowser shellBrowser)
        {
            // Store the pluginServer instance to get the current tab path later
            this.pluginServer = pluginServer;
            this.placeholderText = LoadPlaceholderText();
        }

        public void Close(EndCode endCode)
        {
            foreach (var searchBox in searchBoxes)
            {
                searchBox.Dispose();
            }
            searchBoxes.Clear();
        }

        public bool HasOption => true;

        public void OnMenuItemClick(MenuType menuType, string menuText, ITab tab) { }

        public void OnOption()
        {
            string updatedPlaceholder = ShowPlaceholderOptionsDialog(this.placeholderText);
            if (updatedPlaceholder == null)
            {
                return;
            }

            this.placeholderText = updatedPlaceholder;
            SavePlaceholderText(this.placeholderText);
            UpdateOpenSearchBoxPlaceholders();
        }

        public void OnShortcutKeyPressed(int index) { }

        public bool QueryShortcutKeys(out string[] actions)
        {
            // No shortcut keys are needed for this plugin
            actions = null;
            return false;
        }

        #endregion

        #region IBarMultipleCustomItems members

        public void Initialize(int[] order)
        {
            foreach (var searchBox in searchBoxes)
            {
                searchBox.Dispose();
            }
            searchBoxes.Clear();
        }

        public ToolStripItem CreateItem(bool fLarge, DisplayStyle displayStyle, int index)
        {
            var customFilterBox = new CustomFilterBox(this.placeholderText);

            // Handle KeyDown event to detect "Enter"
            customFilterBox.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string query = customFilterBox.Text;
                    // Call your search method
                    RunEverythingSearch(query);
                    customFilterBox.Clear();
                }
            };

            var controlHost = new ToolStripControlHost(customFilterBox)
            {
                AutoSize = false
            };

            searchBoxes.Add(controlHost);
            return controlHost;
        }


        public int Count => -1;

        public System.Drawing.Image GetImage(bool fLarge, int index)
        {
            // No icon for this custom item
            return null;
        }

        public string GetName(int index)
        {
            return "Search Box";
        }

        #endregion

        private string GetCurrentDirectory()
        {
            string currentDirectory = null;

            // Attempt to use pluginServer.SelectedTab.Path
            if (pluginServer != null && pluginServer.SelectedTab != null)
            {
                currentDirectory = pluginServer.SelectedTab.Path;
                if (!string.IsNullOrEmpty(currentDirectory))
                {
                    return currentDirectory;
                }
            }

            // If that didn't work, try pluginServer.Path
            if (string.IsNullOrEmpty(currentDirectory))
            {
                currentDirectory = pluginServer.Path;
                if (!string.IsNullOrEmpty(currentDirectory))
                {
                    return currentDirectory;
                }
            }

            // As a last resort, use Environment.CurrentDirectory
            currentDirectory = Environment.CurrentDirectory;
            return currentDirectory;
        }

        private void RunEverythingSearch(string query)
        {
            string currentDirectory = GetCurrentDirectory();

            if (string.IsNullOrEmpty(currentDirectory))
            {
                MessageBox.Show("Could not retrieve current directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Include the -s option for the search query
            string arguments = $"-sort size -nomaximized -path \"{currentDirectory}\" -s {EscapeArgument(query)}";
            if (TrySendCommandLineToRunningEverything(arguments))
            {
                return;
            }

            string everythingPath = ResolveEverythingPath();

            if (string.IsNullOrEmpty(everythingPath))
            {
                everythingPath = PromptForEverythingExecutable();
                if (string.IsNullOrEmpty(everythingPath))
                {
                    return;
                }
            }

            if (TryStartEverything(everythingPath, arguments))
            {
                return;
            }

            string userSelectedPath = PromptForEverythingExecutable();
            if (!string.IsNullOrEmpty(userSelectedPath) && !string.Equals(userSelectedPath, everythingPath, StringComparison.OrdinalIgnoreCase))
            {
                TryStartEverything(userSelectedPath, arguments);
            }
        }

        private bool TrySendCommandLineToRunningEverything(string arguments)
        {
            IntPtr everythingWindow = FindWindow(EverythingTaskbarNotificationWindowClass, null);
            if (everythingWindow == IntPtr.Zero)
            {
                return false;
            }

            byte[] commandLineBytes = Encoding.UTF8.GetBytes(arguments + "\0");
            int payloadSize = sizeof(int) + commandLineBytes.Length;
            IntPtr payload = IntPtr.Zero;

            try
            {
                payload = Marshal.AllocHGlobal(payloadSize);
                Marshal.WriteInt32(payload, SW_SHOWNORMAL);
                Marshal.Copy(commandLineBytes, 0, IntPtr.Add(payload, sizeof(int)), commandLineBytes.Length);

                var copyData = new COPYDATASTRUCT
                {
                    dwData = EverythingCommandLineCopyData,
                    cbData = payloadSize,
                    lpData = payload
                };

                IntPtr sendResult;
                IntPtr callResult = SendMessageTimeout(
                    everythingWindow,
                    WM_COPYDATA,
                    IntPtr.Zero,
                    ref copyData,
                    SMTO_ABORTIFHUNG,
                    2000,
                    out sendResult);

                return callResult != IntPtr.Zero && sendResult != IntPtr.Zero;
            }
            finally
            {
                if (payload != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(payload);
                }
            }
        }

        private string ResolveEverythingPath()
        {
            var seenPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string candidate in GetEverythingCandidates())
            {
                if (string.IsNullOrWhiteSpace(candidate))
                {
                    continue;
                }

                string fullPath;
                try
                {
                    fullPath = Path.GetFullPath(candidate);
                }
                catch
                {
                    continue;
                }

                if (!seenPaths.Add(fullPath))
                {
                    continue;
                }

                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }

            return null;
        }

        private IEnumerable<string> GetEverythingCandidates()
        {
            string savedPath = LoadSavedEverythingPath();
            if (!string.IsNullOrWhiteSpace(savedPath))
            {
                yield return savedPath;
            }

            foreach (string registryPath in GetAppPathCandidates())
            {
                yield return registryPath;
            }

            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string[] baseDirectories =
            {
                Path.Combine(programFiles, "Everything 1.5a"),
                Path.Combine(programFiles, "Everything"),
                Path.Combine(programFilesX86, "Everything 1.5a"),
                Path.Combine(programFilesX86, "Everything")
            };
            string[] fileNames =
            {
                "Everything64.exe",
                "Everything.exe"
            };

            foreach (string baseDirectory in baseDirectories)
            {
                if (string.IsNullOrWhiteSpace(baseDirectory))
                {
                    continue;
                }

                foreach (string fileName in fileNames)
                {
                    yield return Path.Combine(baseDirectory, fileName);
                }
            }
        }

        private IEnumerable<string> GetAppPathCandidates()
        {
            const string appPathsKey = @"Software\Microsoft\Windows\CurrentVersion\App Paths";
            string[] exeNames =
            {
                "Everything64.exe",
                "Everything.exe"
            };
            RegistryKey[] roots =
            {
                Registry.CurrentUser,
                Registry.LocalMachine
            };

            foreach (RegistryKey root in roots)
            {
                foreach (string exeName in exeNames)
                {
                    using (RegistryKey appKey = root.OpenSubKey(Path.Combine(appPathsKey, exeName)))
                    {
                        string configuredPath = appKey?.GetValue(string.Empty) as string;
                        if (!string.IsNullOrWhiteSpace(configuredPath))
                        {
                            yield return configuredPath;
                        }
                    }
                }
            }
        }

        private string LoadSavedEverythingPath()
        {
            using (RegistryKey settingsKey = Registry.CurrentUser.OpenSubKey(SettingsRegistryPath, false))
            {
                return settingsKey?.GetValue(EverythingPathValueName) as string;
            }
        }

        private string LoadPlaceholderText()
        {
            using (RegistryKey settingsKey = Registry.CurrentUser.OpenSubKey(SettingsRegistryPath, false))
            {
                return settingsKey?.GetValue(PlaceholderTextValueName, DefaultPlaceholderText) as string ?? DefaultPlaceholderText;
            }
        }

        private void SaveEverythingPath(string everythingPath)
        {
            if (string.IsNullOrWhiteSpace(everythingPath))
            {
                return;
            }

            using (RegistryKey settingsKey = Registry.CurrentUser.CreateSubKey(SettingsRegistryPath))
            {
                settingsKey?.SetValue(EverythingPathValueName, everythingPath, RegistryValueKind.String);
            }
        }

        private void SavePlaceholderText(string placeholderText)
        {
            using (RegistryKey settingsKey = Registry.CurrentUser.CreateSubKey(SettingsRegistryPath))
            {
                settingsKey?.SetValue(PlaceholderTextValueName, placeholderText ?? string.Empty, RegistryValueKind.String);
            }
        }

        private void UpdateOpenSearchBoxPlaceholders()
        {
            foreach (ToolStripItem item in searchBoxes)
            {
                var host = item as ToolStripControlHost;
                var searchBox = host?.Control as CustomFilterBox;
                searchBox?.UpdatePlaceholderText(this.placeholderText);
            }
        }

        private string ShowPlaceholderOptionsDialog(string currentPlaceholder)
        {
            using (var form = new Form())
            using (var label = new Label())
            using (var textBox = new TextBox())
            using (var resetButton = new Button())
            using (var okButton = new Button())
            using (var cancelButton = new Button())
            {
                form.Text = "Search Box Options";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.MaximizeBox = false;
                form.MinimizeBox = false;
                form.ShowInTaskbar = false;
                form.ClientSize = new System.Drawing.Size(360, 120);

                label.AutoSize = true;
                label.Left = 12;
                label.Top = 15;
                label.Text = "Placeholder text (leave empty for no placeholder):";

                textBox.Left = 12;
                textBox.Top = 38;
                textBox.Width = 336;
                textBox.Text = currentPlaceholder ?? string.Empty;

                resetButton.Text = "Reset";
                resetButton.Left = 12;
                resetButton.Top = 76;
                resetButton.Width = 75;
                resetButton.Click += (sender, e) => textBox.Text = DefaultPlaceholderText;

                okButton.Text = "OK";
                okButton.Left = 192;
                okButton.Top = 76;
                okButton.Width = 75;
                okButton.DialogResult = DialogResult.OK;

                cancelButton.Text = "Cancel";
                cancelButton.Left = 273;
                cancelButton.Top = 76;
                cancelButton.Width = 75;
                cancelButton.DialogResult = DialogResult.Cancel;

                form.Controls.Add(label);
                form.Controls.Add(textBox);
                form.Controls.Add(resetButton);
                form.Controls.Add(okButton);
                form.Controls.Add(cancelButton);
                form.AcceptButton = okButton;
                form.CancelButton = cancelButton;

                return form.ShowDialog() == DialogResult.OK ? textBox.Text ?? string.Empty : null;
            }
        }

        private string PromptForEverythingExecutable()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Locate Everything executable";
                dialog.Filter = "Everything executable|Everything*.exe|Executable files|*.exe|All files|*.*";
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;
                dialog.Multiselect = false;
                dialog.FileName = "Everything64.exe";

                DialogResult result = dialog.ShowDialog();
                if (result != DialogResult.OK)
                {
                    return null;
                }

                string selectedPath = dialog.FileName;
                if (!IsSupportedEverythingExecutable(selectedPath))
                {
                    MessageBox.Show(
                        "Please select Everything.exe or Everything64.exe.",
                        "Invalid selection",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return null;
                }

                SaveEverythingPath(selectedPath);
                return selectedPath;
            }
        }

        private bool IsSupportedEverythingExecutable(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return false;
            }

            string fileName = Path.GetFileName(path);
            return string.Equals(fileName, "Everything.exe", StringComparison.OrdinalIgnoreCase)
                || string.Equals(fileName, "Everything64.exe", StringComparison.OrdinalIgnoreCase);
        }

        private bool TryStartEverything(string everythingPath, string arguments)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = everythingPath,
                    Arguments = arguments,
                    UseShellExecute = true,
                };
                Process.Start(startInfo);
                SaveEverythingPath(everythingPath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error launching Everything from '{everythingPath}': {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        // Helper method to escape the query argument
        private string EscapeArgument(string arg)
        {
            if (string.IsNullOrEmpty(arg))
                return "\"\"";
            return $"\"{arg.Replace("\"", "\\\"")}\"";
        }

        #region Plugin Information and Uninstall

        public static void Uninstall()
        {
            try
            {
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\QuizoPlugins\SearchBoxPlugin", true))
                {
                    if (rk != null)
                    {
                        rk.DeleteSubKeyTree("SearchBoxPlugin");
                    }
                }
            }
            catch
            {
                // Ignore any errors during uninstall cleanup
            }
        }

        #endregion
    }

    // Localizer class for plugin metadata
    sealed class Localizer : LocalizedStringProvider
    {
        public override string Author
        {
            get
            {
                return "Your Name";
            }
        }

        public override string Description
        {
            get
            {
                return "Adds a search box to the toolbar to search using Everything.";
            }
        }

        public override string Name
        {
            get
            {
                return "Search Box Plugin";
            }
        }

        public override void SetKey(int iKey)
        {
            // Not used in this plugin
        }
    }
}
