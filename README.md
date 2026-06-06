# Everything Search Box for QTTabBar

A QTTabBar plugin that adds an Everything-powered search box to the Explorer toolbar.

Type a query, press `Enter`, and the plugin opens Everything scoped to the current Explorer tab's path.

![Plugin options](docs/search-box-options.png)

## Features

- toolbar search box for QTTabBar
- optional icon shown inside the toolbar item
- bundled Everything icon set, selectable from plugin options
- configurable placeholder text
- configurable Everything executable path
- configurable Everything argument template with `{path}` and `{query}` placeholders
- supports empty placeholder text
- prefers a running Everything instance when available
- falls back to auto-detecting `Everything.exe` / `Everything64.exe`
- lets the user browse to the Everything executable if auto-detection fails
- dark-mode-aware search box colors
- live toolbar icon preview when changing the icon option

## Current Version

- Plugin name: `Search Box Plugin`
- Author: `HamzaETTH`
- Version: `1.3.0.0`

## Requirements

- Windows
- [QTTabBar](https://github.com/indiff/qttabbar)
- Everything from voidtools
- .NET Framework 4.8 runtime

## Install

1. Build `EverythingSearchBox.dll` or use the compiled Release DLL.
2. Copy `EverythingSearchBox.dll` into your QTTabBar plugin directory.
3. Open QTTabBar options and enable third-party plugins if needed.
4. Add `Search Box Plugin` to the toolbar.
5. Restart Explorer/QTTabBar if the new plugin does not appear immediately.

## Usage

- Click into the search box.
- Type a query.
- Press `Enter`.

The plugin sends this search to Everything:

```text
-sort size -nomaximized -path "<current tab path>" -s "<query>"
```

This launch template is configurable in plugin options.

The current path is resolved in this order:

1. active QTTabBar tab path
2. plugin server path
3. current process directory

## Everything Detection

The plugin uses this fallback order:

1. running Everything instance via IPC
2. saved user-selected Everything path
3. Windows `App Paths` registry entries
4. common install folders
5. browse prompt to select `Everything.exe` or `Everything64.exe`

Saved settings are stored here:

```text
HKEY_CURRENT_USER\Software\QuizoPlugins\SearchBoxPlugin
```

Values used:

- `EverythingPath`
- `PlaceholderText`
- `IconName`
- `ArgumentsTemplate`

## Options

The plugin has an options dialog in QTTabBar.

You can:

- change the placeholder text
- leave the placeholder empty
- view and override the Everything executable path
- browse to a different Everything executable
- auto-detect the Everything executable path again
- change the Everything arguments template
- use `{path}` for the current folder and `{query}` for the search text
- reset placeholder and icon to defaults
- choose a bundled Everything icon
- open the official Everything command-line documentation from the options dialog

Open toolbar items update immediately when changing the icon setting in the options dialog.

## Build

Open `EverythingSearchBox.sln` in Visual Studio and build `Release`, or run:

```powershell
rtk proxy "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" EverythingSearchBox.sln /p:Configuration=Release
```

Built DLL:

```text
EverythingSearchBox\bin\Release\EverythingSearchBox.dll
```

## Project Layout

- `EverythingSearchBox/SearchBoxPlugin.cs`: main plugin logic
- `EverythingSearchBox/CustomFilterBox.cs`: themed search textbox with cue-banner placeholder
- `EverythingSearchBox/SearchBoxToolbarControl.cs`: toolbar control that shows icon + search box
- `EverythingSearchBox/Localizer.cs`: QTTabBar plugin metadata
- `EverythingSearchBox/Resource.resx`: plugin icon resources used by QTTabBar
- `docs/search-box-options.png`: screenshot of the plugin options dialog
- `icons/`: bundled Everything `.ico` files
- `QTTabBarPluginExamples/`: local reference examples from QTTabBar plugin samples

## Notes

- QTTabBar expects plugin-page icon resources to match the plugin class name. This plugin provides `SearchBoxPlugin_small` and `SearchBoxPlugin_large` for that reason.
- If a toolbar item was previously given a manual icon inside QTTabBar, use `Reset Icon` on that item to return to the plugin-provided icon.
- QTTabBar or Explorer may need a restart after replacing the DLL.

## Credits

- Everything by voidtools
- QTTabBar by Quizo and later community maintainers
- bundled icons sourced from the voidtools forum thread `Everything Icon Pack`: https://www.voidtools.com/forum/viewtopic.php?t=5873
- icon credit goes to their creator(s)
