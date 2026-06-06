using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using QTPlugin;
using QTPlugin.Interop;

namespace QuizoPlugins
{
    [Plugin(PluginType.BackgroundMultiple, typeof(Localizer), Version = "1.3.0.0")]
    public class SearchBoxPlugin : IBarMultipleCustomItems
    {
        private const int SW_SHOWNORMAL = 1;
        private const int WM_COPYDATA = 0x004A;
        private const int SMTO_ABORTIFHUNG = 0x0002;
        private const string SettingsRegistryPath = @"Software\QuizoPlugins\SearchBoxPlugin";
        private const string EverythingPathValueName = "EverythingPath";
        private const string PlaceholderTextValueName = "PlaceholderText";
        private const string IconNameValueName = "IconName";
        private const string ArgumentsTemplateValueName = "ArgumentsTemplate";
        private const string DefaultPlaceholderText = "Search...";
        private const string DefaultIconName = "voidtools-01-Everything-Orange.ico";
        private const string DefaultArgumentsTemplate = "-sort size -nomaximized -path {path} -s {query}";
        private const string EverythingCommandLineOptionsUrl = "https://www.voidtools.com/support/everything/command_line_options/";
        private const string EverythingTaskbarNotificationWindowClass = "EVERYTHING_TASKBAR_NOTIFICATION";
        private static readonly IntPtr EverythingCommandLineCopyData = IntPtr.Zero;
        private static readonly string[] AvailableIconNames =
        {
            "voidtools-01-Everything-Orange.ico",
            "voidtools-02-Everything-Yellow.ico",
            "voidtools-03-Everything-Chartreuse.ico",
            "voidtools-04-Everything-Green.ico",
            "voidtools-05-Everything-SpringGreen.ico",
            "voidtools-06-Everything-Cyan.ico",
            "voidtools-07-Everything-SkyBlue.ico",
            "voidtools-08-Everything-Blue.ico",
            "voidtools-09-Everything-Purple.ico",
            "voidtools-10-Everything-Magenta.ico",
            "voidtools-11-Everything-Pink.ico",
            "voidtools-12-Everything-Red.ico",
            "voidtools-13-Everything-Grey.ico",
            "voidtools-14-Everything-White.ico",
            "voidtools-15-Everything-1.5.ico",
            "MMK-Everything-FlatIcon.ico",
            "MMK-Everything-Icon3D.ico",
            "MMK-Everything-Icon3DShadow.ico",
            "LeroyXie-01.ico",
            "LeroyXie-02.ico",
            "LeroyXie-03.ico",
            "LeroyXie-04.ico",
            "LeroyXie-05.ico",
            "LeroyXie-06.ico",
            "LeroyXie-07.ico",
            "LeroyXie-08.ico",
            "Eagleeyez-Everything.ico"
        };

        private readonly List<ToolStripItem> searchBoxes = new List<ToolStripItem>();
        private IPluginServer pluginServer;
        private string placeholderText = DefaultPlaceholderText;
        private string iconName = DefaultIconName;
        private string argumentsTemplate = DefaultArgumentsTemplate;

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
            this.pluginServer = pluginServer;
            placeholderText = LoadPlaceholderText();
            iconName = LoadIconName();
            argumentsTemplate = LoadArgumentsTemplate();
        }

        public void Close(EndCode endCode)
        {
            foreach (ToolStripItem searchBox in searchBoxes)
            {
                searchBox.Dispose();
            }

            searchBoxes.Clear();
        }

        public bool HasOption => true;

        public void OnMenuItemClick(MenuType menuType, string menuText, ITab tab) { }

        public void OnOption()
        {
            PluginOptions updatedOptions = ShowPluginOptionsDialog(placeholderText, iconName, ResolveEverythingPath(), argumentsTemplate);
            if (updatedOptions == null)
            {
                return;
            }

            placeholderText = updatedOptions.PlaceholderText;
            iconName = updatedOptions.IconName;
            argumentsTemplate = updatedOptions.ArgumentsTemplate;
            SaveEverythingPath(updatedOptions.EverythingPath);
            SavePlaceholderText(placeholderText);
            SaveIconName(iconName);
            SaveArgumentsTemplate(argumentsTemplate);
            UpdateOpenSearchBoxPlaceholders();
            UpdateOpenSearchBoxIcons();
        }

        public void OnShortcutKeyPressed(int index) { }

