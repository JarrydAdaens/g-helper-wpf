---
name: design
description: Design specification for G-Helper.WPF — a WPF rebuild of the G-Helper ROG Ally control application, its Shared/WinForms/WPF split, and the embedded milestones index.
metadata:
  version: "3.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---
# G-Helper.WPF - Design Specification

## Purpose of This File

This file is the Design tier: the maintained design specification covering the whole deliverable and how it breaks into its largest pieces. It synthesizes the project's initial dictation (see [dictations-tier-0/2026-09-12_ghelper-wpf-design-document.md](dictations-tier-0/2026-09-12_ghelper-wpf-design-document.md)) into stable project direction. The **Milestones Index** lives as a subsection of this file (see below); the actual Milestone documents are separate files under `milestones/`.

## Context Hierarchy

This is one of only two files in the framework allowed to state the numbered tier table (the other is [agenticworkflow.md](agenticworkflow.md)). Elsewhere, refer to tiers by name only.

| Tier | Document | Purpose |
| --- | --- | --- |
| 0 - Dictation | [dictations-tier-0/](dictations-tier-0/) | Raw dictation and supplemental design changes, before structure is imposed |
| 1 - Design | `design.md` | The whole deliverable, its largest pieces, and the Milestones Index subsection |
| 2 - Milestone | [milestones/](milestones/) | One coherent macro-feature or delivery outcome; contains the story list needed to deliver it |
| 3 - Story | Inside milestone docs, optionally staged first in [backlog/](backlog/) | Discrete work units (features, bugs, refactors) |
| 4 - Implementation Plan | `implementation-plans/<milestone-slug>/<story-slug>/plan.md` | The normalized how-to for one story, with scoring and mitigation |
| 5 - Phase *(optional)* | `implementation-plans/<milestone-slug>/<story-slug>/` | A safe slice of an over-large story |
| Support | [laws.md](laws.md) | Constitutional code quality and security laws — loaded first by all agents |
| Support | [agenticworkflow.md](agenticworkflow.md) | Workflow for AI agent collaboration |
| Support | [agent-thinking.md](agent-thinking.md) | Optional temporary scratchpad for long tasks |
| Support | [wiki/home.md](wiki/home.md) | Operational reference notes and cheat sheets |

The backlog (`backlog/`) is not a numbered tier. It is a staging pool — informally "Milestone -1" — of unscheduled stories before they are pulled into a milestone document. See the Milestones Index below and [agenticworkflow.md](agenticworkflow.md) for how milestone planning draws from it.

---

## Milestones Index

> This index is the table of contents for the project's milestones. It lives inside Design because a standalone milestones table is the same tier as Design. Each entry links down to a separate Milestone document under `milestones/`, which directly contains that milestone's story list.

| Milestone | Document | Status | Why it matters | What it unlocks |
| --- | --- | --- | --- | --- |
| Milestone 1: Repository and Context Setup | [milestones/milestone-1.md](milestones/milestone-1.md) | In Progress | Nothing later can be trusted without a validated baseline and durable upstream-sync rules | A known-good starting point and a repeatable process for absorbing future upstream `G-Helper` changes |
| Milestone 2: WPF Conversion and Architectural Split | [milestones/milestone-2.md](milestones/milestone-2.md) | Not Started | The WinForms/business-logic split is the load-bearing architectural change everything else depends on | A working, empty `G-Helper.WPF` executable head referencing shared logic, with the WinForms app still intact |
| Milestone 3: WPF Equivalence and Product Changes | [milestones/milestone-3.md](milestones/milestone-3.md) | Not Started | This is where the WPF app actually becomes daily-usable and the ROG Ally overlay/binding vision is delivered | The V1 release: WPF feature parity, the left-side overlay shell, and the generalised gamepad/keyboard/mouse binding system |

Keep this index in sync as milestones are added, completed, reordered, or reclassified. When a backlog story scores as epic-sized, promote it into this index as a new milestone.

---

## Executive Summary

