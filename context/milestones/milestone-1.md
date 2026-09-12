---
name: milestone-1
description: Milestone 1 - Repository and Context Setup. Establishes the fork, its agentic context, a validated behavioural baseline, and upstream synchronisation rules before any architectural change begins.
metadata:
  version: "3.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---
# Milestone 1: Repository and Context Setup

> Milestone. A coherent macro-feature or delivery outcome. (Tier numbers live only in `design.md` / `agenticworkflow.md`.)
>
> Related: [design.md (Milestones Index)](../design.md#milestones-index), [../backlog/](../backlog/)

---

## Intent

Establish a clean, reproducible fork of upstream G-Helper with the documentation, validated baseline, and upstream-sync rules required before any architectural change begins.

## Why it matters

- No later milestone can be trusted without a validated, working baseline to compare against.
- The fork intentionally rearranges source files in Milestone 2 onward; without upstream-sync rules established first, future upstream G-Helper changes become very difficult to absorb.
- This milestone changes no product behaviour, so it is the safest place to get process and documentation right.

## Outcome / Definition of Done

- The fork's relationship to upstream G-Helper is documented and clonable.
- Project context (design, milestones, backlog) exists and lets a future agent understand the intended end state without the original conversation.
- A clean checkout has been shown to build and launch, with baseline behaviour recorded.
- Branch strategy and the upstream migration/source-mapping document exist and are referenced from agent context.

## Status

In Progress — 2/4 stories complete (Stories 2 and 3 complete; Stories 1 and 4 open — see notes on each story).

---

## Story Index

| # | Story | Type | Complexity | Effort | Risk | Plan | Status |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | [Fork the original project](#story-1) | Docs/Tooling | — | — | — | *not yet generated* | Needs Verification |
| 2 | [Initialise project context](#story-2) | Docs | — | — | — | *not yet generated* | Complete |
| 3 | [Validate the clean baseline](#story-3) | Research | — | — | — | *not yet generated* | Complete |
| 4 | [Establish upstream synchronisation rules](#story-4) | Docs/Tooling | — | — | — | *not yet generated* | In Progress |

---

## Stories

<a id="story-1"></a>

### Story 1: Fork the original project

**Type:** Tooling

**Summary:**
Create the public project fork from the current G-Helper repository so this repository can receive future upstream updates.

**Why / value:**
Every later story depends on this repository actually being a fork of, and able to sync from, upstream `seerge/g-helper`.

**Rough scope:**
GitHub-level fork relationship and git remote configuration; no source changes.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-1/fork-the-original-project/plan.md`

**Status:** Needs Verification — the repository has an `origin` remote (`JarrydAdaens/g-helper-wpf`) and recent commits reference upstream `seerge/g-helper` issue numbers, but no distinct `upstream` git remote was found during initialization, and the GitHub fork relationship itself was not verified (no authenticated `gh`/API access at initialization time). See the Design [Open Questions](../design.md#open-questions).

---

<a id="story-2"></a>

### Story 2: Initialise project context

**Type:** Docs

**Summary:**
Create the initial project context required by the agentic workflow: high-level documentation describing project goals, architecture direction, the WPF migration, input-remapping goals, the milestone plan, implementation constraints, and user stories.

**Why / value:**
Lets later agents understand the intended end state without requiring the original conversation or dictation.

**Rough scope:**
`context/design.md`, `context/milestones/`, `context/backlog/`, `context/dictations-tier-0/` — this initialization pass.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** not applicable — this story is the context-initialization work itself, tracked directly here rather than via a separate implementation plan.

**Status:** Complete — this initialization pass preserved the source design document as Dictation, and populated Design (with Milestones Index), all three Milestone documents, and the Backlog to reflect it.

---

<a id="story-3"></a>

### Story 3: Validate the clean baseline

**Type:** Research

**Summary:**
Before structural changes begin, confirm that the forked upstream source restores dependencies, compiles, launches, and performs normally on the target ROG Ally, so it can serve as the behavioural baseline for later comparisons.

**Why / value:**
Milestone 2's extraction work needs a known-good baseline to diff against; without this, regressions introduced during the split are hard to distinguish from pre-existing behaviour.

**Rough scope:**
`app/GHelper.sln` restore/build; manual launch and exercise on a ROG Ally; record results and any environment-specific setup in `context/design.md` or the wiki.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-1/validate-the-clean-baseline/plan.md`

**Status:** Complete — a clean checkout of `app/GHelper.sln` restored, built (`dotnet build`, Debug, x64: 0 warnings, 0 errors), and launched successfully on the actual target hardware, an ASUS ROG Ally RC71L (confirmed via `Win32_ComputerSystem`). The main UI rendered correctly with live sensor data, and the runtime log showed normal startup with no unhandled exceptions; the only non-blocking issue was an expected elevation requirement for the Battery Charge Limit scheduled task. Full results recorded in [../wiki/baseline-validation.md](../wiki/baseline-validation.md).

---

<a id="story-4"></a>

### Story 4: Establish upstream synchronisation rules

**Type:** Docs/Tooling

**Summary:**
Set up the repository so future upstream G-Helper changes can continue to be consumed after the WPF fork substantially rearranges the codebase: preserve `main` for upstream sync, use a separate development branch, document the merge process, and create the migration/source-mapping document.

**Why / value:**
Without this, the fork's intentional source rearrangement in Milestone 2+ makes future upstream merges impractical.

**Rough scope:**
Branch strategy documentation; migration/source-mapping document (new file, likely under `context/wiki/` or a dedicated doc); Pipeline 2 in `context/design.md` already records the intended merge process.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-1/establish-upstream-synchronisation-rules/plan.md`

**Status:** In Progress — the repository's current branch, `wpf`, already serves as the development branch the design document calls `G-Helper.WPF` (see the naming open question in [design.md](../design.md#open-questions)), and `context/design.md` now documents the intended upstream-merge pipeline. The migration/source-mapping document itself (design doc Section 11) does not exist yet.

---

## Interdependency Order

1. Story 1 (fork exists) should be confirmed before Story 4 (upstream sync rules) can be considered complete, since sync rules depend on a known upstream relationship.
2. Story 2 (this initialization) does not block Story 3 or Story 4, but both should read the resulting Design/Milestones before proceeding.
3. Story 3 (validated baseline) should complete before Milestone 2 Story 5 (extract reusable business logic) begins, so regressions introduced by extraction can be identified against a known-good baseline.

---

## Backlog Sources

- No backlog themes were pulled into this milestone. All four stories were mapped directly from the initial dictation (design document Section 13, Milestone 1) during project initialization, with high confidence.

---

## Deferred / Follow-up Work

- Verifying the GitHub fork relationship and deciding on an `upstream` remote (Story 1) is deferred pending user confirmation or authenticated GitHub access.
- Renaming the `wpf` branch to match the design document's `G-Helper.WPF` naming, or confirming `wpf` as final, is deferred to Story 4 follow-up.

---

## Notes

- Keep this Story Index in sync with the [Milestones Index](../design.md#milestones-index) in Design.
