---
name: backlog-template
description: Backlog file template (story inventory / Milestone -1). Holds up to 30 stories, priority-sorted, ready for milestone planning to pull from.
metadata:
  version: "3.0"
  agentic_rails_source_version: "3.0"
  owner: "Your Name"
  repo: "your-repo"
---
# Backlog N

> Story inventory / Milestone -1. This is an unscheduled pool of possible work, not a formal tier. Maximum **30 stories** per file; overflow spawns the next backlog file.
>
> Front files hold the most urgent, concrete, doable work. Back files hold wishful, experimental, least-concrete work. The backlog is evergreen and allowed to be messy.

---

## Story Index

<!-- Keep highest priority at the top. Status: Backlog, Pulled into milestone, In Progress, Complete, Reclassified. -->

| # | Story | Type | Priority | Complexity | Effort | Risk | Milestone | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | [Story name](#story-1) | Feature/Bug/Refactor | High | — | — | — | *unscheduled* | Backlog |
| 2 | [Story name](#story-2) | Feature/Bug/Refactor | Medium | — | — | — | *unscheduled* | Backlog |

Scores are filled in during milestone planning. An unscored story shows `—`.

---

<!-- ============================================================ -->
<!-- STORY TEMPLATE                                                -->
<!-- Copy the block below for each story. Keep it short until      -->
<!-- the story is scheduled; refine it close to execution.         -->
<!-- ============================================================ -->

<a id="story-1"></a>

### Story: *Name*

**Type:** Feature | Bug | Refactor

**Summary:** One or two short paragraphs describing the discrete unit of work. This is the *story summary* the risk judge evaluates — keep it cheap to read.

**Why / value:** Why this is worth doing and how urgent it is.

**Rough scope:** What it touches. Enough to size it, not a full plan.

**Scores (filled at planning):**

- Complexity: *reasoning demand*
- Effort: *volume — files, LOC, modules*
- Risk: *uncapped headline + sub-scores (technical, architectural, dependency, ...)*

**Reclassification check:** If scoring reveals this is epic-sized, promote it to a [Milestone](../milestones/) and remove it from the backlog.

---

<a id="story-2"></a>

### Story: *Name*

**Type:** Feature | Bug | Refactor

**Summary:** *...*

**Why / value:** *...*

**Rough scope:** *...*

**Scores (filled at planning):**

- Complexity: —
- Effort: —
- Risk: —