        public bool QueryShortcutKeys(out string[] actions)
        {
            actions = null;
            return false;
        }

        #endregion

        #region IBarMultipleCustomItems members

        public void Initialize(int[] order)
        {
            foreach (ToolStripItem searchBox in searchBoxes)
            {
                searchBox.Dispose();
            }

            searchBoxes.Clear();
        }

        public ToolStripItem CreateItem(bool fLarge, DisplayStyle displayStyle, int index)
        {
            Image toolbarIcon = GetToolbarItemIcon(fLarge);
            var toolbarControl = new SearchBoxToolbarControl(placeholderText, toolbarIcon, fLarge);
            toolbarControl.QuerySubmitted += (sender, query) => RunEverythingSearch(query);

            var controlHost = new ToolStripControlHost(toolbarControl)
            {
                AutoSize = false,
                Width = toolbarControl.Width,
                Height = toolbarControl.Height
            };

            searchBoxes.Add(controlHost);
            return controlHost;
        }

        public int Count => -1;

        public System.Drawing.Image GetImage(bool fLarge, int index)
        {
            Image defaultImage = GetDefaultPluginImage(fLarge);
            Image selectedIcon = TryLoadSelectedPluginIcon(fLarge ? 24 : 16);
            return selectedIcon ?? defaultImage;
        }

        public string GetName(int index)
        {
            return "Search Box";
        }

        #endregion

        private string GetCurrentDirectory()
        {
            string currentDirectory = null;

            if (pluginServer != null && pluginServer.SelectedTab != null)
            {
                currentDirectory = pluginServer.SelectedTab.Path;
                if (!string.IsNullOrEmpty(currentDirectory))
                {
                    return currentDirectory;
                }
            }

            if (string.IsNullOrEmpty(currentDirectory))
            {
                currentDirectory = pluginServer?.Path;
                if (!string.IsNullOrEmpty(currentDirectory))
                {
                    return currentDirectory;
                }
            }

            return Environment.CurrentDirectory;
        }

        private void RunEverythingSearch(string query)
        {
            string currentDirectory = GetCurrentDirectory();
            if (string.IsNullOrEmpty(currentDirectory))
            {
                MessageBox.Show("Could not retrieve current directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string arguments = BuildEverythingArguments(currentDirectory, query);
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
            return ResolveEverythingPath(includeSavedPath: true);
        }

        private string ResolveAutoDetectedEverythingPath()
        {
            return ResolveEverythingPath(includeSavedPath: false);
        }

        private string ResolveEverythingPath(bool includeSavedPath)
        {
            var seenPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string candidate in GetEverythingCandidates(includeSavedPath))
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

                if (IsSupportedEverythingExecutable(fullPath))
                {
                    return fullPath;
                }
            }

            return null;
        }

