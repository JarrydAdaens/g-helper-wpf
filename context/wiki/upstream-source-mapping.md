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

`G-Helper.Shared` does not exist yet. Until Milestone 2 Story 5 extracts it, **every upstream file maps 1:1 to the same path in this fork** — no source file has been moved, split, or renamed.

| Upstream path | Fork path | Status |
| --- | --- | --- |
| `app/**` | `app/**` | Identical |

Top-level `app/` folders as they currently stand: `Ally/`, `AnimeMatrix/`, `AutoUpdate/`, `Battery/`, `Display/`, `Fan/`, `Gpu/`, `Helpers/`, `Input/`, `Mode/`, `Overlay/`, `Pawn/`, `Peripherals/`, `Properties/`, `Resources/`, `UI/`, `USB/`.

## What This Page Will Track

From Milestone 2 onward, add a row for every upstream path that stops mapping 1:1 — moved into `G-Helper.Shared`, split across projects, replaced by a WPF equivalent, or deliberately dropped. Paths that are still identical need no row: the absence of a row means "unchanged, same path".
