---
name: wiki-home
description: Wiki hub for context-oriented reference material and cheat sheets.
metadata:
  version: "2.0"
  agentic_rails_source_version: "2.0"
  owner: "Your Name"
  repo: "your-repo"
---

# Project Wiki

[Back to Design Specification](../design.md)

This wiki folder is optional supporting documentation for operational notes that do not belong in the authoritative context tier system.

Authoritative project context lives in:

- Dictation: [../dictations-tier-0/](../dictations-tier-0/)
- Design (+ Milestones Index): [../design.md](../design.md)
- Milestone: [../milestones/](../milestones/), each directly containing its story list
- Backlog (story inventory / Milestone -1): [../backlog/](../backlog/)
- Implementation Plan and optional Phases: `../implementation-plans/*/`

## Context as a Working Language

In an agentic IDE, context is one of the programming languages of the workflow. Markdown, YAML, XML documentation comments, and Doxygen comments are not just file formats. They are ways of expressing structure, intent, constraints, examples, and meaning so both humans and agents can reason over them clearly.

Wielding context thoughtfully has a direct effect on the quality of outcomes. Clear headings, stable structure, explicit examples, and deliberate wording make the difference between vague guidance and reliable execution. Good context does not just describe the work. It shapes what the agent can understand, retain, and do well.

## Wiki Guidance

- Add pages only when they provide durable reference value.
- Prefer linking from here rather than scattering wiki pages without navigation.
- Remove placeholder sections you do not need.
- Verify wiki content against source-of-truth docs before relying on it.
- Do not let wiki notes override design, milestones, goals, or implementation plans.

## Cheat Sheets

- [doxygen_cheat_sheet.md](doxygen_cheat_sheet.md) - reference guide for Doxygen comment syntax, grouping, linking, and generated docs features
- [markdown_cheat_sheet.md](markdown_cheat_sheet.md) - reference guide for core Markdown syntax and major flavours
- [mermaid_chart_cheat_sheet.md](mermaid_chart_cheat_sheet.md) - reference guide for Mermaid class diagram syntax, relationships, and styling
- [xml_documentation_comments_cheat_sheet.md](xml_documentation_comments_cheat_sheet.md) - reference guide for C# XML documentation comments and common tags
- [yaml_cheat_sheet.md](yaml_cheat_sheet.md) - reference guide for YAML structure, scalars, collections, and common parser caveats

## Other Pages

- [secrets.md](secrets.md) - placeholder guidance for documenting secret-management decisions
