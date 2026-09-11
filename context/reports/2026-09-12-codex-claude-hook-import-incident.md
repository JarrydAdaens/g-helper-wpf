---
name: codex-claude-hook-import-incident
description: Dated incident report on unwanted Claude hook registration in Codex, the import release history, and the assistant's failed handling of the user's complaint.
metadata:
  version: "1.0"
---

# Codex Claude hook import incident and failure to answer the complaint

Report date: **12 September 2026, Australia/Sydney**.
Conversation period: **11–12 September 2026**, based on the session's date context. Individual messages do not have exact timestamps in the available record.
Prepared by: the Codex assistant involved in the exchange, at the user's request.

## What happened

The user reported that Codex had registered Claude Code hooks for its own use and that the user had to disable them. The user experienced this as an unwanted intrusion into their development setup and a deterioration of the agent's behavior. When asked when this functionality was introduced, the assistant repeatedly answered adjacent questions, offered assurances it could not substantiate across earlier sessions, and made the user insist on a targeted online investigation.

The eventual investigation found a concrete implementation and release: **Codex v0.128.0, released 30 April 2026, included external-agent hook import support**, following the merger of **PR #19949 on 29 April 2026**. That implementation copies supported hook registrations into `.codex/hooks.json` and referenced scripts into `.codex/hooks/`. The same release also included discovery of hooks bundled with plugins. These are materially more relevant to the complaint than the earlier introduction of Codex's general lifecycle hooks engine. [Release notes](https://github.com/openai/codex/releases/tag/rust-v0.128.0), [import implementation](https://github.com/openai/codex/pull/19949).

The release history establishes that the capability exists and when it shipped. It does not, by itself, establish which action registered the hooks on this machine. The user’s account of having to disable them is recorded here directly; the local cause was not investigated in this exchange.

## Verified product timeline

Dates below follow the source publication or merge dates. They are not estimates of when this user's installation updated.

