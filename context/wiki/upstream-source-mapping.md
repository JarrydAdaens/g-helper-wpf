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
| `app/Helpers/ProcessHelper.cs` | `source/app/Helpers/ProcessHelper.cs` + `source/GHelper.Shared/Helpers/UserIdentity.cs` | Split. `IsRunningAsSystem` / `IsUserAdministrator` implementations moved to `UserIdentity`; `ProcessHelper` keeps one-line delegating members so upstream call sites still resolve. The rest of `ProcessHelper` stays in `app/` because it uses WinForms `Application` and `MessageBox`. |
| `app/GHelper.sln` | `source/app/GHelper.sln` (content unchanged) + `source/G-Helper.WPF.sln` | Supplemented. The upstream solution is untouched content-wise and still builds the WinForms head alone; the sibling root solution builds all three projects. |
| `app/favicon.ico`, `app/Resources/standard.ico` | unchanged, plus copies at `source/GHelper.WPF/favicon.ico` and `source/GHelper.WPF/Resources/standard.ico` | Copied, not moved. The WPF head uses the same artwork for its executable and tray icons. If upstream changes either file, mirror it into `source/GHelper.WPF/`. |

Top-level `source/app/` folders as they currently stand: `Ally/`, `AnimeMatrix/`, `AutoUpdate/`, `Battery/`, `Display/`, `Fan/`, `Gpu/`, `Helpers/`, `Input/`, `Mode/`, `Overlay/`, `Pawn/`, `Peripherals/`, `Properties/`, `Resources/`, `UI/`, `USB/` — all still present.

New fork-only paths with no upstream counterpart: `source/GHelper.Shared/` and `source/GHelper.WPF/`.

## What This Page Will Track

From Milestone 2 onward, add a row for every upstream path that stops mapping 1:1 — moved into `G-Helper.Shared`, split across projects, replaced by a WPF equivalent, or deliberately dropped. Paths that are still identical need no row: the absence of a row means "unchanged, same path".
