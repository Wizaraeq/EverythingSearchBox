# AGENTS.md

This file provides guidance to AI agents working in this repository.

## What this is

A QTTabBar plugin (`EverythingSearchBox.dll`) that adds a toolbar search box to Windows Explorer via QTTabBar. When the user types a query and presses Enter, the plugin opens an Everything search scoped to the currently active tab's directory.

## Build

Open `EverythingSearchBox.sln` in Visual Studio 2017+ and build, or from the command line:

```bash
rtk proxy "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" EverythingSearchBox.sln /p:Configuration=Release
```

Output: `EverythingSearchBox\bin\Release\EverythingSearchBox.dll`

To install, copy `EverythingSearchBox.dll` into QTTabBar's plugin directory and enable it from QTTabBar options.

## Key dependency

`QTPluginLib.dll` is resolved from the Windows GAC:
`C:\Windows\assembly\GAC_MSIL\QTPluginLib\1.0.0.0__78a0cde69b47ca25\QTPluginLib.dll`

A local copy is also kept at `EverythingSearchBox\lib\QTPluginLib.dll`, but the project references the GAC version. QTTabBar installs the required GAC assembly.

## Architecture

There are three main source files:

| File | Purpose |
|------|---------|
| `SearchBoxPlugin.cs` | Main plugin class. Implements `IBarMultipleCustomItems`, creates `CustomFilterBox` instances, handles Enter key submission, shows plugin options, and launches or forwards searches to Everything. |
| `CustomFilterBox.cs` | `TextBox` subclass with configurable native cue-banner placeholder text, fixed 85x20 size, and live dark/light mode detection from `AppsUseLightTheme`. |
| `Localizer.cs` | `LocalizedStringProvider` subclass for plugin metadata shown by QTTabBar. |

## Plugin lifecycle

- `Open()` stores the `IPluginServer` reference and loads persisted settings such as placeholder text.
- `CreateItem()` instantiates a `ToolStripControlHost` wrapping a `CustomFilterBox`.
- `OnOption()` opens the plugin's options dialog.
- `Close()` disposes toolbar items.

## Search launch behavior

`RunEverythingSearch()` uses this fallback order:

1. Try to send the search command line to a running Everything instance via the `EVERYTHING_TASKBAR_NOTIFICATION` window and `WM_COPYDATA`.
2. Try a saved user-selected executable path from `HKCU\Software\QuizoPlugins\SearchBoxPlugin`.
3. Try registry-discovered `App Paths` entries for `Everything64.exe` / `Everything.exe`.
4. Try common install locations under `Program Files` / `Program Files (x86)` for both `Everything 1.5a` and `Everything`.
5. If no valid executable is found, prompt the user to browse to `Everything.exe` or `Everything64.exe` and persist that choice.

The command line passed to Everything is:

```text
-sort size -nomaximized -path "<current tab path>" -s "<query>"
```

`GetCurrentDirectory()` falls back in this order:

1. `pluginServer.SelectedTab.Path`
2. `pluginServer.Path`
3. `Environment.CurrentDirectory`

## Placeholder / options behavior

- The plugin now has options: `HasOption` is `true`.
- `OnOption()` opens a small WinForms dialog that lets the user:
  - set custom placeholder text
  - leave it empty for no placeholder
  - reset it to the default `Search...`
  - choose a bundled toolbar icon from the embedded Everything icon set
- Placeholder text is stored in `HKCU\Software\QuizoPlugins\SearchBoxPlugin` under `PlaceholderText`.
- The Everything executable override is stored in the same registry key under `EverythingPath`.
- The selected bundled toolbar icon is stored in the same registry key under `IconName`.
- Existing open toolbar search boxes are updated immediately after the placeholder setting changes.
- Placeholder text is implemented as a native cue banner, not as actual `TextBox.Text`.

## Toolbar icon behavior

- The plugin toolbar icon is bundled into the DLL from the repo-level `icons\` folder as embedded `.ico` resources.
- `GetImage()` returns a cached 16px or 24px bitmap rendered from the selected embedded icon.
- The default icon is `voidtools-01-Everything-Orange.ico`.

## Dark mode

`CustomFilterBox` reads `HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize\AppsUseLightTheme` at construction time and on `SystemEvents.UserPreferenceChanged` to update background and foreground colors.

Always unsubscribe from `SystemEvents.UserPreferenceChanged` in `Dispose()` to avoid leaks.

## Local example plugins

Use `QTTabBarPluginExamples` as the local reference set for QTTabBar plugin patterns.

Especially useful examples:

- `QTTabBarPluginExamples\SamplePlugin` for baseline plugin structure
- `QTTabBarPluginExamples\Spacer` for `HasOption` + `OnOption()` + small WinForms options dialog
- `QTTabBarPluginExamples\WindowManager` for larger plugin option flows

Prefer those local examples over searching externally when you need to confirm the expected QTTabBar plugin pattern.
