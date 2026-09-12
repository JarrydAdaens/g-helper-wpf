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

In Progress — 1/3 stories complete. Story 6 (the WPF head) is done: it builds, references the shared library, and runs as a tray application. Story 5 has landed its first extraction slice, Story 7 is holding, and the bulk of the shared-logic extraction is still ahead. Separately, all first-party code (`app/`, `GHelper.Shared/`, `GHelper.WPF/`, and the umbrella `.sln`) moved under a new `source/` folder in a repo-wide tidy pass (2026-09-12); every path below now starts with `source/`. See [upstream-source-mapping.md](../wiki/upstream-source-mapping.md).

---

## Story Index

| # | Story | Type | Complexity | Effort | Risk | Plan | Status |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 5 | [Extract reusable business logic](#story-5) | Refactor | — | — | — | [plan](../implementation-plans/milestone-2/extract-reusable-business-logic/plan.md) | In Progress (slice 1 of N) |
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

**Status:** In Progress — slice 1 of an unknown number landed.

**Moved so far:** `GHelper.Shared` project created (`net10.0-windows`, x64, no `UseWindowsForms`/`UseWPF`) and referenced by `source/app/GHelper.csproj`. Moved into it: `Helpers/Logger.cs`, `Helpers/DeviceHelper.cs`, `Helpers/Keystone.cs`. Added `Helpers/UserIdentity.cs`, holding the `IsRunningAsSystem` / `IsUserAdministrator` implementations lifted out of `source/app/Helpers/ProcessHelper.cs`, which now delegates to it.

**Still to do (everything else):** `AppConfig.cs`, `AsusACPI.cs`, `HardwareControl.cs`, `NativeMethods.cs`, the remainder of `Helpers/`, and the `Ally/`, `AnimeMatrix/`, `AutoUpdate/`, `Battery/`, `Display/`, `Fan/`, `Gpu/`, `Input/`, `Mode/`, `Pawn/`, `Peripherals/`, `USB/` folders. Two files gate most of this and should lead the next slice:

- `source/app/AppConfig.cs` reads `Application.StartupPath` (WinForms) and references `AsusACPI` and `GHelper.Mode`. Almost every candidate module depends on it.
- `source/app/Helpers/ProcessHelper.cs` still uses `MessageBox`, `Application.Exit`, and `Application.ExecutablePath` in `CheckAlreadyRunning` and `RunAsAdmin`. Those two members are genuinely head-specific; the remaining process utilities are not and can move once the type is split.

`Peripherals/` and `Pawn/` are the cleanest large modules to move after that — `Pawn/` has zero UI references, and `Peripherals/` has them in only one of its 41 files (`PeripheralsProvider.cs`).

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

**Status:** In Progress — continuous constraint, holding after slice 1.

Verified after the first extraction slice: `dotnet build app\GHelper.sln -c Debug` succeeds with 0 warnings / 0 errors, and the launched app shows its normal panel with live sensor readings and writes to the shared log as before.

Re-checked after Story 6's tray work: `dotnet build app\GHelper.sln -c Debug` still succeeds with 0 warnings / 0 errors. That change touched only `GHelper.WPF/`, so the WinForms head was not launched again — the two heads share `%APPDATA%\GHelper\`, and running them together would have muddied the tray verification.

Re-checked again after the `source/` repo tidy pass (2026-09-12): `dotnet build source\G-Helper.WPF.sln -c Debug` and `dotnet build source\app\GHelper.sln -c Debug` both succeed with 0 warnings / 0 errors from the new location.

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
