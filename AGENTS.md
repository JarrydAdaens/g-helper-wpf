---
name: agents
description: Mandatory repository-specific instructions that tell AI coding agents how to load context, work within scope, and follow project standards.
metadata:
  version: "3.1"
  agentic_rails_source_version: "3.1"
  owner: "Your Name"
  repo: "your-repo"
---
# Agent Instructions

This file provides mandatory guidance for AI coding agents. Follow these rules in every interaction.

---

## 0: Required Startup Workflow

Before making changes, load context in this order:

### 0.1: Repository Context Order

1. Review `context/laws.md`. This is the constitutional authority for code quality, security, and architectural constraints. Laws defined there are inviolable and override any conflicting guidance in this file.
2. Review `context/agenticworkflow.md`. This is the mandatory workflow standard for the context tier system.
3. Review `context/design.md`, including its embedded Milestones Index. This is always required baseline context.
4. Review the relevant `context/milestones/*.md` when the task touches roadmap, scope, or milestone planning, then the relevant story section inside that milestone.
5. Review the relevant `context/implementation-plans/<milestone-slug>/<story-slug>/plan.md` when one exists.
6. Review `context/backlog/*.md` only when selecting or recovering unscheduled work.
7. Review `context/dictations-tier-0/` files only when the task involves raw dictation synthesis, context enrichment, scope revision, or rationale recovery.
8. Review optional temporary artifacts (e.g. `context/agent-thinking.md`) only when explicitly relevant.
9. If continuing phased work, review the relevant handover document in `context/implementation-plans/` first, as directed by `context/agenticworkflow.md`.
10. When work involves drafting commit logs or preparing to commit, use the installed `commit-log` skill if available. By default, output the commit log in chat for the user to copy. Only perform an actual commit when the user explicitly asks for one.

## 0.2: Repository Expectations

- Follow the workflow defined in `context/agenticworkflow.md`.
- Treat `context/design.md` (with its Milestones Index) as always-required context.
- Use `context/milestones/` (each milestone directly containing its story list) and `context/backlog/` (the story inventory / Milestone -1) to anchor implementation scope and acceptance criteria.
- Never use `goal` as a tier name (it is retired) and never use `task` as a tier name. The work unit is a Story.
- Treat `context/dictations-tier-0/` as raw dictation that must be synthesized into maintained docs before it becomes authoritative.
- Review `harness/README-HARNESS.md` and any module whose documented trigger matches the task; `harness/` holds the verifiers, gates, guardrail seams, sensors, and actuators for this project.
- Run matching `harness/` gates and verifiers when task scope triggers them, using an independent evaluator sub-agent when available. A claim about visible or runtime behavior requires the matching harness check to pass, or a documented reason it could not run.
- Use relevant installed shared rules, skills, workflow-skills, and specialist agents when they are available and task-appropriate.
- Repository-local instructions override reusable external tooling when they conflict.
- Do not execute pull or push operations in any VCS.
- You may stage and commit changes when explicitly requested. Push is never permitted.
- When drafting a commit log or creating a commit, use the `commit-log` skill, and default to outputting the commit log in chat unless the user explicitly asks you to create the commit.

## 0.3: Optional Project Versioning

This starter is language- and framework-agnostic. When a generated project has first-party assemblies, packages, apps, or deployable artifacts with shared version properties, define the versioning policy in project-specific documentation.

Recommended context-driven version shape:

| Part | Meaning |
| --- | --- |
| Major | Significant product release the organization treats as major. |
| Milestone | Current milestone number from `context/milestones/`, or the highest milestone actively in progress. |
| Build or Reserved | Build ordinal, CI value, or `0` until automation exists. |

If a project adopts this policy, document the exact files to update and keep every first-party version property aligned when milestone scope changes.

---

## 1. Agent Behaviour

### 1.1 Think Before Coding

Do not assume. Do not hide confusion. Surface tradeoffs.

Before implementing:

- State assumptions explicitly. If uncertain, ask.
- If multiple interpretations exist, present them instead of choosing silently.
- If a simpler approach exists, say so.
- Push back when the requested approach conflicts with repository laws, scope, or maintainability.
- If something is unclear and cannot be resolved from context, stop and ask.

### 1.2 Simplicity First

Minimum code that solves the problem. Nothing speculative.

- No features beyond what was asked.
- No abstractions for single-use code.
- No flexibility or configurability that was not requested.
- Do not expose internal tuning knobs as user-facing controls unless the user explicitly asked for them.
- No error handling for impossible scenarios.
- If you write 200 lines and it could be 50, rewrite it.

Ask yourself: "Would a senior engineer say this is overcomplicated?" If yes, simplify.

### 1.3 Surgical Changes

Touch only what you must. Clean up only your own mess.

When editing existing code:

- Do not improve adjacent code, comments, or formatting unless needed for the task.
- Do not refactor things that are not broken.
- Match existing style, even if you would do it differently.
- If you notice unrelated dead code, mention it instead of deleting it.

When your changes create orphans:

- Remove imports, variables, files, and functions that your changes made unused.
- Do not remove pre-existing dead code unless asked.

The test: every changed line should trace directly to the user's request.

### 1.4 Goal-Driven Execution

Define success criteria. Loop until verified.

Transform tasks into verifiable goals:

- "Add validation" -> "Write tests for invalid inputs, then make them pass"
- "Fix the bug" -> "Write a test that reproduces it, then make it pass"
- "Refactor X" -> "Ensure tests pass before and after"

For multi-step tasks, state a brief plan:

```text
1. [Step] -> verify: [check]
2. [Step] -> verify: [check]
3. [Step] -> verify: [check]
```

For modal, editor, workflow, or stateful features, define both the activation path and the cleanup path up front. If loading or enabling one feature automatically turns on another, clearing or disabling it must explicitly unwind that state.

Strong success criteria let you loop independently. Weak criteria require clarification.

### 1.5 Comments

- Write comments that explain why, not what.
- A few high-level comments explaining the purpose of classes, modules, or methods can be helpful.
- Comments explaining tricky or non-obvious code are helpful.
- Avoid comments that are redundant with the code.
- Use formal API documentation only when the interface is public-facing or important to consumers.

---

## 2. Refactoring Rules

Immediate refactoring is required when:

- A class, module, or file grows beyond a single responsibility.
- Methods or functions perform multiple actions.
- Logic is duplicated across the project.
- Large conditional blocks control behavior that should be owned by clearer components.

Refactoring must prioritize:

- Composition over inheritance.
- Small, focused components.
- Clear ownership of responsibilities.

Apply the Rule of Three: refactor after something is duplicated three times, not before.

Before adding significant functionality to a large or monolithic file, consider a structural refactor with no intended behavior changes first. Splitting a file into smaller, composable parts before feature work is often safer than mixing the refactor and the new behavior in one step. Verify the refactor independently before layering new functionality on top.