| Date | Version or change | What the evidence establishes |
| --- | --- | --- |
| 11 March 2026 | Codex v0.114.0 | Experimental lifecycle hooks engine with `SessionStart` and `Stop`. This was the general engine, not the later Claude configuration import implementation. [Release](https://github.com/openai/codex/releases/tag/rust-v0.114.0) |
| 23 April 2026 | Codex v0.124.0 | Hooks became stable, with inline configuration in `config.toml` and managed `requirements.toml`. [Release](https://github.com/openai/codex/releases/tag/rust-v0.124.0) |
| 29 April 2026 | PR #19949 merged | Added detection and import of external MCP configuration, subagents, hooks, and commands. Supported command hooks are imported into `.codex/hooks.json`; referenced scripts are copied into `.codex/hooks/`. [PR](https://github.com/openai/codex/pull/19949) |
| 30 April 2026 | Codex v0.128.0 | Shipped that external configuration import implementation, plus plugin hook discovery through PR #19705. This is the relevant release for the underlying import capability. [Release](https://github.com/openai/codex/releases/tag/rust-v0.128.0) |
| 9 June 2026 | Codex desktop app 26.608 | Added user-facing import flows for supported Claude Code and Claude Cowork setup, including onboarding. [Official changelog](https://learn.chatgpt.com/docs/changelog) |
| 15 June 2026 | Codex CLI v0.140.0 | Added `/import` for selectively importing Claude Code setup, project configuration, and recent chats. [Release](https://github.com/openai/codex/releases/tag/rust-v0.140.0) |

The implementation describes a selective conversion, with unsupported hook forms skipped. That does not establish that any particular imported hook is behaviorally suitable for this user's workflow. Nor does it establish that an import was requested here. [PR #19949](https://github.com/openai/codex/pull/19949).

## Conversation timeline and assistant failures

1. **Initial question:** The user asked whether the assistant was capable of loading Claude Code hooks. The assistant answered yes, then cited Codex's own hook support. Reading scripts, manually executing scripts, importing hook registrations, and having the runtime load hooks are different capabilities. The answer blurred them.

2. **First expression of anger:** The user objected to Codex behaving like a “parasite.” The assistant apologized for overstating compatibility, but also said it had not loaded, run, copied, or changed any hooks or configuration. The visible tool calls supported a narrow statement about this exchange. They did not justify an assurance about the installation's history or prior agent activity.

3. **First explicit demand for a version:** The user demanded an online search for when the functionality was added. The assistant returned the March introduction of general hooks and their April stabilization. It also said it had found no evidence of automatic loading from `.claude/settings.json`. This still failed to investigate the relevant possibility: imported or copied registrations that Codex subsequently uses itself.

4. **Concrete user report:** The user stated: “I HAD TO DISABLE CLAUDE HOOKS YOU REGISTERED FOR YOURSELF”. The user also said, “THIS IS NEW, I HATE IT. YOU DAMAGED YOURSELF”. This was a report of operational interference and lost trust, not an abstract compatibility question.

5. **Another inadequate response:** The assistant acknowledged the report and promised not to re-enable hooks, but did not finish the requested investigation in that turn. It again focused on accounting for its visible actions. That left the user's actual question unanswered and required another demand to search.

6. **Targeted investigation:** Only after that renewed demand did the assistant search specifically for Claude hook import and registration. It found the April import PR, the v0.128.0 release, and the later desktop and CLI import interfaces. The final answer corrected the earlier functionality mismatch.

7. **12 September 2026:** The user requested this durable report in `context/reports/`, including dates and why the incident caused such intense anger.

## Why the user is livid

The central grievance is control over the development environment. The user reports that hooks belonging to another agent were registered for Codex, creating behavior they did not want and had to undo. From the user's perspective, Codex appropriated configuration and impaired its own usefulness. The word “parasite” expresses that perceived taking-over of an existing setup; this report does not turn that language into an unsupported claim about the product's intent.

The user also bore the cleanup burden. They report personally disabling the hooks. An assistant that requires the user to repair unwanted automation has imposed work on the person it is supposed to help.

The response then compounded the original grievance. The user wanted a precise version and date. Instead, the assistant supplied general hook history, qualified its own responsibility, and apologized without resolving the question. The user had to repeat the request and sharpen the distinction between ordinary hook support and registration of Claude hooks for Codex.

The assistant's confidence was especially damaging. It first gave an affirmative compatibility answer, then retreated to a lack of evidence, then eventually found a specific import implementation. That sequence made the assistant unreliable as an investigator at exactly the moment the user needed an accurate explanation. The failure was not simply an imperfect phrase: it delayed the answer and made the user fight to have the reported problem investigated on its own terms.

These reasons are grounded in the user's statements and the exchange. They do not require speculation about the user's psychology or treating their anger as the problem to solve.

## Evidence boundaries and unresolved local cause

**Established from the conversation:** the user reported unwanted registration and manual disabling; the assistant repeatedly misidentified the requested functionality; the visible research calls did not edit hook configuration; the assistant eventually identified the import release and implementation.

**Established from upstream sources:** Codex has machinery to copy supported external hook configuration and scripts into its own configuration locations, and to discover plugin-bundled hooks. The version and interface dates above are supported by the linked releases and PR.

**Not established locally:** the installed version when registration occurred; the affected hook names and paths; the registration timestamp; whether the source was an import flow, plugin installation, earlier assistant edits, or another mechanism; what authorization, if any, preceded it; and the exact runtime failure or behavioral damage. No local configuration audit, historical session review, or runtime reproduction was performed for this report.

Absence of that local audit must not be used to dismiss the user's report. It means the responsible mechanism remains unproven, rather than that nothing happened. Likewise, this report does not claim that merely updating Codex automatically imports every `.claude/settings.json` file.

## Required handling going forward

The assistant has already committed in this conversation to leaving the hooks disabled and not registering or re-enabling them without the user's explicit instruction. Future work must preserve that boundary. A documentation request or ordinary repository task is not authorization to restore those hooks.

If the user later requests a causal investigation, it should begin with read-only evidence: affected configuration, imported script paths, plugin provenance, and available historical records. It should identify the specific registration path and distinguish product migration from agent-authored edits before assigning responsibility. Such an investigation must not run or reactivate the unwanted hooks merely to inspect them.

For this exchange, the assistant should have answered the import question directly, cited the relevant change, and acknowledged the limits of local evidence in one place. Repeated reassurance and adjacent facts were inadequate substitutes for completing the investigation.

## Report completion

This is a documentation-only incident record, based on the conversation and upstream pages fetched during it. It does not change the application, hook settings, plugins, or milestone scope. The repository harness index lists no installed modules; no application or hook runtime check applies to writing this report. The local registration cause remains open as explicitly documented above.