**G-Helper.WPF** is a public fork of [G-Helper](https://github.com/seerge/g-helper) — the third-party ASUS ROG hardware control application — rebuilt around a WPF front end.

- **What it is:** the same G-Helper hardware-control capability (fan curves, power modes, Aura/RGB lighting, battery limits, display controls, ROG Ally input handling), fronted by a new WPF UI instead of the existing WinForms UI, plus an extended, user-configurable input-remapping system.
- **Who it is for:** ROG Ally (and other ASUS ROG laptop/handheld) owners who currently use G-Helper, with particular attention to the handheld/Ally experience.
- **What problem it solves:** the existing WinForms UI is a conventional floating desktop window, which is a poor fit for handheld use, and its configurable-button system only exposes a small fixed set of hard-coded actions. This fork replaces the shell with a left-anchored, handheld-friendly overlay (in the spirit of the pre-Game-Bar Armoury Crate SE overlay) and replaces the fixed button list with an extensible, arbitrary input-binding system (gamepad, keyboard, and mouse outputs).
- **Why this architecture is appropriate:** the existing hardware/business logic is sound and must be preserved; only the UI layer and the binding model need to change. Splitting UI-specific code from reusable logic lets both a WinForms head and a WPF head exist side by side during the transition, and keeps the fork mergeable against upstream G-Helper as that project continues to evolve.

### Core Principles

- **Preserve behaviour before redesigning it.** The first goal is functional parity with upstream G-Helper, not a rewrite. UI and binding-model changes come only after parity is reached.
- **Separate UI from reusable logic.** Hardware communication, device services, power/fan/battery/RGB logic, controller input handling, and configuration must not depend on WinForms or WPF specifically, so both executable heads can reference the same shared logic.
- **Design for upstream mergeability from the start.** Because this fork rearranges source files, a living migration/source-mapping document (see [Processing Pipelines](#pipeline-2-upstream-merge) below) must be kept current whenever code moves, splits, or is replaced — not reconstructed after the fact.
- **Standard behaviour is the default; bindings are overrides.** The generalised input-binding system must never require every physical input to have a permanently visible configuration slot — unbound inputs keep their normal controller behaviour, and a user-created binding is an explicit override on top.
- **Show only what's relevant.** The binding editor is context-sensitive: only the configuration controls relevant to the selected action type are visible; everything else collapses.

---

## System Architecture

### How the Pieces Fit Together

The target shape is a three-project split:

```text
G-Helper (solution)
│
├── G-Helper.Shared            reusable, UI-independent logic
│   ├── hardware communication (ASUS ACPI/WMI)
│   ├── device services
│   ├── power management
│   ├── fan control
│   ├── controller / input handling
│   ├── RGB / Aura logic
│   ├── battery logic
│   └── configuration
│
├── G-Helper                   existing WinForms executable head (app/)
│   └── references G-Helper.Shared
│
└── G-Helper.WPF                new WPF executable head
    └── references G-Helper.Shared
```

`G-Helper.Shared` does not exist yet as a separate project; today all logic and UI live together in the single `app/` WinForms project (`app/GHelper.csproj`). Milestone 2 Story 5 is the extraction of this shared layer out of `app/`, done incrementally rather than as a single rewrite. Exact project names may change during extraction; the constraint that does not change is that UI-specific code must not leak into the shared layer.

**External services and dependencies (as currently understood from the existing `app/` source, not yet fully re-verified against the extracted Shared layer):**

- ASUS ACPI/WMI hardware interface (`app/AsusACPI.cs`, `app/Helpers/AsusService.cs`) — fan curves, power modes, battery limits, keyboard/backlight control.
- ROG Ally-specific handheld logic (`app/Handheld.cs`, `app/Ally/`) — the touch-keyboard gesture and other Ally-only behaviour this project extends.
- Windows tray/shell integration for the tray-resident application model both executable heads must preserve.
- Controller/input handling (`app/Input/`) — the layer the new generalised binding system builds on.

### Repository Structure

```text
g-helper-wpf/
|-- app/                        existing WinForms executable head (GHelper.csproj / GHelper.sln)
|   |-- Ally/                   ROG Ally-specific behaviour (touch keyboard gesture, etc.)
|   |-- Input/                  controller/input handling
|   |-- Helpers/                cross-cutting hardware/OS helpers
|   |-- UI/                     WinForms controls and forms
|   `-- ...
|-- context/
|   |-- dictations-tier-0/
|   |-- design.md
|   |-- milestones/
|   |-- backlog/
|   |-- implementation-plans/
|   |-- laws.md
|   |-- agenticworkflow.md
|   |-- agent-thinking.md
|   `-- wiki/
|-- harness/
|-- docs/
|-- .github/
`-- README.md
```

`G-Helper.Shared` and `G-Helper.WPF` do not exist yet; they are created by Milestone 2 (Story 5 and Story 6 respectively) alongside `app/`, which keeps its current role as the WinForms head until that milestone's extraction work lands.

---

## Processing Pipelines

### Pipeline 1 — Input Binding Resolution

1. A physical input event occurs (controller button, D-pad direction, trigger, Command Center/ROG button, or supported rear paddle).
2. The binding resolver checks whether a user-created binding exists for that input.
3. If no binding exists, standard controller behaviour is used unchanged (for example, physical X produces the X controller action).
4. If a binding exists, its configured action type determines the output:
   - **Gamepad Action** — emits a different logical gamepad output (for example X -> Y).
   - **Keyboard Action** — emits a captured keyboard key (for example X -> Backspace).
   - **Mouse Action** — emits a mouse action (for example Right Trigger -> Left Click).
   - **Touch Keyboard** — opens/closes the on-screen touch keyboard (no parameter).
   - **Custom/Launch** — runs the configured executable/command string.
5. The binding editor only shows the configuration control relevant to the selected action type; all others stay collapsed.

### Pipeline 2 — Upstream Merge

1. Update `main` from the upstream `G-Helper` repository.
2. Inspect the upstream diff.
3. Use the migration/source-mapping document to find the corresponding fork location for each changed file.
4. Separate UI-only changes from reusable hardware/business-logic changes.
5. Port business/hardware fixes into `G-Helper.Shared`.
6. Port relevant UI behaviour into WPF views rather than copying WinForms changes verbatim.
7. Update the migration document wherever the source relationship changed.
8. Validate both build and runtime behaviour before merging into the development branch.

The migration/source-mapping document itself is created in Milestone 1 Story 4 and does not exist yet.

---

## Configuration

### Primary Configuration

The existing WinForms application persists settings via `app/AppConfig.cs` and related per-feature settings/designer files (`app/Settings.cs`, `app/OverlayConfig.cs`, `app/AsusKeyboardSettings.cs`, `app/AsusMouseSettings.cs`, `app/Matrix.cs`, `app/Fans.cs`, `app/Extra.cs`, `app/Slash.cs`, `app/Handheld.cs`). The exact on-disk format and schema have not been re-verified for this synthesis pass; Milestone 2 Story 5 (extracting shared logic, including configuration) should confirm and document the concrete format here.

### Secondary or App-Level Configuration

Not yet defined beyond the above. Revisit once the Shared layer's configuration boundary is confirmed.

### Secrets and Credentials

No secrets or external-service credentials are known to be part of this application's configuration surface. If any are discovered during the Shared extraction, document the handling approach here.

---

## Application Layers

### G-Helper.Shared (target, not yet extracted)

UI-independent hardware communication, device services, power/fan/battery/RGB logic, controller/input handling, and configuration. Consumed by both executable heads.

### G-Helper (existing WinForms head)

The current `app/` project. Remains the reference behavioural baseline throughout the split (Milestone 1 Story 3) and must keep compiling through Milestone 2 unless an explicit later decision retires it (Milestone 2 Story 7).

### G-Helper.WPF (new head)

The new executable head. Starts as a functional shell proving it can start, reference Shared, and run as a tray app (Milestone 2 Story 6), then reaches feature parity (Milestone 3 Story 8), then becomes the left-side ROG Ally overlay with the generalised binding system (Milestone 3, remaining stories).

---

## Security and Privacy

No specific security or privacy requirements have been identified beyond what the existing G-Helper application already implies (local hardware control, no known network/cloud data handling). Revisit if the Shared extraction or WPF work surfaces anything that changes this.

---

## Observability

### Logging

The existing application has a `Logger.cs` helper (`app/Helpers/Logger.cs`); behaviour and destination not yet re-documented here.

### Debugging

Not yet defined for the WPF head. The existing WinForms baseline (Milestone 1 Story 3) should record any environment-specific build/run setup needed for debugging.

### Health Checks

Not applicable at this stage; this is a desktop tray application, not a service.

---

## Testing Policy

Not yet defined. No test project currently exists in the repository. A concrete testing policy should be established once the `G-Helper.Shared` extraction (Milestone 2 Story 5) creates a natural seam for unit-testing hardware-independent logic; until then, validation is manual (build, launch, exercise on a ROG Ally), per Milestone 1 Story 3.

---

## Performance

Not yet a defined concern. Revisit once the WPF overlay shell (Milestone 3 Story 9) exists and its open/close responsiveness on the ROG Ally can be evaluated.

---

## Open Questions

- The design document's branch-strategy diagram names the development branch `G-Helper.WPF`, but the repository's actual current branch performing that role is named `wpf`. Confirm whether the branch should be renamed or whether `wpf` is the accepted name going forward, and update Milestone 1 Story 4 accordingly.
- The repository has an `origin` remote (`JarrydAdaens/g-helper-wpf`) but no distinct `upstream` git remote was found pointing at `seerge/g-helper`. Confirm whether this repository is registered as a GitHub fork of `seerge/g-helper` and whether a dedicated `upstream` remote should be added for Milestone 1 Story 1/4.
- The `G-Helper.Shared` component boundary (Section 3 of the source dictation) is a target shape, not a verified inventory. Milestone 2 Story 5 planning should derive the actual extraction boundary from the current `app/` source rather than assuming the listed components map one-to-one to existing files.
- The exact on-disk configuration format/schema used by `app/AppConfig.cs` and related settings files has not been re-verified for this design pass.

---

## Context Maintenance

Use Dictation to revise this design when the project vision changes. Do not leave important decisions stranded in raw notes, chats, or addenda. Promote durable decisions into this file, the Milestones Index, milestone docs, stories, or implementation plans as appropriate.

When an older design statement is superseded, update it directly and preserve only the rationale needed for future agents to understand the decision.

---

## Navigation

### Specification Hierarchy

- [dictations-tier-0/README-DICTATIONS-TIER-0.md](dictations-tier-0/README-DICTATIONS-TIER-0.md) - Dictation, raw and unstructured
- [design.md](design.md) - Design overview and Milestones Index
- [milestones/](milestones/) - Milestone documents, each directly containing its story list
- [backlog/](backlog/) - Story inventory / Milestone -1, the unscheduled staging pool
- `implementation-plans/*/` - Implementation Plans, optional Phases, and execution records

### Reference Documents

- [agenticworkflow.md](agenticworkflow.md) - AI collaboration workflow
- [agent-thinking.md](agent-thinking.md) - optional temporary agent scratchpad

### Wiki

- [wiki/home.md](wiki/home.md) - wiki navigation hub
