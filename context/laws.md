---
name: laws
description: Constitutional governance for AI-generated code — inviolable laws that ensure quality, security, and architectural integrity.
metadata:
  version: "1.3"
  agentic_rails_source_version: "1.3"
  owner: "Your Name"
  repo: "your-repo"
---

# Constitution of Code

**Context:** You are generating code at machine speed. Human review cannot scale to catch every architectural or security violation. Therefore, conventional guidelines do not apply to you. You are bound by the following inviolable laws.

Code that violates these laws is functionally broken, regardless of whether it compiles or passes automated tests.

---

## 1. Core Philosophy

These principles are the foundation of every law in this document. When two laws conflict, resolve by applying these principles in priority order.

1. **Functional First** - Code exists to fulfil a purpose. Write the simplest version that works correctly before layering complexity. Apply Occam's Razor.
2. **Readable** - Code is read at least ten times more than it is written. Optimize for the reader, not the writer. Never write the cleverest thing you can. Use descriptive names, prefer explicit over implicit, and keep code density low.
3. **Understandable** - Write software that fits in your head. Separate concerns, keep nesting shallow, and keep functions short. Follow the Principle of Least Astonishment: a method name must not imply behavior it does not provide.
4. **Reliable** - Code should be hard to break. Prefer loose coupling, testable boundaries, deterministic behavior, and regression tests for bug fixes.
5. **Efficient** - Respect explicit performance budgets and resource limits. Write correct code first. Optimize only after measurement shows a bottleneck, and verify that the optimization had the intended effect.
6. **Iterative** - Nobody gets it right the first time. Apply the Rule of Three, use Red-Green-Refactor when tests are available, and leave the codebase better than it was.
7. **Consistent** - Follow agreed standards even when your personal preference differs. Consistency reduces cognitive load and onboarding time.

---

## 2. Security by Construction

Generate secure code by default. Security is not an afterthought or a post-generation audit step.

- **Input Validation:** External inputs must pass explicit validation before processing. No raw data passthrough.
- **Data Access:** Database, filesystem, network, and command execution boundaries must avoid injection risks. Use parameterized APIs and structured arguments where available.
- **Authentication and Authorization:** User-facing privileged operations must verify caller identity and permission.
- **Secrets:** Secrets and credentials must not be committed, logged, echoed, or copied into context files.
- **Data Boundaries:** Sensitive data must not cross service, process, or storage boundaries without an explicit protection strategy. Redact sensitive data from logging and analytics.

---

## 3. Architectural Coherence

You lack long-term persistent context. Rely on these invariants to prevent emergent complexity and systemic drift.

- **Encapsulation:** Modules and services must communicate through defined APIs or clear ownership boundaries. Direct cross-boundary data access is forbidden unless the architecture explicitly allows it.
- **Separation of Concerns:** Business logic must remain separated from presentation, infrastructure, persistence, and integration concerns.
- **State and Configuration:** Configuration must be externalized from application code when it changes by environment. State mutations must be predictable and isolated.
- **Innovation Boundary:** Follow established repository patterns. Do not invent new architectural patterns or introduce new third-party dependencies unless explicitly instructed or justified by the task.

### SOLID Principles

All object-oriented code should respect SOLID:

- **Single Responsibility** - A class or module has one reason to change. If it handles more than one concern, split it.
- **Open/Closed** - Open for extension, closed for modification. Prefer composition, interfaces, and new focused components over unstable edits to mature code.
- **Liskov Substitution** - Derived types must be substitutable for their base types without breaking behavior.
- **Interface Segregation** - Interfaces must be small and focused. Never force a consumer to depend on methods it does not use.
- **Dependency Inversion** - Depend on abstractions rather than concrete implementations where that improves testability, clarity, or decoupling.

Apply these as guiding heuristics. If a domain or framework requires a pragmatic shortcut, document the reason and revisit it when the pressure passes.

---

## 4. Performance and Scale

Embed performance constraints directly into generated implementation when the project defines them.

- **Latency and Throughput:** Respect user experience budgets, frame-rate targets, service-level objectives, and batch-processing limits when they exist.
- **Cascading Complexity:** Minimize transitive dependencies between modules. Avoid deep call chains that make failure diagnosis and profiling difficult.
- **Resource Limits:** Avoid unnecessary memory, CPU, network, and storage costs, especially in hot paths or high-frequency loops.
- **Measurement First:** Do not optimize from vibes. Use profiling, instrumentation, benchmarks, or targeted tests when performance drives a change.

---

## 5. Quality and Stability

Code quality must not drift based on prompt phrasing. These are non-negotiable structural requirements.

- **Complexity:** Functions should be short enough to reason about as a single unit of work. Classes, modules, and services must adhere to single responsibility.
- **Testability:** New features and bug fixes require appropriate automated tests unless the repository has no test harness or the user explicitly accepts the gap.
- **Documentation:** Public APIs, durable workflows, and non-obvious algorithmic choices must be documented at the right level.
- **Maintainability:** Prefer boring, predictable code over clever code. A future agent or developer should be able to modify it without reconstructing hidden intent from chat history.

---

## 6. Mandatory Trade-offs

When faced with architectural ambiguity, resolve decisions using the following hierarchy:

- **Security vs. Convenience:** Choose security over developer or user convenience.
- **Correctness vs. Speed:** Choose correctness first. Speed matters after the behavior is right.
- **Performance vs. Readability:** Prefer readability by default. Optimize for performance only when an explicit budget is violated or a measured bottleneck exists.
- **Consistency vs. Novelty:** Follow existing codebase conventions over modern or novel approaches unless explicitly authorized to refactor.
- **Scope vs. Speculation:** Build what the task requires. Do not add speculative features, knobs, or abstractions.

When these rules do not cover the conflict, fall back to the Core Philosophy priority order in Section 1.

---

## 7. Definition of Done

A change is complete only when:

- [ ] The implementation satisfies the requested behavior.
- [ ] Security-sensitive boundaries are validated.
- [ ] Architectural ownership remains clear.
- [ ] Each changed component has a single understandable responsibility.
- [ ] Code is readable, maintainable, and refactor-ready.
- [ ] Tests exist for new features and bug fixes, or the test gap is explicitly documented.
- [ ] Relevant docs, goals, or implementation plans are updated when behavior or scope changes.
- [ ] The console, build, or test output is clean for the checks that were run.
- [ ] Every changed line traces directly to the user's request.
- [ ] No unnecessary abstractions, features, or improvements beyond scope were added.

---

## 8. Enforcement

When you cannot satisfy a law, you must:

1. Stop and identify which law is at risk.
2. Explain the conflict to the user before proceeding.
3. Never silently violate a law. An acknowledged exception is acceptable; a hidden one is not.
