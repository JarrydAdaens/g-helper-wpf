---
name: upstream-source-mapping
description: Living map from upstream G-Helper source locations to their G-Helper.WPF fork equivalents, plus the branch/remote topology upstream merges rely on.
metadata:
  version: "1.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---

# Upstream Source Mapping

[Back to Wiki Home](home.md) · [Milestone 1, Story 4](../milestones/milestone-1.md#story-4)

Step 3 of [Pipeline 2 — Upstream Merge](../design.md#pipeline-2--upstream-merge) depends on this page: given a file changed upstream, it answers "where does that code live in this fork now?". Update it in the same change that moves code, rather than reconstructing it afterwards.

## Repository Topology

| Role | Where | Notes |
| --- | --- | --- |
| Upstream | `upstream` remote → `seerge/g-helper`, default branch `main` | Fetch only. Never push. |
| Fork sync branch | `main` | Mirrors upstream. No fork work is committed here. |
| Development branch | `wpf` | All fork work. `main` merges forward into it. |

Verified 2026-09-12:

- GitHub records this repository as a fork of `seerge/g-helper` (public GitHub API: `"fork": true`, `parent` and `source` both `seerge/g-helper`).
- The `upstream` remote exists and fetches successfully.
- `main` is 0 ahead / 4 behind `upstream/main` — an unmodified ancestor, so it is still a clean sync point. The 4 fork commits live on `wpf`.
- Upstream's default branch is `main`, not `master`.

## Current Mapping State

`GHelper.Shared` now exists, but Milestone 2 Story 5's extraction is only partially done. **Every upstream path not listed below maps to the same relative path under `source/` in this fork** — since the repo tidy pass below, the baseline transform is "prepend `source/`", not "identical". The absence of a row beyond that transform means "unchanged, just under `source/`".

| Upstream path | Fork path | Status |
| --- | --- | --- |
| `app/**` (everything) | `source/app/**` | **Repo tidy pass (2026-09-12).** All first-party code — `app/`, `GHelper.Shared/`, `GHelper.WPF/`, and `G-Helper.WPF.sln` — moved one level deeper under a new `source/` folder, matching the layout convention of other repos in this workspace. Purely a parent-directory move done with `git mv`; no file content changed. `context/`, `harness/`, `docs/`, `.github/`, and root-level config/doc files were not moved. CI (`build.yml`, `release.yml`) and `.gitignore` were updated to match. |
| `app/Helpers/Logger.cs` | `source/GHelper.Shared/Helpers/Logger.cs` | Moved. One line changed: the SYSTEM check now calls `UserIdentity` instead of `ProcessHelper`. |
| `app/Helpers/DeviceHelper.cs` | `source/GHelper.Shared/Helpers/DeviceHelper.cs` | Moved unchanged. |
| `app/Helpers/Keystone.cs` | `source/GHelper.Shared/Helpers/Keystone.cs` | Moved unchanged. |
| `app/Helpers/ProcessHelper.cs` | `source/app/Helpers/ProcessHelper.cs` + `source/GHelper.Shared/Helpers/UserIdentity.cs` + `source/GHelper.Shared/Helpers/ProcessUtility.cs` | Split, in two slices. Slice 1: `IsRunningAsSystem` / `IsUserAdministrator` implementations moved to `UserIdentity`. Slice 2: the remaining non-UI process/service utilities (`KillByName`, `KillSmartDisplayControl`, `KillByProcess`, `StopDisableService`, `StartEnableService`, `RunCMD`, `SetPriority`) moved to `ProcessUtility` — named separately from `ProcessHelper`, not reused, because both types share the `GHelper.Helpers` namespace and identical names across the two referenced assemblies would make every unqualified call site ambiguous. `ProcessHelper` keeps one-line delegating members for all of the above so upstream call sites still resolve unchanged. `CheckAlreadyRunning` and `RunAsAdmin` stay in `app/` because they use WinForms `Application` and `MessageBox`. |
| `app/AppConfig.cs` | `source/GHelper.Shared/AppConfig.cs` | Moved whole-file. The type keeps the global namespace it always had, so all ~1000 `AppConfig.` call sites across `source/app/` resolve unchanged. Three edits: `Application.StartupPath` became `AppContext.BaseDirectory` (the same value — that is literally what WinForms returns on modern .NET — and unlike `Assembly.Location` it still resolves for a single-file publish); `ProcessHelper.IsRunningAsSystem()` became `UserIdentity.IsRunningAsSystem()`; and the `Modes.GetCurrent()` / `Modes.GetCurrentBase()` / `AsusACPI.Performance*` references became `ModeConfig.*`. `System.Management` was added to `GHelper.Shared.csproj` for the WMI model detection that came with the file. |
| `app/AsusACPI.cs` | `source/app/AsusACPI.cs` + `source/GHelper.Shared/AsusFan.cs` | Split, narrowly. The `AsusFan` enum moved to the shared layer because `AppConfig` keys its stored fan curves by it; it stays in the global namespace, so its ~100 call sites are unchanged. The five `Performance*` constants now alias `GHelper.Mode.ModeConfig` instead of declaring their own literals, so the values stay single-sourced. `AsusMode` and `AsusGPU` were left in place, and so was the `AsusACPI` class itself — it reaches back into the head's `Program.acpi` in five methods, so it cannot move until that is untangled. |
| `app/Mode/Modes.cs` | `source/app/Mode/Modes.cs` + `source/GHelper.Shared/Mode/ModeConfig.cs` | Split. The three pure config reads (`GetCurrent`, `GetBase`, `GetCurrentBase`) plus the performance-mode constants moved to `ModeConfig`, which is what lets the shared `AppConfig` resolve the current mode. `Modes` keeps one-line delegating members for all three, so its call sites are unchanged. The rest of `Modes` stays in the head: `GetName` / `GetDictonary` need `Properties.Strings`, and `SetCurrent` / `InitFullSpeed` need `Program.acpi`. |
| `app/GHelper.sln` | `source/app/GHelper.sln` (content unchanged) + `source/G-Helper.WPF.sln` | Supplemented. The upstream solution is untouched content-wise and still builds the WinForms head alone; the sibling root solution builds all three projects. |
| `app/Pawn/CpuInfo.cs`, `app/Pawn/IntelMsr.cs`, `app/Pawn/PawnIOWrapper.cs`, `app/Pawn/RyzenSmu.cs` | `source/GHelper.Shared/Pawn/*.cs` | Moved unchanged (Milestone 2 Story 5, slice 4). Zero WinForms references confirmed before the move; the `PawnIO` namespace was kept as-is since call sites across `source/app/` reference it both fully-qualified (`PawnIO.CpuInfo`, `PawnIO.IntelMsr`) and via `using PawnIO;`, and neither form needed to change. `app/Pawn/IntelMSR.bin` and `app/Pawn/RyzenSMU.bin` **deliberately stayed** in `source/app/Pawn/` — they are `EmbeddedResource` entries in `GHelper.csproj` (`LogicalName` `GHelper.IntelMSR.bin` / `GHelper.RyzenSMU.bin`), and `IntelMsr.Initialize(Assembly)` / `RyzenSmuService.Initialize(Assembly)` build the resource name from the *caller's* assembly name (`assembly.GetName().Name + ".IntelMSR.bin"`), which only resolves correctly when the `.bin` stays embedded in the assembly that calls `Initialize` — currently always `GHelper.exe` (`HardwareControl.cs`, `Mode/ModeControl.cs`). Moving the `.bin` files would have required rewriting the `EmbeddedResource` paths in `GHelper.csproj` for no behavioural benefit, so the folder split at the `.cs`/`.bin` boundary instead. |
| `app/favicon.ico`, `app/Resources/standard.ico` | unchanged, plus copies at `source/GHelper.WPF/favicon.ico` and `source/GHelper.WPF/Resources/standard.ico` | Copied, not moved. The WPF head uses the same artwork for its executable and tray icons. If upstream changes either file, mirror it into `source/GHelper.WPF/`. |

Top-level `source/app/` folders as they currently stand: `Ally/`, `AnimeMatrix/`, `AutoUpdate/`, `Battery/`, `Display/`, `Fan/`, `Gpu/`, `Helpers/`, `Input/`, `Mode/`, `Overlay/`, `Pawn/`, `Peripherals/`, `Properties/`, `Resources/`, `UI/`, `USB/` — all still present.

New fork-only paths with no upstream counterpart: `source/GHelper.Shared/` and `source/GHelper.WPF/`.

## What This Page Will Track

From Milestone 2 onward, add a row for every upstream path that stops mapping 1:1 — moved into `G-Helper.Shared`, split across projects, replaced by a WPF equivalent, or deliberately dropped. Paths that are still identical need no row: the absence of a row means "unchanged, same path".
