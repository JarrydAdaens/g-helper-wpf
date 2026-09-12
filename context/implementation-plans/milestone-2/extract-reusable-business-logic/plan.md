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
| 2 | Split `ProcessHelper`: move the process/service utilities, leave `CheckAlreadyRunning` and `RunAsAdmin` in the head | Not started |
| 3 | Move `AppConfig` (replace `Application.StartupPath`; resolve the `AsusACPI` / `GHelper.Mode` edges first) | Not started |
| 4+ | Hardware modules, cheapest first: `Pawn/` (no UI references), `Peripherals/` (UI references in one file of 41), then `USB/`, `Battery/`, `Fan/`, `Display/`, `Gpu/`, `Mode/`, `AnimeMatrix/`, `Ally/`, `Input/` | Not started |

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
