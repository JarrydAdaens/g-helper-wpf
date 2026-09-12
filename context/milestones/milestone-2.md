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
- The WinForms `app/` project still compiles and behaves correctly throughout, referencing the same shared logic.
- The upstream migration/source-mapping document (introduced in Milestone 1 Story 4) has been updated for every significant source move made in this milestone.

## Status

In Progress — 0/3 stories complete. Story 5 and Story 6 have both started; Story 7 is holding. The first extraction slice and a minimal WPF shell have landed and both heads build and launch, but the bulk of the shared-logic extraction is still ahead.

---

## Story Index

| # | Story | Type | Complexity | Effort | Risk | Plan | Status |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 5 | [Extract reusable business logic](#story-5) | Refactor | — | — | — | [plan](../implementation-plans/milestone-2/extract-reusable-business-logic/plan.md) | In Progress (slice 1 of N) |
| 6 | [Create G-Helper.WPF](#story-6) | Feature | — | — | — | [plan](../implementation-plans/milestone-2/create-ghelper-wpf/plan.md) | In Progress (shell only) |
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
New shared class-library project; incremental moves out of `app/AppConfig.cs`, `app/AsusACPI.cs`, `app/Helpers/`, `app/Handheld.cs`, `app/Ally/`, `app/Input/`, and related settings/designer files, keeping `app/` referencing the new project as each piece moves.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-2/extract-reusable-business-logic/plan.md`

**Status:** In Progress — slice 1 of an unknown number landed.

**Moved so far:** `GHelper.Shared` project created (`net10.0-windows`, x64, no `UseWindowsForms`/`UseWPF`) and referenced by `app/GHelper.csproj`. Moved into it: `Helpers/Logger.cs`, `Helpers/DeviceHelper.cs`, `Helpers/Keystone.cs`. Added `Helpers/UserIdentity.cs`, holding the `IsRunningAsSystem` / `IsUserAdministrator` implementations lifted out of `app/Helpers/ProcessHelper.cs`, which now delegates to it.

**Still to do (everything else):** `AppConfig.cs`, `AsusACPI.cs`, `HardwareControl.cs`, `NativeMethods.cs`, the remainder of `Helpers/`, and the `Ally/`, `AnimeMatrix/`, `AutoUpdate/`, `Battery/`, `Display/`, `Fan/`, `Gpu/`, `Input/`, `Mode/`, `Pawn/`, `Peripherals/`, `USB/` folders. Two files gate most of this and should lead the next slice:

- `app/AppConfig.cs` reads `Application.StartupPath` (WinForms) and references `AsusACPI` and `GHelper.Mode`. Almost every candidate module depends on it.
- `app/Helpers/ProcessHelper.cs` still uses `MessageBox`, `Application.Exit`, and `Application.ExecutablePath` in `CheckAlreadyRunning` and `RunAsAdmin`. Those two members are genuinely head-specific; the remaining process utilities are not and can move once the type is split.

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

**Status:** In Progress — shell only, tray infrastructure not started.

**Done:** `GHelper.WPF` project exists (`net10.0-windows`, x64, `UseWPF`), references `GHelper.Shared`, builds, launches, and shows a single placeholder window that reads the shared assembly identity, log path, `UserIdentity.IsRunningAsSystem()`, and `DeviceHelper.GetGpuError()` — verifying shared services initialise from this second process.

**Still to do:** tray-icon bootstrap and tray-resident lifetime (show/hide, context menu, single-instance handling), an application icon and manifest, and initialising real shared services rather than the four read-only probes currently on the window.

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

- **Resolved:** the project names are `GHelper.Shared` and `GHelper.WPF`, at the repository root beside `app/`, with a new root `G-Helper.WPF.sln` covering all three projects and `app/GHelper.sln` left untouched. Rationale recorded in [design.md](../design.md#how-the-pieces-fit-together).
- Establishing a concrete testing policy (currently undefined in Design) is a natural follow-up once Story 5 creates a UI-independent seam to test against.

---

## Notes

- Keep this Story Index in sync with the [Milestones Index](../design.md#milestones-index) in Design.
- If Story 5's extraction turns out to be much larger than a single story once scoped, consider splitting it into per-subsystem stories (hardware, input, RGB, configuration, etc.) rather than forcing one oversized story through.
