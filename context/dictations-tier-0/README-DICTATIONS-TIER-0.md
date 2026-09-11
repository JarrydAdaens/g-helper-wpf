---
name: dictations-tier-0-readme
description: Dictation guidance for raw dictation, rough project seeds, and recurring vision updates before any structure is imposed.
metadata:
  version: "3.0"
  agentic_rails_source_version: "3.0"
  owner: "Your Name"
  repo: "your-repo"
---
# Dictation

Dictation is the raw source layer for the context tier system. It holds a brain-dump of the rough shape of a project — messy human intent before any structure is imposed and before that intent has been synthesized into maintained design, milestone, story, or implementation-plan context. A transcription is typically the output of one to several hours of just talking.

Use this folder for durable transcription such as:

- initial dictated project seed documents (the rough shape of the whole project)
- voice-note transcripts from AI conversations
- supplemental design addenda
- scope changes, milestone splits, deleted milestones, or reordered work
- notes that explain why an Implementation Plan needs to be regenerated or revised

Do not treat Dictation as authoritative after synthesis. Once a transcription changes project direction, promote the accepted decision into the maintained context files:

- Design: `../design.md` (including its Milestones Index)
- Milestone: `../milestones/`
- Backlog (story inventory / Milestone -1): `../backlog/`
- Implementation Plan: `../implementation-plans/`

## Naming Guidance

Prefer filenames that sort by date and describe the source:

- `YYYY-MM-DD_initial_project_seed.md`
- `YYYY-MM-DD_addendum_<short-topic>.md`
- `YYYY-MM-DD_scope_revision_<short-topic>.md`
- `YYYY-MM-DD_milestone_reorder_<short-topic>.md`

Keep names short, stable, and specific enough for an agent to cite from a story or implementation plan.

## Intake Template

```md
# <Date> - <Short Topic>

## Source

- Captured from: <voice notes, AI chat, meeting, personal notes, etc.>
- Related project area: <milestone, story, subsystem, or unknown>

## Raw Notes

Paste or summarize the raw dictated material here.

## Important Signals

- New decision:
- Changed assumption:
- New risk:
- Open question:
- Possible milestone or story impact:

## Integration Notes

- Update `../design.md` (and its Milestones Index):
- Update `../milestones/`:
- Update `../backlog/`:
- Update `../implementation-plans/`:
```

## Agent Rules

- Read Dictation only when the task involves transcription synthesis, project enrichment, scope revision, or rationale recovery.
- Preserve uncertainty instead of turning rough notes into fake certainty.
- Ask or record questions when Dictation contradicts maintained context.
- After synthesis, link the Dictation source only where the raw source remains useful for traceability.
- Do not store secrets, credentials, or private information here unless the project has explicitly defined a safe handling policy.
