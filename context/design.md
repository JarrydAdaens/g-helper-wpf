---
name: design
description: Design specification template defining project architecture, principles, constraints, the context tier system, and the embedded milestones index.
metadata:
  version: "3.0"
  agentic_rails_source_version: "3.0"
  owner: "Your Name"
  repo: "your-repo"
---
# Project Name - Design Specification

## Purpose of This File

This repository is an intentionally empty starter for future projects. Replace the placeholders in this file when you fork or copy the template into a real codebase.

This file is the Design tier: the maintained design specification covering the whole deliverable and how it breaks into its largest pieces. It should synthesize relevant Dictation into stable project direction. The **Milestones Index** lives as a subsection of this file (see below); the actual Milestone documents are separate files under `milestones/`.

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
| Milestone 1: *Name* | [milestones/milestone-1.md](milestones/milestone-1.md) | Not Started | *Why this milestone matters* | *What completing it unlocks* |

Keep this index in sync as milestones are added, completed, reordered, or reclassified. When a backlog story scores as epic-sized, promote it into this index as a new milestone.

---

## Executive Summary

Describe the project this repository will become:

- what it is
- who it is for
- what problem it solves
- why this architecture is appropriate

### Core Principles

- **Principle 1:** Replace with a guiding architectural principle.
- **Principle 2:** Replace with a second principle.
- **Principle 3:** Replace with a third principle.
- **Principle 4:** Add more only when they are truly decision-driving.

---

## System Architecture

### How the Pieces Fit Together

Describe the major components of the system, how they interact, and what boundaries matter.

**External services and dependencies:**

- `Service or dependency` - purpose
- `Service or dependency` - purpose

### Repository Structure

Update this tree as the project takes shape.

```text
project-root/
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
|-- source/
|-- tests/
|-- scripts/
`-- README.md
```

---

## Processing Pipelines

> Delete this section if the project has no defined pipelines. Add it back when the system's data flows are known.

Describe the main data flows or user journeys only after the project has enough shape to justify them.

### Pipeline 1

1. Step 1
2. Step 2
3. Step 3

### Pipeline 2

1. Step 1
2. Step 2
3. Step 3

---

## Configuration

Document configuration only once the real stack is known.

### Primary Configuration

Describe the main project configuration surface.

### Secondary or App-Level Configuration

Describe any machine-level or environment-level settings.

### Secrets and Credentials

Document the real secret-management approach here and in the wiki once chosen.

---

## Application Layers

List only the layers the project actually uses.

### Layer 1

Purpose and architecture summary.

### Layer 2

Purpose and architecture summary.

---

## Security and Privacy

- Define how data is stored and protected.
- Define the expected secret-management strategy.
- Define any privacy or compliance constraints.

---

## Observability

### Logging

Describe logging approach.

### Debugging

Describe debugging tools or diagnostic outputs.

### Health Checks

Describe any health checks or operational diagnostics.

---

## Testing Policy

Define what kinds of tests are expected and when they are required.

---

## Performance

If performance is not yet a concern, say so explicitly and revisit it later.

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
