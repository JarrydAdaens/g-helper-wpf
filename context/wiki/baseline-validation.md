---
name: baseline-validation
description: Recorded results of validating the clean forked baseline (Milestone 1 Story 3) — build, launch, and runtime behaviour on a real ROG Ally.
metadata:
  version: "1.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---

# Baseline Validation

[Back to Wiki Home](home.md) · [Milestone 1, Story 3](../milestones/milestone-1.md#story-3)

Records the outcome of Milestone 1 Story 3: confirming the forked upstream source restores, builds, launches, and behaves normally before any structural change begins. Later milestones diff their behaviour against this record.

## Environment

- **Host:** ASUS ROG Ally RC71L (confirmed via `Win32_ComputerSystem` — `Manufacturer: ASUSTeK COMPUTER INC.`, `Model: ROG Ally RC71L_RC71L`). This is the actual target device, not a proxy machine.
- **OS:** Windows 11 Pro, build 10.0.26200, x64.
- **SDK:** .NET SDK 10.0.400 (`dotnet --version`).
- **Branch:** `wpf`.

## Build

```
dotnet build app\GHelper.sln -c Debug
```

- Restore, then build, completed cleanly: **0 warnings, 0 errors**.
- Output: `app\bin\x64\Debug\net10.0-windows\GHelper.dll` / `GHelper.exe`.
- No dependency-restore or SDK-version issues on a clean checkout.

## Launch

Ran `GHelper.exe` directly from the Debug output folder. The app is a WinForms tray-resident application (`UseWindowsForms=True`, `StartupObject=GHelper.Program`); it opened its main panel window rather than starting hidden.

The main window correctly identified the host as `G-Helper - ROG Ally RC71L` and rendered live sensor data (CPU/GPU temperature, fan RPM), confirming hardware access works end-to-end on a clean checkout.

## Runtime Log Evidence

App logs are **not** written into the repo; they go to `%APPDATA%\GHelper\log.txt` (see `app/Helpers/Logger.cs`). On this run the log showed normal startup sequencing with no unhandled exceptions:

- HID controller input devices detected and initialized (`vid_0b05&pid_1abe`, ASUS Tech. Inc.).
- Display/EDID detection (`TL070FVXS01-0`) and mode set succeeded.
- Aura (RGB) lighting commands sent successfully.
- Battery/controller/USB-C performance-mode state read successfully.

Two benign, expected errors appeared, both caused by running unelevated rather than any build/runtime defect:

```
Can't remove charge limit task: The system cannot find the file specified. (0x80070002)
Can't create a charge limit task: Access is denied. (0x80070005 (E_ACCESSDENIED))
```

The Battery Charge Limit feature schedules a Windows Scheduled Task, which requires elevation. Run G-Helper elevated (or accept the UAC prompt it should raise) if this feature needs to be exercised.

## Side Effect: Run-on-Startup Task Repointed

On launch, the app noticed its own version was newer than the previously scheduled startup task's target and **automatically repointed the existing `Run on Startup` Scheduled Task** to this Debug build's path:

```
Startup file is older 0.279.0.0, current is 0.282.0.0
Rescheduling to: C:\Projects\g-helper-wpf\app\bin\x64\Debug\net10.0-windows\GHelper.exe
```

This is inherent app behaviour (see the startup-task logic in `app/`), not something a validation run can opt out of short of disabling "Run on Startup" in the UI first. It is a real, machine-level side effect worth knowing about before running dev builds on a daily-driver device: **any debug build you launch with "Run on Startup" enabled will replace what runs at next login.**

For this validation pass, the task (`GHelper_S-1-5-21-677843870-752855350-3902718895-1001`) was disabled afterward at the user's request, rather than repointed to a previous production build (the prior target path was not recorded before it was overwritten).

**Recommendation for future baseline/dev runs:** before launching a debug build on a machine that also runs G-Helper day-to-day, either uncheck "Run on Startup" in the currently-installed build first, or expect to manually fix the scheduled task afterward.

## Result

| Check | Result |
| --- | --- |
| Dependency restore | Pass |
| Build (Debug, `x64`) | Pass — 0 warnings, 0 errors |
| Launch on target hardware (ROG Ally RC71L) | Pass |
| Main UI renders | Pass (see screenshot below) |
| Live hardware read (CPU/GPU temp, fan RPM, controller input, display, Aura) | Pass |
| Unhandled exceptions in log | None observed |
| Known non-blocking issue | Charge-limit scheduled-task creation requires elevation |

## Screenshot

The main panel as launched from a clean Debug build, docked at its default screen position, showing live Balanced-mode sensor readings, screen refresh-rate controls, Ally Controller bindings, and battery charge limit:

*(Screenshot delivered to the user directly; not embedded in-repo to avoid committing binary artifacts to `context/`.)*

## Conclusion

The clean, forked baseline restores, builds, and launches correctly on the actual target hardware (ROG Ally RC71L), with no build errors and no unhandled runtime exceptions. This satisfies Milestone 1 Story 3's Definition of Done and establishes the behavioural baseline that Milestone 2's extraction work (`G-Helper.Shared` split) should be diffed against.
