---
name: build-and-run
description: Quick reference for building and launching the fork locally — commands, log location, and known gotchas.
metadata:
  version: "1.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---

# Build and Run

[Back to Wiki Home](home.md)

Practical how-to, distinct from [baseline-validation.md](baseline-validation.md) (that page is a dated validation *record*; this page is the reusable *reference*).

## Prerequisites

- .NET SDK 10.0.400+ (`dotnet --version`).
- Windows (the app is WinForms today; `UseWindowsForms=True`, `net10.0-windows`).

## Build

```
dotnet build G-Helper.WPF.sln -c Debug
```

Builds all three projects: `GHelper.Shared`, the WinForms head, and the WPF head. `dotnet build app\GHelper.sln -c Debug` still works and builds only the WinForms head (plus `GHelper.Shared`, which it references).

Outputs:

- `app\bin\x64\Debug\net10.0-windows\GHelper.exe` — WinForms head
- `GHelper.WPF\bin\x64\Debug\net10.0-windows\GHelper.WPF.exe` — WPF head

## Run

Launch either exe directly, or `dotnet run --project app\GHelper.csproj` / `--project GHelper.WPF\GHelper.WPF.csproj`.

`GHelper.exe` is tray-resident. It may open its main panel on launch or start hidden, depending on saved settings; when it starts hidden its `MainWindowHandle` stays zero, so scripted automation has to find the titled top-level window of the process instead.

## Logs

Not written into the repo. Location: `%APPDATA%\GHelper\log.txt` (see `GHelper.Shared/Helpers/Logger.cs`), or `%PROGRAMDATA%\GHelper\log.txt` when running as SYSTEM.

## Gotchas

- **"Run on Startup" auto-repoints itself.** If that option is checked and you launch a build with a different version number than what's currently scheduled, the app silently repoints the real Windows Scheduled Task to the build you just ran. On a machine that also runs G-Helper day-to-day, uncheck "Run on Startup" first, or expect to manually fix the scheduled task afterward.
- **Battery Charge Limit needs elevation.** Without it, expect a benign `Access is denied (0x80070005)` in the log when the app tries to (re)create its charge-limit scheduled task. Not a build defect.
- **`KillRunningGHelper` build target.** The `.csproj` kills any running `GHelper.exe` before building (via a named event handle), so a build won't silently fail to overwrite a locked exe.
- **Don't run both heads at once.** `GHelper.exe` and `GHelper.WPF.exe` share `%APPDATA%\GHelper\` — the same log file and settings. Close one before starting the other.
