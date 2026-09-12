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
| 2 | Tray-icon bootstrap and tray-resident lifetime (show/hide, context menu, single-instance) | App survives window close and reopens from the tray | Not started |
| 3 | Application icon and manifest | Correct icon in tray and taskbar | Not started |
| 4 | Initialise real shared services from this process rather than read-only probes | Services start without the WinForms head running | Not started |

## Validation

- `dotnet build G-Helper.WPF.sln -c Debug` → 0 errors, 0 warnings.
- Launch `GHelper.WPF\bin\x64\Debug\net10.0-windows\GHelper.WPF.exe` and confirm the shared layer resolves at runtime.
- Never run `GHelper.exe` and `GHelper.WPF.exe` at the same time: they share `%APPDATA%\GHelper\log.txt` and settings.

## Notes

The placeholder window deliberately carries no view model. It exists to prove the process boundary, and it will be deleted rather than grown — MVVM structure arrives with the first real view in Milestone 3.

## Execution Log

**Step 1 (2026-09-12).** Created `GHelper.WPF` (`net10.0-windows`, x64, `UseWPF`) with `App.xaml`, `App.xaml.cs`, `MainWindow.xaml`, `MainWindow.xaml.cs`, and a project reference to `GHelper.Shared`. The window reports the shared assembly identity, `Logger.logFile`, `UserIdentity.IsRunningAsSystem()`, and `DeviceHelper.GetGpuError()`, and the process appends to the shared log — confirming shared logic initialises from a second process. Steps 2-4 are untouched, so Story 6 is not close to complete.
