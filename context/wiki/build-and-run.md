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
dotnet build app\GHelper.sln -c Debug
```

Output: `app\bin\x64\Debug\net10.0-windows\GHelper.exe`.

## Run

Launch `GHelper.exe` directly, or `dotnet run --project app\GHelper.csproj`. It's a tray-resident app — it opens its main panel window on launch rather than starting hidden.

## Logs

Not written into the repo. Location: `%APPDATA%\GHelper\log.txt` (see `app/Helpers/Logger.cs`).

## Gotchas

- **"Run on Startup" auto-repoints itself.** If that option is checked and you launch a build with a different version number than what's currently scheduled, the app silently repoints the real Windows Scheduled Task to the build you just ran. On a machine that also runs G-Helper day-to-day, uncheck "Run on Startup" first, or expect to manually fix the scheduled task afterward.
- **Battery Charge Limit needs elevation.** Without it, expect a benign `Access is denied (0x80070005)` in the log when the app tries to (re)create its charge-limit scheduled task. Not a build defect.
- **`KillRunningGHelper` build target.** The `.csproj` kills any running `GHelper.exe` before building (via a named event handle), so a build won't silently fail to overwrite a locked exe.
