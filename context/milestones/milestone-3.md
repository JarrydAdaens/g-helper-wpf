---
name: milestone-3
description: Milestone 3 - WPF Equivalence and Product Changes. Rebuilds the existing G-Helper UI in WPF, replaces the floating window with a left-side ROG Ally overlay, and delivers the generalised gamepad/keyboard/mouse input-binding system that forms the V1 release.
metadata:
  version: "3.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---
# Milestone 3: WPF Equivalence and Product Changes

> Milestone. A coherent macro-feature or delivery outcome. (Tier numbers live only in `design.md` / `agenticworkflow.md`.)
>
> Related: [design.md (Milestones Index)](../design.md#milestones-index), [../backlog/](../backlog/)

---

## Intent

Turn `G-Helper.WPF` from an empty executable head (Milestone 2 output) into the desired daily-use ROG Ally control application: WPF feature parity, the left-side overlay shell, and the generalised gamepad/keyboard/mouse input-binding system. Completing this milestone's stories satisfies the project's V1 Definition of Done.

## Why it matters

- This is where the product value the whole fork exists for is actually delivered — everyone using this app instead of upstream WinForms G-Helper needs what this milestone builds.
- The stories here are intentionally sequenced from "don't lose anything" (parity) through "change the shell" (overlay) to "add new capability" (binding system), matching the design document's own phase discipline: do not redesign behaviour during translation, reach parity, then diverge.

## Outcome / Definition of Done

Matches the design document's V1 Definition of Done in full:

- Existing G-Helper functionality is reproduced in WPF.
- The application opens as a left-side ROG Ally overlay.
- Touch keyboard is bindable through the normal action system.
- Users can add arbitrary direct gamepad bindings via an extensible list/grid.
- Gamepad inputs can emit alternate gamepad actions, keyboard keys, or mouse clicks.
- The binding editor only shows configuration relevant to the selected action.
- Complex paddle combinations are explicitly out of scope for this milestone/V1.

## Status

Not Started — 0/8 stories complete. Depends on Milestone 2 (Shared/WinForms/WPF split) being complete first.

---

## Story Index

| # | Story | Type | Complexity | Effort | Risk | Plan | Status |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 8 | [Rebuild existing G-Helper views in WPF](#story-8) | Feature | — | — | — | *not yet generated* | Not Started |
| 9 | [Replace the floating window with a left-side overlay](#story-9) | Feature | — | — | — | *not yet generated* | Not Started |
| 10 | [Expose touch keyboard as a bindable action](#story-10) | Feature | — | — | — | *not yet generated* | Not Started |
| 11 | [Generalise configurable input bindings](#story-11) | Feature | — | — | — | *not yet generated* | Not Started |
| 12 | [Add gamepad-output actions](#story-12) | Feature | — | — | — | *not yet generated* | Not Started |
| 13 | [Add keyboard actions](#story-13) | Feature | — | — | — | *not yet generated* | Not Started |
| 14 | [Add mouse actions](#story-14) | Feature | — | — | — | *not yet generated* | Not Started |
| 15 | [Make action configuration context-sensitive](#story-15) | Feature | — | — | — | *not yet generated* | Not Started |

---

## Stories

<a id="story-8"></a>

### Story 8: Rebuild existing G-Helper views in WPF

**Type:** Feature

**Summary:**
Translate the existing WinForms UI into WPF views, seeking functional equivalence rather than redesign. All settings and controls the WinForms app exposes should have WPF equivalents.

**Why / value:**
Feature parity before feature expansion is the design document's explicit ordering rule; nothing below in this milestone should ship ahead of this.

**Rough scope:**
New WPF views under `G-Helper.WPF`, one per existing WinForms form (`app/UI/`, `app/*.cs` + `.Designer.cs` pairs such as `Fans`, `Matrix`, `Handheld`, `Extra`, `Slash`, `Updates`), each bound to the shared logic from Milestone 2.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-3/rebuild-existing-ghelper-views-in-wpf/plan.md`

**Status:** Not Started

---

<a id="story-9"></a>

### Story 9: Replace the floating window with a left-side overlay

**Type:** Feature

**Summary:**
Change the WPF shell so the application opens as a left-anchored, handheld-friendly control overlay inspired by the older (pre-Game-Bar) Armoury Crate SE interface, instead of a conventional floating desktop window. Tray behaviour and hardware/input shortcuts to open/close it must remain intact.

**Why / value:**
This is the core UX problem statement of the whole project (design document Section 2.2) — the floating-window model is a poor fit for the ROG Ally.

**Rough scope:**
New overlay shell/window hosting the Story 8 views; anchor/sizing logic against the primary/internal display; wiring the existing tray icon and configured hardware-button shortcuts to open/close it.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-3/replace-the-floating-window-with-a-left-side-overlay/plan.md`

**Status:** Not Started

---

<a id="story-10"></a>

### Story 10: Expose touch keyboard as a bindable action

**Type:** Feature

**Summary:**
Add the existing touch-keyboard behaviour to the normal bindable action list (a `Touch Keyboard` / "Open / close touch keyboard" action), so it can be assigned to Command Center or any other supported input, without removing the existing hard-coded paddle gesture.

**Why / value:**
This is the second concrete problem statement in the design document (Section 2.3) and does not depend on the full generalised binding system (Stories 11-15) to be useful on its own.

**Rough scope:**
`source/app/Helpers/OnScreenKeyboard.cs` / `source/app/Helpers/TouchscreenHelper.cs` behaviour exposed through whatever action-selection mechanism exists after Story 8/11 land; requires no parameter field.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-3/expose-touch-keyboard-as-a-bindable-action/plan.md`

**Status:** Not Started

---

<a id="story-11"></a>

### Story 11: Generalise configurable input bindings

**Type:** Feature

**Summary:**
Replace the small fixed list of configurable buttons with an extensible list/grid of user-created bindings: add via `+`, remove custom bindings, choose the physical input source (standard gamepad buttons plus supported special ASUS buttons), configure the action. Complex paddle combinations are explicitly out of scope for V1.

**Why / value:**
This is the foundation Stories 12-15 build on — without a generic binding collection, gamepad/keyboard/mouse output actions have nowhere to attach.

**Rough scope:**
New binding-collection UI and underlying binding-storage model in `G-Helper.WPF` (and/or `G-Helper.Shared` for the resolver), building on `app/Input/`.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-3/generalise-configurable-input-bindings/plan.md`

**Status:** Not Started

---

<a id="story-12"></a>

### Story 12: Add gamepad-output actions

**Type:** Feature

**Summary:**
Allow a binding to emit a different logical gamepad output (`Gamepad Action`), selectable via a secondary combo box of standard logical buttons, so a physical input like X can be configured to emit Y.

**Why / value:**
This is the core of arbitrary controller-button remapping described in the design document (Section 8.4) and the binding-resolution example (Section 7).

**Rough scope:**
New action type in the Story 11 binding model plus its resolver behaviour (Pipeline 1 in [design.md](../design.md#pipeline-1-input-binding-resolution)).

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-3/add-gamepad-output-actions/plan.md`

**Status:** Not Started

---

<a id="story-13"></a>

### Story 13: Add keyboard actions

**Type:** Feature

**Summary:**
Allow controller inputs to emit keyboard keys (`Keyboard Action`) via a key-capture control: the user activates the field, the next keypress is captured and becomes the configured key, shown in readable form — no manual virtual-key-code entry.

**Why / value:**
Enables bindings such as `X -> Backspace`, a capability the fixed WinForms button list never had.

**Rough scope:**
New action type in the Story 11 binding model; a WPF key-capture control; readable key-name display.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-3/add-keyboard-actions/plan.md`

**Status:** Not Started

---

<a id="story-14"></a>

### Story 14: Add mouse actions

**Type:** Feature

**Summary:**
Allow controller inputs to emit mouse actions (`Mouse Action`) via a secondary combo box, initially supporting at least Left Click and Right Click, letting the user override existing mouse-emulation defaults (for example swapping which of Right Bumper/Right Trigger produces which click).

**Why / value:**
Makes today's implicit mouse-emulation mapping explicit and user-configurable.

**Rough scope:**
New action type in the Story 11 binding model; design allows future mouse actions beyond Left/Right Click to be added later.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-3/add-mouse-actions/plan.md`

**Status:** Not Started

---

<a id="story-15"></a>

### Story 15: Make action configuration context-sensitive

**Type:** Feature

**Summary:**
Clean up the binding editor so only controls relevant to the selected action are shown: custom-executable actions show their text/path field, Gamepad Action shows the gamepad-output selector, Keyboard Action shows the key-capture field, Mouse Action shows the mouse-action selector, and actions needing no parameter collapse all of them.

**Why / value:**
Closes out the binding-editor UX described in the design document (Sections 8 and 9) — without this, every action type's controls would be visible at once regardless of relevance.

**Rough scope:**
UI-only story layered on top of Stories 11-14's action types; no new action types introduced here.

**CER:**

- Complexity: —
- Effort: —
- Risk: —

**Plan:** `../implementation-plans/milestone-3/make-action-configuration-context-sensitive/plan.md`

**Status:** Not Started

---

## Interdependency Order

1. Story 8 (WPF view parity) before Story 9 (overlay shell), since the overlay hosts the parity views.
2. Story 9 (overlay shell) should land before or alongside Story 11 (generalised bindings), since the overlay is the natural home for the binding-editor UI.
3. Story 11 (generalised bindings) before Stories 12, 13, and 14 (gamepad/keyboard/mouse output actions), since each needs the binding collection to attach to.
4. Story 15 (context-sensitive editor) last, since it depends on all action types from Stories 11-14 (plus the existing custom/launch action and Story 10's Touch Keyboard) already existing.
5. Story 10 (touch keyboard action) can proceed in parallel with Stories 11-14 once Story 8 exists, since it does not depend on the generalised binding system.

---

## Backlog Sources

- No backlog themes were pulled into this milestone. All eight stories were mapped directly from the initial dictation (design document Section 13, Milestone 3) during project initialization, with high confidence.

---

## Deferred / Follow-up Work

- Complex paddle-button combinations are explicitly deferred past V1 (design document Section 6.2 and Section 14) — do not pull this into Milestone 3 without an explicit new decision.
- Additional mouse actions beyond Left/Right Click are anticipated but not required for Story 14's V1 scope.

---

## Notes

- Keep this Story Index in sync with the [Milestones Index](../design.md#milestones-index) in Design.
- This milestone's completion is equivalent to the design document's V1 Definition of Done; do not mark it Complete until every acceptance criterion listed there is verifiably met.
