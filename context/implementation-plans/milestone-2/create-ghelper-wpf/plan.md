---
name: plan-create-ghelper-wpf
description: Implementation plan for Milestone 2 Story 6 — standing up the GHelper.WPF executable head, starting from a minimal shell that proves it can reference and run shared logic.
metadata:
  version: "1.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---
# Story 6 — Create `G-Helper.WPF`

[Milestone 2](../../../milestones/milestone-2.md#story-6) · [design.md](../../../design.md#how-the-pieces-fit-together)

## Objective

A second executable head that builds, references `GHelper.Shared`, launches, and eventually behaves as a tray application — so Milestone 3's overlay work has a running project to build into.

## Estimates

Small. The shell is a handful of files; the tray work is the real content of this story.

## Steps

| # | Step | Verify | Status |
| --- | --- | --- | --- |
| 1 | Minimal WPF project referencing `GHelper.Shared`: `App.xaml`, one placeholder window | Builds, launches, window visible, shared calls return real values | Done |
| 2 | Tray-icon bootstrap and tray-resident lifetime (show/hide, context menu, single-instance) | App survives window close and reopens from the tray | Done |
| 3 | Application icon and manifest | Correct icon in tray and taskbar | Done |
| 4 | Initialise real shared services from this process rather than read-only probes | Services start without the WinForms head running | Done, as far as the shared layer allows — see the execution log |

## Decisions

**Tray icon: `System.Windows.Forms.NotifyIcon`, not a hand-rolled shell wrapper.** The WPF head sets `UseWindowsForms=True` purely to reach `NotifyIcon`; no WinForms window or control is ever created. The alternatives were a third-party WPF tray package (ruled out by [laws.md](../../../laws.md) §3 — no new dependencies without cause) or ~150 lines of `Shell_NotifyIcon` P/Invoke owning taskbar-restart, DPI and popup-dismissal edge cases for no functional gain at this stage. `TrayIcon` is a sealed wrapper that raises intent events and holds no application state, so replacing the mechanism later is a one-file change. Side effect worth knowing: enabling WinForms drops `System.IO` and `System.Net.Http` from the project's implicit usings, and `System.Windows.Forms` / `System.Drawing` are removed from them deliberately so `System.Windows.*` stays unambiguous in the WPF code.

**Single instance: the running instance wins.** `SingleInstance` owns a session-local named `EventWaitHandle`; a later launch signals it and quits, and the owner shows its window. The WinForms head's `ProcessHelper.CheckAlreadyRunning` does the reverse — a global event that makes the *new* instance replace the old one, killing the previous process. Activation is the behaviour asked for here, and staying session-local (`Local\`) means the WPF head can never collide with a G-Helper instance running as SYSTEM or in another user's session.

**Icons follow the WinForms head's split.** `favicon.ico` (64/128/256 frames) is the executable icon; `Resources/standard.ico` (32/24/16 frames) is the tray icon, loaded at `SystemInformation.SmallIconSize` so it stays crisp. Both are copies of the existing `app/` art, not new artwork. The mode-tinted variants (`eco`, `ultimate`, …) are not used yet because the WPF head has no performance-mode state to reflect.

## Validation

- `dotnet build G-Helper.WPF.sln -c Debug` → 0 errors, 0 warnings.
- Launch `GHelper.WPF\bin\x64\Debug\net10.0-windows\GHelper.WPF.exe` and confirm the shared layer resolves at runtime.
- Never run `GHelper.exe` and `GHelper.WPF.exe` at the same time: they share `%APPDATA%\GHelper\log.txt` and settings.

## Notes

The placeholder window deliberately carries no view model. It exists to prove the process boundary, and it will be deleted rather than grown — MVVM structure arrives with the first real view in Milestone 3.

## Execution Log

**Step 1 (2026-09-12).** Created `GHelper.WPF` (`net10.0-windows`, x64, `UseWPF`) with `App.xaml`, `App.xaml.cs`, `MainWindow.xaml`, `MainWindow.xaml.cs`, and a project reference to `GHelper.Shared`. The window reports the shared assembly identity, `Logger.logFile`, `UserIdentity.IsRunningAsSystem()`, and `DeviceHelper.GetGpuError()`, and the process appends to the shared log — confirming shared logic initialises from a second process. Steps 2-4 are untouched, so Story 6 is not close to complete.

**Steps 2-4 (2026-09-12).** Made the head tray-resident. `App` now drives the whole lifetime: it claims the single-instance slot, logs startup through the shared `Logger`, creates the window without showing it, and puts up the tray icon; `ShutdownMode` is `OnExplicitShutdown`, so only the tray's Exit item ends the process. Added `TrayIcon` (icon, tooltip, left-click toggle, "Open G-Helper" / "Exit" menu) and `SingleInstance` (named event, activate-the-owner). `MainWindow` cancels its own close and hides instead, and no longer writes the startup log line — lifetime logging belongs to `App`. Added `app.manifest` (asInvoker, Windows 10/11, themed common controls) and `favicon.ico` / `Resources/standard.ico` copied from `app/`.

Step 4 is done only as far as `GHelper.Shared` currently reaches: it holds `Logger`, `DeviceHelper`, `Keystone` and `UserIdentity`, none of which is a startable service. What the head now does is run real shared code across its whole lifetime — startup, duplicate-instance handover and shutdown all go through `Logger`, with `UserIdentity` read at startup — rather than only probing it once from a window constructor. Genuine service initialisation (config, ACPI, hardware control) cannot exist here until Story 5 extracts it.

**Verified 2026-09-12** on Windows 11, .NET 10, by driving the real tray with UI Automation and synthesized mouse input:

- `dotnet build G-Helper.WPF.sln -c Debug` and `dotnet build app\GHelper.sln -c Debug` — both 0 warnings / 0 errors.
- Launch shows no window; the notification area gains a "G-Helper WPF" icon and the log records the startup line.
- Left-clicking the tray icon shows the window, and clicking again hides it.
- Closing the window with its X button hides it; the window handle and process survive.
- The context menu offers "Open G-Helper" and "Exit"; Open shows the hidden window.
- Launching a second time leaves the process list unchanged, exits the newcomer, shows the existing window, and logs the handover.
- Exit ends the process, removes the tray icon with no ghost left behind, and logs the shutdown line. Relaunching afterwards claims the single-instance slot cleanly.
