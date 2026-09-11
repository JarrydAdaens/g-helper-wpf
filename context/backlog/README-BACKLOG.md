---
name: backlog-readme
description: Guidance for the story inventory / Milestone -1, the evergreen, prioritized pool of discrete work units that milestone planning pulls into milestone documents.
metadata:
  version: "3.0"
  agentic_rails_source_version: "3.0"
  owner: "Your Name"
  repo: "your-repo"
---
# Backlog

> Story inventory / Milestone -1. This is an unscheduled pool of possible work, not a formal tier.

A **Story** is one discrete work unit: a bug fix, a feature, or a refactor. The backlog is the pool of stories, built once from Design + Milestones and then refined continuously. Milestone planning pulls stories from here into milestone documents.

## Backlog rules

- **30-story cap per file.** Each backlog file holds a maximum of 30 stories; overflow spawns a new backlog file (`backlog-1.md`, `backlog-2.md`, ...).
- **Priority sorting front-to-back.** Most urgent, important, concrete, and doable stories go in the **front** files; wishful, experimental, least-concrete, least-urgent stories sink to the **back**.
- **Evergreen and messy by design.** The backlog is not meant to be perfectly organized end to end. It can represent six months to several years of work. Do not try to maintain perfect foresight.
- **Reclassify mis-sized stories.** A story that, once scored, reveals itself to be epic-sized is actually a **Milestone**. Promote it into a milestone (see [../milestones/](../milestones/)) and the [Milestones Index](../design.md#milestones-index) rather than forcing it through as a story.
- **Supplement freely.** Add stories as understanding improves.

## Scoring

Stories are scored on three orthogonal metrics before model assignment: **Complexity** (reasoning demand), **Effort** (volume), and **Risk** (uncapped headline + sub-scores). Scoring is performed by right-rail tooling and recorded when a story is pulled into a milestone or planned. See [agenticworkflow.md](../agenticworkflow.md#scoring-model-complexity-effort-risk).

## Files

- [BACKLOG_TEMPLATE.md](BACKLOG_TEMPLATE.md) - copy this to start a new backlog file.
- [backlog-1.md](backlog-1.md) - the front (highest-priority) backlog file.

## Flow

1. Build the backlog once from Design + Milestones.
2. Sort it front-to-back by priority.
3. Milestone planning searches the backlog by milestone theme, gathers stories, scores them, and pulls them into the relevant [../milestones/](../milestones/) document.
4. A promoted story marks its original backlog entry as moved, scheduled, promoted, or superseded to avoid duplicating active truth.
5. A story ready to run gets an Implementation Plan under [../implementation-plans/](../implementation-plans/).
