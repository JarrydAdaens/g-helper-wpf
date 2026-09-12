---
name: milestone-2
description: Milestone 2 - WPF Conversion and Architectural Split. Extracts reusable G-Helper logic into a shared library and stands up a functional WPF executable head alongside the preserved WinForms app.
metadata:
  version: "3.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---
# Milestone 2: WPF Conversion and Architectural Split

> Milestone. A coherent macro-feature or delivery outcome. (Tier numbers live only in `design.md` / `agenticworkflow.md`.)
>
> Related: [design.md (Milestones Index)](../design.md#milestones-index), [../backlog/](../backlog/)

---

## Intent

Separate reusable G-Helper logic from the WinForms front end into a shared library, and stand up a functional (initially near-empty) WPF executable head that references it, without breaking the existing WinForms application.

## Why it matters

- This is the load-bearing architectural change: every later WPF feature (Milestone 3) depends on a working Shared/WPF split existing first.
- Extracting logic incrementally, while keeping WinForms compiling, is the only way to validate the split against the Milestone 1 baseline as it happens rather than after a large rewrite.

## Outcome / Definition of Done

- A `G-Helper.Shared`-equivalent project exists and holds hardware/business logic that used to live only in the WinForms `app/` project.
- `G-Helper.WPF` exists, builds, references the shared logic, launches, and behaves as a tray application.
- The WinForms `source/app/` project still compiles and behaves correctly throughout, referencing the same shared logic.
- The upstream migration/source-mapping document (introduced in Milestone 1 Story 4) has been updated for every significant source move made in this milestone.

## Status

In Progress — 1/3 stories complete. Story 6 (the WPF head) is done: it builds, references the shared library, and runs as a tray application. Story 5 has landed five extraction slices, Story 7 is holding, and a good deal of the shared-logic extraction is still ahead. Separately, all first-party code (`app/`, `GHelper.Shared/`, `GHelper.WPF/`, and the umbrella `.sln`) moved under a new `source/` folder in a repo-wide tidy pass (2026-09-12); every path below now starts with `source/`. See [upstream-source-mapping.md](../wiki/upstream-source-mapping.md).

---

## Story Index

| # | Story | Type | Complexity | Effort | Risk | Plan | Status |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 5 | [Extract reusable business logic](#story-5) | Refactor | — | — | — | [plan](../implementation-plans/milestone-2/extract-reusable-business-logic/plan.md) | In Progress (slice 5 of N) |
| 6 | [Create G-Helper.WPF](#story-6) | Feature | — | — | — | [plan](../implementation-plans/milestone-2/create-ghelper-wpf/plan.md) | Complete |
| 7 | [Preserve WinForms baseline during the split](#story-7) | Refactor | — | — | — | *not yet generated* | In Progress (continuous, holding) |

---

## Stories

<a id="story-5"></a>

### Story 5: Extract reusable business logic

**Type:** Refactor

**Summary:**
Move non-UI behaviour out of the WinForms executable project (`app/`) into a reusable C# class-library project — hardware communication, device services, power management, fan control, controller/input handling, RGB/Aura logic, battery logic, and configuration — done progressively rather than as one large move.

**Why / value:**
This is the prerequisite for a WPF head to exist without duplicating hardware logic. It also reduces the surface area that future upstream WinForms changes touch directly.

**Rough scope:**
New shared class-library project; incremental moves out of `source/app/AppConfig.cs`, `source/app/AsusACPI.cs`, `source/app/Helpers/`, `source/app/Handheld.cs`, `source/app/Ally/`, `source/app/Input/`, and related settings/designer files, keeping `app/` referencing the new project as each piece moves.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-2/extract-reusable-business-logic/plan.md`

**Status:** In Progress — slice 5 of an unknown number landed. `AppConfig`, the file that gated most of the remaining work, has moved, and two hardware modules have followed it: `Pawn/`, then the bulk of `Peripherals/` (9,904 lines, the largest slice so far).

**Moved so far:** `GHelper.Shared` project created (`net10.0-windows`, x64, no `UseWindowsForms`/`UseWPF`) and referenced by `source/app/GHelper.csproj`. Moved into it:

- `Helpers/Logger.cs`, `Helpers/DeviceHelper.cs`, `Helpers/Keystone.cs` (slice 1).
- `source/app/Helpers/ProcessHelper.cs`, fully split: `Helpers/UserIdentity.cs` (slice 1) holds `IsRunningAsSystem` / `IsUserAdministrator`, and `Helpers/ProcessUtility.cs` (slice 2) holds the remaining process/service utilities (`KillByName`, `KillSmartDisplayControl`, `KillByProcess`, `StopDisableService`, `StartEnableService`, `RunCMD`, `SetPriority`). `ProcessHelper` keeps one-line delegating shims for all seven so its ~50 existing call sites across `source/app/` needed no changes. Only `CheckAlreadyRunning` and `RunAsAdmin` remain as real logic in `ProcessHelper`, since both use WinForms `Application`/`MessageBox`.
- `AppConfig.cs` in full (slice 3), keeping its global namespace so all ~1000 call sites across `source/app/` were untouched. `Application.StartupPath` became `AppContext.BaseDirectory` (the same value on modern .NET, and the only candidate that survives a single-file publish), and `ProcessHelper.IsRunningAsSystem()` became `UserIdentity.IsRunningAsSystem()`. Its two other edges were resolved by moving the `AsusFan` enum out of `AsusACPI.cs` into `GHelper.Shared/AsusFan.cs`, and by splitting the three pure config reads out of `GHelper.Mode.Modes` (`GetCurrent`, `GetBase`, `GetCurrentBase`) plus the five `Performance*` constants into a new shared `GHelper.Mode.ModeConfig`. `Modes` and `AsusACPI` keep delegating members and constant aliases respectively, so neither type's call sites changed.

The on-disk configuration format is now confirmed: a single flat JSON object of string-keyed scalars at `%APPDATA%\GHelper\config.json`, written atomically through a `.tmp` / `.bak` swap, with a `%PROGRAMDATA%` copy kept in sync for the SYSTEM case and a portable `config.json` beside the executable taking precedence over both.

- `Pawn/CpuInfo.cs`, `Pawn/IntelMsr.cs`, `Pawn/PawnIOWrapper.cs`, `Pawn/RyzenSmu.cs` (slice 4), moved unchanged with the `PawnIO` namespace kept as-is — confirmed zero WinForms references before moving. `Pawn/IntelMSR.bin` and `Pawn/RyzenSMU.bin` deliberately stayed behind in `source/app/Pawn/`: they are embedded resources in `GHelper.csproj` whose logical resource name is derived from the *calling* assembly's name at runtime, so they only resolve correctly while embedded in `GHelper.exe`, the only assembly that currently calls `Initialize(Assembly)`.

- `Peripherals/`'s 40 device files — `IPeripheral.cs`, `Keyboard/**`, `Mouse/**`, 9,904 lines — into `GHelper.Shared/Peripherals/` (slice 5), namespaces unchanged. Three edges had to be resolved first: `AnimeMatrix/Communication/` (`Device`, `Packet`, `UsbProvider`, `WindowsUsbProvider`) moved too, because `AsusKeyboard` and `AsusMouse` derive from `Device`, with `Packet`'s constructor widened `internal` → `protected` for the two subclasses still in the head; the `AuraMode` and `AuraSpeed` enums moved into `GHelper.Shared/USB/`, keeping the `GHelper.USB` namespace, since leaving them behind would have made the reference circular; and `AsusMouse`'s twelve default combo commands, spelled with `System.Windows.Forms.Keys`, now use a private `Vk` constant holder producing byte-identical command strings. `GHelper.Shared.csproj` gained `HidSharpCore` and an explicit `System.Drawing` implicit-using (the type is in the base framework; only the implicit using came from WinForms).

  `PeripheralsProvider.cs` deliberately stayed in `source/app/`: it is the one file in the folder that drives `Program.settingsForm` / `Program.inputDispatcher`, and it does so from inside its own connect/disconnect/battery flows rather than at its edges, so it needs a callback seam rather than a delegating shim. Nothing in the 40 moved files references it back.

**Still to do (everything else):** `AsusACPI.cs`, `HardwareControl.cs`, `NativeMethods.cs`, `Peripherals/PeripheralsProvider.cs`, the remainder of `Helpers/`, `Mode/` and `AnimeMatrix/`, and the `Ally/`, `AutoUpdate/`, `Battery/`, `Display/`, `Fan/`, `Gpu/`, `Input/`, `USB/` folders.

`AsusACPI` remains the awkward one: it has no UI dependency at all, but five of its own instance methods reach back into the head's `Program.acpi` static, which has to be untangled before it can move.

---

<a id="story-6"></a>

### Story 6: Create `G-Helper.WPF`

**Type:** Feature

**Summary:**
Create a new WPF executable project that builds, references the shared G-Helper library, launches, and stands up tray-application infrastructure. It may contain almost no UI at this stage — the goal is proving the shell works.

**Why / value:**
Establishes the second executable head early, so Milestone 3's view/overlay work has a running project to build into rather than starting from nothing.

**Rough scope:**
New WPF project in the solution; reference to the Story 5 shared project; minimal `App.xaml`/tray-icon bootstrap; verification that shared services can initialise from this process.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-2/create-ghelper-wpf/plan.md`

**Status:** Complete — the head builds, references `GHelper.Shared`, and runs as a real tray application.

**Done:** `GHelper.WPF` (`net10.0-windows`, x64, `UseWPF`) references `GHelper.Shared` and starts tray-resident: no window on launch, a "G-Helper WPF" notification-area icon, left-click to show/hide, a context menu with "Open G-Helper" and "Exit", and a window whose close button hides it rather than ending the process. A second launch activates the running instance instead of duplicating it (session-local named event; the WinForms head's opposite "new instance replaces old" behaviour was deliberately not copied). The executable carries `favicon.ico` and an `app.manifest` mirroring `source/app/GHelper.csproj`. Startup, duplicate-instance handover and shutdown all log through the shared `Logger`, and the placeholder window still reports shared assembly identity, log path, `UserIdentity.IsRunningAsSystem()` and `DeviceHelper.GetGpuError()`.

**Caveat on "initialise real shared services":** `GHelper.Shared` currently holds only `Logger`, `DeviceHelper`, `Keystone` and `UserIdentity` — none of which is a startable service. The head therefore exercises shared code across its whole lifetime rather than starting services that do not exist yet. Real service initialisation (configuration, ACPI, hardware control) arrives with Story 5's extraction and belongs to that story, not this one.

---

<a id="story-7"></a>

### Story 7: Preserve WinForms baseline during the split

**Type:** Refactor

**Summary:**
Ensure the architectural split does not silently break the existing WinForms application while source files are being reorganised: it must keep compiling (unless explicitly retired later), remain available for behavioural comparison, and not intentionally change hardware behaviour.

**Why / value:**
The WinForms app is the safety net during the entire conversion. Losing it mid-split would remove the only known-good comparison point established in Milestone 1 Story 3.

**Rough scope:**
Ongoing verification alongside Story 5/6 work rather than a single discrete change; regression triage whenever extraction changes WinForms behaviour.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-2/preserve-winforms-baseline-during-the-split/plan.md`

**Status:** In Progress — continuous constraint, holding after Story 5 slice 5.

Verified after the first extraction slice: `dotnet build app\GHelper.sln -c Debug` succeeds with 0 warnings / 0 errors, and the launched app shows its normal panel with live sensor readings and writes to the shared log as before.

Re-checked after Story 6's tray work: `dotnet build app\GHelper.sln -c Debug` still succeeds with 0 warnings / 0 errors. That change touched only `GHelper.WPF/`, so the WinForms head was not launched again — the two heads share `%APPDATA%\GHelper\`, and running them together would have muddied the tray verification.

Re-checked again after the `source/` repo tidy pass (2026-09-12): `dotnet build source\G-Helper.WPF.sln -c Debug` and `dotnet build source\app\GHelper.sln -c Debug` both succeed with 0 warnings / 0 errors from the new location.

Re-checked after Story 5 slice 3 (the `AppConfig` move), which warranted a full runtime check because it changes config I/O: both solutions still build 0/0, and the launched head logs `Config loaded from ...\AppData\Roaming\GHelper\config.json` — the same path as before the move — then opens its panel titled `G-Helper - ROG Ally RC71L` with live sensor readings (CPU 53°C / 3200RPM, GPU 43°C / 2700RPM) and the Ally-only controller section present, which means model detection still drives the model-conditional UI. Switching Balanced → Silent → Balanced applied at the hardware level and round-tripped `performance_mode` through `config.json` correctly; comparing the file before and after the session shows `start_count` as the only changed key.

Re-checked after Story 5 slice 5 (the `Peripherals/` move): both solutions still build 0/0, and the launched head opens its panel titled `G-Helper - ROG Ally RC71L` with the usual startup log and no new errors, including the `AuraMode: AuraStatic` and `Aura 1ABE` lines that exercise the relocated `AuraMode` enum. No ASUS peripheral is attached to this machine, and startup peripheral detection runs inside a fire-and-forget `Task`, so a type-load failure there would not surface — covered instead by reflecting over `GHelper.Shared.dll` and constructing all 103 concrete `IPeripheral` implementations (103 ok, 0 failed).

---

## Interdependency Order

1. Story 5 (extract shared logic) should lead, since Story 6 (`G-Helper.WPF`) needs something to reference.
2. Story 7 (preserve WinForms baseline) runs continuously alongside Story 5 and Story 6, not as a separate sequential step — every extraction in Story 5 must be checked against it before moving on.
3. This milestone as a whole depends on Milestone 1 Story 3 (validated clean baseline) being complete, so regressions here can be attributed correctly.

---

## Backlog Sources

- No backlog themes were pulled into this milestone. All three stories were mapped directly from the initial dictation (design document Section 13, Milestone 2) during project initialization, with high confidence.

---

## Deferred / Follow-up Work

- **Resolved:** the project names are `GHelper.Shared` and `GHelper.WPF`, under `source/` beside `source/app/`, with a root `source/G-Helper.WPF.sln` covering all three projects and `source/app/GHelper.sln` left content-untouched. Rationale recorded in [design.md](../design.md#how-the-pieces-fit-together).
- **Resolved (2026-09-12):** all first-party code moved one level deeper, under a new `source/` folder, in a repo-wide tidy pass unrelated to any single story. See [upstream-source-mapping.md](../wiki/upstream-source-mapping.md).
- Establishing a concrete testing policy (currently undefined in Design) is a natural follow-up once Story 5 creates a UI-independent seam to test against.

---

## Notes

- Keep this Story Index in sync with the [Milestones Index](../design.md#milestones-index) in Design.
- If Story 5's extraction turns out to be much larger than a single story once scoped, consider splitting it into per-subsystem stories (hardware, input, RGB, configuration, etc.) rather than forcing one oversized story through.
