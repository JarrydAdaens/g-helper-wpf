# G-Helper.WPF

A public fork of [G-Helper](https://github.com/seerge/g-helper) — the lightweight Armoury Crate alternative for Asus laptops — rebuilt around a WPF front end, with particular focus on the ROG Ally handheld experience.

> The upstream project's own README (features, supported models, FAQs, downloads) lives at [docs/README.md](docs/README.md) and is kept as upstream content for provenance and future merges. This file is about the fork itself.

## What this fork changes

- Splits UI-specific code from reusable hardware/business logic so a WinForms head and a new WPF head can both exist on top of the same shared logic.
- Replaces the WinForms floating window with a left-anchored, handheld-friendly overlay shell for the ROG Ally, in the spirit of the pre-Game-Bar Armoury Crate SE overlay.
- Replaces the small fixed list of configurable buttons with an extensible, user-created input-binding system — physical inputs can be remapped to other gamepad actions, keyboard keys, or mouse clicks — plus exposes the existing touch-keyboard gesture as a normal bindable action.

The first goal is not a redesign: it is functional parity with the existing WinForms G-Helper, achieved by separating logic from UI, before any new WPF-specific behaviour is added.

## Status

Early-stage fork. Milestone 1 (repository and context setup) is complete; the WinForms application (`app/`) is still the only working executable head. See [Current Status](#current-status) below for what's been verified.

## Getting Started

Open `app/GHelper.sln` in Visual Studio and build/run `app/GHelper.csproj` — this is currently the same WinForms application as upstream G-Helper. No separate WPF project or shared class library exists yet; both are introduced in Milestone 2. See [context/wiki/build-and-run.md](context/wiki/build-and-run.md) for a command-line quickstart.

## Current Status

- Confirmed as a GitHub fork of `seerge/g-helper`, with an `upstream` remote configured and verified reachable. See [context/wiki/upstream-source-mapping.md](context/wiki/upstream-source-mapping.md).
- A clean-checkout build/launch pass has been recorded as a baseline on a real ROG Ally RC71L. See [context/wiki/baseline-validation.md](context/wiki/baseline-validation.md).

See [context/milestones/milestone-1.md](context/milestones/milestone-1.md) for the current story-by-story status.

## Project Context

This repository uses the Agentic Rails context tier system to keep AI coding agents (and humans) aligned on scope and direction across sessions:

- [context/design.md](context/design.md) — architecture, principles, and the Milestones Index
- [context/milestones/](context/milestones/) — the roadmap, one file per milestone, each containing its own stories
- [context/backlog/](context/backlog/) — the unscheduled story inventory
- [AGENTIC_RAILS_README.MD](AGENTIC_RAILS_README.MD) — what Agentic Rails is
- [AGENTS.md](AGENTS.md) — mandatory agent operating rules for this repository
