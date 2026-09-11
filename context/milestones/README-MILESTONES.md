---
name: milestones-readme
description: Guidance for the Milestone documents, each of which directly contains the story list needed to deliver a coherent macro-feature or delivery outcome.
metadata:
  version: "3.0"
  agentic_rails_source_version: "3.0"
  owner: "Your Name"
  repo: "your-repo"
---
# Milestones

> Milestone. A coherent macro-feature or delivery outcome. (Tier numbers live only in `design.md` / `agenticworkflow.md`.)

A **Milestone** is a coherent macro-feature or delivery outcome (for example "Dockerize the app", "Port the game to a new platform", or "Get the repo ready to move on"). Each milestone document **directly contains the story list** needed to deliver it. There are multiple milestone documents; one file per milestone.

The high-level **Milestones Index** (the table of contents pointing at these files) lives inside [design.md](../design.md), because a milestones table is the same tier as Design. The files in this folder are the milestones themselves, each containing its own Story Index.

## How milestones are produced

1. After the rough Design exists, fragment it into milestones **conversationally** with the human. Propose a breakdown, then negotiate the count and shape until it fits the human's capacity and priorities.
2. Milestones are grouped around large-scale deliverables. A milestone generally contains roughly 10 to 20 stories, as a guideline rather than a hard rule.
3. Milestone planning searches the backlog (story inventory / Milestone -1) for stories relevant to the milestone's theme, pulls them into the milestone document, and sequences them by interdependency.

## Files

- [MILESTONE_TEMPLATE.md](MILESTONE_TEMPLATE.md) - copy this for each new milestone.
- [milestone-1.md](milestone-1.md) - starter example milestone.

## Rules

- One file per milestone. Keep milestones thematic, not granular.
- Each milestone directly contains its Story Index; stories do not live in a separate scheduling layer.
- Keep the Milestones Index in [design.md](../design.md) in sync with the files here.
- When a backlog story scores as epic-sized, reclassify it upward into a new milestone here and in the Design index.
- The system is progressive: complete a milestone and move on rather than perpetually re-opening old milestones.