        private IEnumerable<string> GetEverythingCandidates(bool includeSavedPath)
        {
            if (includeSavedPath)
            {
                string savedPath = LoadSavedEverythingPath();
                if (!string.IsNullOrWhiteSpace(savedPath))
                {
                    yield return savedPath;
                }
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

        private string LoadIconName()
        {
            using (RegistryKey settingsKey = Registry.CurrentUser.OpenSubKey(SettingsRegistryPath, false))
            {
                string configuredIconName = settingsKey?.GetValue(IconNameValueName, DefaultIconName) as string;
                return AvailableIconNames.Contains(configuredIconName, StringComparer.OrdinalIgnoreCase)
                    ? configuredIconName
                    : DefaultIconName;
            }
        }

        private string LoadArgumentsTemplate()
        {
            using (RegistryKey settingsKey = Registry.CurrentUser.OpenSubKey(SettingsRegistryPath, false))
            {
                return settingsKey?.GetValue(ArgumentsTemplateValueName, DefaultArgumentsTemplate) as string ?? DefaultArgumentsTemplate;
            }
        }

        private void SaveEverythingPath(string everythingPath)
        {
            using (RegistryKey settingsKey = Registry.CurrentUser.CreateSubKey(SettingsRegistryPath))
            {
                if (settingsKey == null)
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(everythingPath))
                {
                    settingsKey.DeleteValue(EverythingPathValueName, false);
                    return;
                }

                settingsKey.SetValue(EverythingPathValueName, everythingPath, RegistryValueKind.String);
            }
        }

        private void SavePlaceholderText(string placeholderText)
        {
            using (RegistryKey settingsKey = Registry.CurrentUser.CreateSubKey(SettingsRegistryPath))
            {
                settingsKey?.SetValue(PlaceholderTextValueName, placeholderText ?? string.Empty, RegistryValueKind.String);
            }
        }

        private void SaveIconName(string iconName)
        {
            using (RegistryKey settingsKey = Registry.CurrentUser.CreateSubKey(SettingsRegistryPath))
            {
                settingsKey?.SetValue(IconNameValueName, iconName ?? DefaultIconName, RegistryValueKind.String);
            }
        }

        private void SaveArgumentsTemplate(string template)
        {
            using (RegistryKey settingsKey = Registry.CurrentUser.CreateSubKey(SettingsRegistryPath))
            {
                settingsKey?.SetValue(ArgumentsTemplateValueName, string.IsNullOrWhiteSpace(template) ? DefaultArgumentsTemplate : template, RegistryValueKind.String);
            }
        }

        private void UpdateOpenSearchBoxPlaceholders()
        {
            foreach (ToolStripItem item in searchBoxes)
            {
                ToolStripControlHost host = item as ToolStripControlHost;
                SearchBoxToolbarControl toolbarControl = host?.Control as SearchBoxToolbarControl;
                toolbarControl?.UpdatePlaceholderText(placeholderText);
            }
        }

        private void UpdateOpenSearchBoxIcons()
        {
            foreach (ToolStripItem item in searchBoxes)
            {
                ToolStripControlHost host = item as ToolStripControlHost;
                SearchBoxToolbarControl toolbarControl = host?.Control as SearchBoxToolbarControl;
                if (toolbarControl == null)
                {
                    continue;
                }

                toolbarControl.UpdateIcon(GetToolbarItemIcon(toolbarControl.IsLargeLayout));
            }
        }

        private PluginOptions ShowPluginOptionsDialog(string currentPlaceholder, string currentIconName, string currentEverythingPath, string currentArgumentsTemplate)
        {
            string originalIconName = iconName;

            using (var form = new Form())
            using (var label = new Label())
            using (var placeholderTextBox = new TextBox())
            using (var pathLabel = new Label())
            using (var pathTextBox = new TextBox())
            using (var browseButton = new Button())
            using (var autoDetectButton = new Button())
            using (var argumentsLabel = new Label())
            using (var argumentsTextBox = new TextBox())
            using (var argumentsHelpLink = new LinkLabel())
            using (var iconLabel = new Label())
            using (var iconComboBox = new ComboBox())
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
                form.ClientSize = new System.Drawing.Size(520, 370);

                label.AutoSize = true;
                label.Left = 12;
                label.Top = 15;
                label.Text = "Placeholder text (leave empty for no placeholder):";

                placeholderTextBox.Left = 12;
                placeholderTextBox.Top = 38;
                placeholderTextBox.Width = 416;
                placeholderTextBox.Text = currentPlaceholder ?? string.Empty;

                pathLabel.AutoSize = true;
                pathLabel.Left = 12;
                pathLabel.Top = 72;
                pathLabel.Text = "Everything executable path:";

                pathTextBox.Left = 12;
                pathTextBox.Top = 95;
                pathTextBox.Width = 244;
                pathTextBox.Text = currentEverythingPath ?? string.Empty;

                browseButton.Text = "Browse...";
                browseButton.Left = 262;
                browseButton.Top = 93;
                browseButton.Width = 80;
                browseButton.Click += (sender, e) =>
                {
                    string selectedPath = PromptForEverythingExecutable(saveSelection: false);
                    if (!string.IsNullOrEmpty(selectedPath))
                    {
                        pathTextBox.Text = selectedPath;
                    }
                };

                autoDetectButton.Text = "Auto-detect";
                autoDetectButton.Left = 348;
                autoDetectButton.Top = 93;
                autoDetectButton.Width = 80;
                autoDetectButton.Click += (sender, e) =>
                {
                    string detectedPath = ResolveAutoDetectedEverythingPath();
                    if (string.IsNullOrEmpty(detectedPath))
                    {
                        MessageBox.Show(
                            "Could not auto-detect an Everything executable.",
                            "Search Box Options",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        return;
                    }

                    pathTextBox.Text = detectedPath;
                };

                argumentsLabel.AutoSize = true;
                argumentsLabel.Left = 12;
                argumentsLabel.Top = 130;
                argumentsLabel.Text = "Everything arguments template ({path} = current folder, {query} = search text):";

                argumentsTextBox.Left = 12;
                argumentsTextBox.Top = 153;
                argumentsTextBox.Width = 496;
                argumentsTextBox.Height = 72;
                argumentsTextBox.Multiline = true;
                argumentsTextBox.ScrollBars = ScrollBars.Vertical;
                argumentsTextBox.Text = string.IsNullOrWhiteSpace(currentArgumentsTemplate) ? DefaultArgumentsTemplate : currentArgumentsTemplate;

                argumentsHelpLink.AutoSize = true;
                argumentsHelpLink.Left = 12;
                argumentsHelpLink.Top = 230;
                argumentsHelpLink.Text = "Everything command line options";
                argumentsHelpLink.LinkClicked += (sender, e) =>
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = EverythingCommandLineOptionsUrl,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Could not open the documentation link: {ex.Message}",
                            "Search Box Options",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                };

                iconLabel.AutoSize = true;
                iconLabel.Left = 12;
                iconLabel.Top = 259;
                iconLabel.Text = "Toolbar icon:";

                iconComboBox.Left = 12;
                iconComboBox.Top = 282;
                iconComboBox.Width = 496;
                iconComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                iconComboBox.Items.AddRange(AvailableIconNames);
                iconComboBox.SelectedItem = AvailableIconNames.Contains(currentIconName, StringComparer.OrdinalIgnoreCase)
                    ? currentIconName
                    : DefaultIconName;
                iconComboBox.SelectedIndexChanged += (sender, e) =>
                {
                    iconName = iconComboBox.SelectedItem as string ?? DefaultIconName;
                    UpdateOpenSearchBoxIcons();
                };

                resetButton.Text = "Reset";
                resetButton.Left = 12;
                resetButton.Top = 311;
                resetButton.Width = 75;
                resetButton.Click += (sender, e) =>
                {
                    placeholderTextBox.Text = DefaultPlaceholderText;
                    pathTextBox.Text = ResolveAutoDetectedEverythingPath() ?? string.Empty;
                    argumentsTextBox.Text = DefaultArgumentsTemplate;
                    iconComboBox.SelectedItem = DefaultIconName;
                };

                okButton.Text = "OK";
                okButton.Left = 352;
                okButton.Top = 311;
                okButton.Width = 75;
                okButton.Click += (sender, e) =>
                {
                    string selectedPath = (pathTextBox.Text ?? string.Empty).Trim();
                    string selectedArgumentsTemplate = (argumentsTextBox.Text ?? string.Empty).Trim();
                    if (!string.IsNullOrEmpty(selectedPath) && !IsSupportedEverythingExecutable(selectedPath))
                    {
                        MessageBox.Show(
                            "Please select a valid Everything executable (Everything.exe or Everything64.exe), or leave the field empty to use automatic detection.",
                            "Search Box Options",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        pathTextBox.Focus();
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(selectedArgumentsTemplate))
                    {
                        MessageBox.Show(
                            "Please provide an arguments template.",
                            "Search Box Options",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        argumentsTextBox.Focus();
                        return;
                    }

                    form.DialogResult = DialogResult.OK;
                    form.Close();
                };

                cancelButton.Text = "Cancel";
                cancelButton.Left = 433;
                cancelButton.Top = 311;
                cancelButton.Width = 75;
                cancelButton.DialogResult = DialogResult.Cancel;

                form.Controls.Add(label);
                form.Controls.Add(placeholderTextBox);
                form.Controls.Add(pathLabel);
                form.Controls.Add(pathTextBox);
                form.Controls.Add(browseButton);
                form.Controls.Add(autoDetectButton);
                form.Controls.Add(argumentsLabel);
                form.Controls.Add(argumentsTextBox);
                form.Controls.Add(argumentsHelpLink);
                form.Controls.Add(iconLabel);
                form.Controls.Add(iconComboBox);
                form.Controls.Add(resetButton);
                form.Controls.Add(okButton);
                form.Controls.Add(cancelButton);
                form.AcceptButton = okButton;
                form.CancelButton = cancelButton;

                DialogResult result = form.ShowDialog();
                if (result != DialogResult.OK)
                {
                    iconName = originalIconName;
                    UpdateOpenSearchBoxIcons();
                    return null;
                }

                return new PluginOptions(
                    placeholderTextBox.Text ?? string.Empty,
                    iconComboBox.SelectedItem as string ?? DefaultIconName,
                    (pathTextBox.Text ?? string.Empty).Trim(),
                    (argumentsTextBox.Text ?? string.Empty).Trim());
            }
        }

        private string PromptForEverythingExecutable()
        {
            return PromptForEverythingExecutable(saveSelection: true);
        }

        private string PromptForEverythingExecutable(bool saveSelection)
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

                if (saveSelection)
                {
                    SaveEverythingPath(selectedPath);
                }

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
                    UseShellExecute = true
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

        private string BuildEverythingArguments(string currentDirectory, string query)
        {
            string template = string.IsNullOrWhiteSpace(argumentsTemplate) ? DefaultArgumentsTemplate : argumentsTemplate;
            return template
                .Replace("{path}", EscapeCommandLineArgument(currentDirectory ?? string.Empty))
                .Replace("{query}", EscapeCommandLineArgument(query ?? string.Empty));
        }

        private string EscapeCommandLineArgument(string arg)
        {
            if (arg == null)
            {
                return "\"\"";
            }

            bool needsQuotes = arg.Length == 0 || arg.IndexOfAny(new[] { ' ', '\t', '\n', '\v', '"' }) >= 0;
            if (!needsQuotes)
            {
                return arg;
            }

            var builder = new StringBuilder(arg.Length + 2);
            builder.Append('"');
            int backslashCount = 0;

            foreach (char ch in arg)
            {
                if (ch == '\\')
                {
                    backslashCount++;
                    continue;
                }

                if (ch == '"')
                {
                    builder.Append('\\', backslashCount * 2 + 1);
                    builder.Append('"');
                    backslashCount = 0;
                    continue;
                }

                if (backslashCount > 0)
                {
                    builder.Append('\\', backslashCount);
                    backslashCount = 0;
                }

                builder.Append(ch);
            }

            if (backslashCount > 0)
            {
                builder.Append('\\', backslashCount * 2);
            }

            builder.Append('"');
            return builder.ToString();
        }

        private static Image GetDefaultPluginImage(bool fLarge)
        {
            return fLarge ? Resource.SearchBoxPlugin_large : Resource.SearchBoxPlugin_small;
        }

        private Image GetToolbarItemIcon(bool fLarge)
        {
            Image defaultImage = GetDefaultPluginImage(fLarge);
            Image selectedIcon = TryLoadSelectedPluginIcon(fLarge ? 24 : 16);
            return selectedIcon ?? defaultImage;
        }

        private Image TryLoadSelectedPluginIcon(int size)
        {
            if (!AvailableIconNames.Contains(iconName, StringComparer.OrdinalIgnoreCase) ||
                string.Equals(iconName, DefaultIconName, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            try
            {
                string resourceName = typeof(SearchBoxPlugin).Assembly
                    .GetManifestResourceNames()
                    .FirstOrDefault(name => name.EndsWith("." + iconName, StringComparison.OrdinalIgnoreCase));

                if (resourceName == null)
                {
                    return null;
                }

                Assembly assembly = typeof(SearchBoxPlugin).Assembly;
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (Icon icon = stream != null ? new Icon(stream, new Size(size, size)) : null)
                {
                    if (icon == null)
                    {
                        return null;
                    }

                    Bitmap bitmap = new Bitmap(size, size);
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Clear(Color.Transparent);
                        graphics.DrawIcon(icon, new Rectangle(0, 0, size, size));
                    }

                    return bitmap;
                }
            }
            catch
            {
                return null;
            }
        }

        #region Plugin Information and Uninstall

        public static void Uninstall()
        {
            try
            {
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\QuizoPlugins", true))
                {
                    if (rk != null)
                    {
                        rk.DeleteSubKeyTree("SearchBoxPlugin");
                    }
                }
            }
            catch
            {
                // Ignore any errors during uninstall cleanup.
            }
        }

        #endregion

        private sealed class PluginOptions
        {
            public PluginOptions(string placeholderText, string iconName, string everythingPath, string argumentsTemplate)
            {
                PlaceholderText = placeholderText;
                IconName = iconName;
                EverythingPath = everythingPath;
                ArgumentsTemplate = argumentsTemplate;
            }

            public string PlaceholderText { get; }

            public string IconName { get; }

            public string EverythingPath { get; }

            public string ArgumentsTemplate { get; }
        }
    }
}
