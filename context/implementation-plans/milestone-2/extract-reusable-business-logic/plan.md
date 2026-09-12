---
name: plan-extract-reusable-business-logic
description: Implementation plan for Milestone 2 Story 5 — incrementally extracting UI-independent logic out of the WinForms app/ project into GHelper.Shared.
metadata:
  version: "1.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---
# Story 5 — Extract reusable business logic

[Milestone 2](../../../milestones/milestone-2.md#story-5) · [design.md](../../../design.md#how-the-pieces-fit-together) · [upstream-source-mapping.md](../../../wiki/upstream-source-mapping.md)

## Objective

Move UI-independent logic out of `source/app/` into `GHelper.Shared`, one safe slice at a time, with `source/app/GHelper.sln` compiling and running correctly after every slice (Story 7).

## Estimates

Deliberately not scored per-slice. The story is open-ended by design; each slice is small enough to execute and verify in one pass, and the slice list below is the real unit of work.

## Approach

Slices are chosen by dependency direction, not by folder tidiness. A file can only move once nothing it calls is left behind in `source/app/`. Two files currently gate almost everything: `AppConfig` (uses WinForms `Application.StartupPath`, and references `AsusACPI` and `GHelper.Mode`) and `ProcessHelper` (uses `MessageBox`, `Application.Exit`, `Application.ExecutablePath`).

Where a type is part UI-specific and part not, split it and leave one-line delegating members behind in `source/app/` rather than churning every call site. Delete the shims when the remainder of the type moves.

## Slices

| # | Slice | Status |
| --- | --- | --- |
| 1 | Create `GHelper.Shared`; move `Logger`, `DeviceHelper`, `Keystone`; extract `UserIdentity` out of `ProcessHelper` | Done |
| 2 | Split `ProcessHelper`: move the process/service utilities, leave `CheckAlreadyRunning` and `RunAsAdmin` in the head | Done |
| 3 | Move `AppConfig` (replace `Application.StartupPath`; resolve the `AsusACPI` / `GHelper.Mode` edges first). Folded in: move the `AsusFan` enum, and split `Modes`' config reads out as `ModeConfig` | Done |
| 4+ | Hardware modules, cheapest first: `Pawn/` (no UI references), `Peripherals/` (UI references in one file of 41), then `USB/`, `Battery/`, `Fan/`, `Display/`, `Gpu/`, the rest of `Mode/`, `AnimeMatrix/`, `Ally/`, `Input/` | Not started |

## Validation (per slice)

1. `dotnet build source\G-Helper.WPF.sln -c Debug` → 0 errors, 0 warnings.
2. Launch `source\app\bin\x64\Debug\net10.0-windows\GHelper.exe`, confirm the panel opens with live sensor readings and the log is still being written.
3. Update [upstream-source-mapping.md](../../../wiki/upstream-source-mapping.md) with a row for every path that stops mapping 1:1.

## Risk mitigation

- `GHelper.Shared` sets neither `UseWindowsForms` nor `UseWPF`, so any UI leak into the shared layer fails the build rather than passing review.
- Moves use `git mv` so history follows the file and upstream merges can still track it.
- The WinForms head is never allowed to be red between slices.

## Execution Log

**Slice 1 (2026-09-12).** Created `GHelper.Shared`; `git mv`'d `Logger.cs`, `DeviceHelper.cs`, `Keystone.cs` from `app/Helpers/`; added `UserIdentity.cs` holding the `IsRunningAsSystem` / `IsUserAdministrator` implementations lifted from `ProcessHelper`, which now delegates. Added the `ProjectReference` to `app/GHelper.csproj` and a root `G-Helper.WPF.sln`. Solution builds 0/0; WinForms head launches and behaves normally.

Slice 1 is intentionally small: it is the only set of files with no remaining dependency on anything that has to stay in `app/`, and moving `Logger` first is what unblocks every later slice.

**Slice 2 (2026-09-12).** Moved the remaining non-UI members of `ProcessHelper` (`KillByName`, `KillSmartDisplayControl`, `KillByProcess`, `StopDisableService`, `StartEnableService`, `RunCMD`, `SetPriority`) into a new `GHelper.Shared/Helpers/ProcessUtility.cs`. Named it `ProcessUtility` rather than reusing `ProcessHelper`, since both types share the `GHelper.Helpers` namespace and identical names across the app assembly and the `GHelper.Shared` assembly it references would make every unqualified call site ambiguous (CS0104) — the same naming pattern slice 1 already established for `UserIdentity`. `ProcessHelper` keeps one-line delegating shims for all seven, so its ~50 existing call sites across `source/app/` needed no changes. `CheckAlreadyRunning` and `RunAsAdmin` stayed in `app/` as real logic (WinForms `Application`/`MessageBox`). Updated the `ProcessHelper.cs` mapping row. Both solutions build 0/0; the WinForms head's behaviour is unaffected (delegation only, no logic changes).

**Slice 3 (2026-09-12).** `git mv`'d `AppConfig.cs` into `GHelper.Shared` whole, keeping its global namespace so all ~1000 `AppConfig.` call sites across `source/app/` resolve unchanged — the opposite of the shim pattern used in slices 1 and 2, and the right call here precisely because `AppConfig` is the file every later slice depends on. Shimming it would have forced a rename at every one of those call sites as each module moved.

Three edges resolved rather than shimmed:

- **`Application.StartupPath`** → `AppContext.BaseDirectory`. On modern .NET `Application.StartupPath` *is* `AppContext.BaseDirectory` (both include the trailing separator, which is what the existing `.Trim('\\')` was there for), and `AppContext.BaseDirectory` is also the only one of the two candidates that still resolves under a single-file publish — `Assembly.GetEntryAssembly().Location` returns an empty string there, which would have silently relocated the portable config. Confirmed at runtime: the launched head logs `Config loaded from C:\Users\<user>\AppData\Roaming\GHelper\config.json`, the same path as before.
- **`AsusACPI`** — only two things were actually needed: the `AsusFan` enum and the `PerformanceTurbo` / `PerformanceSilent` constants. `AsusFan` moved to `GHelper.Shared/AsusFan.cs` (global namespace, so its ~100 call sites are untouched), and the five `Performance*` constants moved to `ModeConfig` with `AsusACPI` keeping them as one-line aliases, so the values stay single-sourced and its own 20 call sites are untouched. The `AsusACPI` class itself did **not** move: it calls `Program.acpi` — the head's entry point — in five of its own instance methods. Rewriting those to `this` would probably be correct but is not provably behaviour-preserving, so it was left for a later slice.
- **`GHelper.Mode`** — `Modes` and `AppConfig` are mutually recursive. Only three `Modes` members are involved (`GetCurrent`, `GetBase`, `GetCurrentBase`) and all three are pure `AppConfig` reads, so they moved into a new `GHelper.Shared/Mode/ModeConfig.cs` with `Modes` keeping delegating members. The rest of `Modes` stays in the head, where it needs `Properties.Strings` and `Program.acpi`.

`System.Management` was added to `GHelper.Shared.csproj` for the WMI model detection that arrived with `AppConfig`. Both solutions build 0/0. Noted but deliberately untouched: `AsusMode` and `AsusGPU` in `AsusACPI.cs` are declared and never used anywhere in the repository — pre-existing dead code, not this slice's to delete.
