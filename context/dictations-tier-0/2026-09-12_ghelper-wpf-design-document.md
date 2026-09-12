---
name: 2026-09-12-ghelper-wpf-design-document
description: Initial project seed dictation for G-Helper.WPF — purpose, WinForms-to-WPF split architecture, ROG Ally overlay shell, generalised input-binding system, upstream sync strategy, and Milestones 1-3 with V1 Definition of Done.
metadata:
  version: "1.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---
# 2026-09-12 - G-Helper.WPF Initial Design Document

## Source

- Captured from: AI-drafted design document supplied by the user as `G-Helper.WPF_Design_Document.md` (`C:\Users\Jarry\Downloads\G-Helper.WPF_Design_Document.md`), used as the initialization seed for this repository.
- Related project area: whole-project initialization — Design, Milestones 1-3, and the full V1 story set.

## Raw Notes

Preserved verbatim from the supplied source document.

```markdown
# G-Helper.WPF — Design and Implementation Plan

## 1. Purpose

This project is a public fork of G-Helper focused on rebuilding the application around a WPF front end and extending the existing ROG Ally input-remapping experience.

The immediate goal is not to redesign the application from scratch. The first goal is to preserve the existing G-Helper behaviour while separating UI from business logic, introducing a WPF executable head, and then incrementally changing the WPF version into the application we actually want to use.

The resulting application is called:

**G-Helper.WPF**

It remains recognisably G-Helper, continues to use the existing hardware-control logic, remains a tray application, and preserves upstream compatibility as far as practical.

---

## 2. Problems This Project Solves

### 2.1 WinForms is the wrong UI layer for this fork

The existing G-Helper front end is implemented in WinForms.

For this fork, the UI needs to move to WPF because:

- WPF is the preferred UI framework for this project.
- It provides better control over layout, styling, view composition and overlays.
- The application will eventually diverge substantially from the existing WinForms presentation.
- Keeping hardware/business logic mixed with the existing UI makes that work harder than necessary.

The first architectural task is therefore to separate the current application into:

- reusable non-UI logic;
- the existing WinForms executable;
- a new WPF executable.

---

### 2.2 The existing main window behaviour is not desirable on the ROG Ally

The current G-Helper application behaves like a conventional floating desktop application window.

On the ROG Ally, the desired interaction is closer to the original Armoury Crate SE experience:

- G-Helper.WPF remains resident in the system tray;
- activating it brings up a large console-style overlay;
- the overlay is anchored to the left-hand side of the display;
- it is intended to feel like a system/handheld control panel rather than a normal floating desktop window;
- configured hardware-button shortcuts should open and close this same overlay.

The first WPF UI should preserve the functionality and content of the existing G-Helper views, but host them in this left-side overlay shell.

---

### 2.3 Some useful Ally input actions are hard-coded instead of bindable

G-Helper already contains behaviour for showing the Windows touch keyboard.

On the Ally, the existing gesture using a rear paddle and D-pad direction can trigger the keyboard, but that action is not exposed to the normal configurable action list.

That makes the behaviour less flexible than it should be.

The WPF fork will expose the touch keyboard as a normal bindable action.

The existing hard-coded gesture does not need to be removed. The new feature simply makes the same action available to the input binding system.

A user should therefore be able to bind the Command Center button directly to:

**Open / close touch keyboard**

---

## 3. Target Architecture

The intended solution structure is conceptually:

```text
G-Helper
│
├── G-Helper.Shared
│   ├── hardware communication
│   ├── device services
│   ├── power management
│   ├── fan control
│   ├── controller/input handling
│   ├── RGB/Aura logic
│   ├── battery logic
│   ├── configuration
│   └── other reusable business logic
│
├── G-Helper
│   └── existing WinForms executable head
│
└── G-Helper.WPF
    └── new WPF executable head
```

The exact project names may change while implementing the split, but the architectural requirement is fixed:

**UI-specific code must be separated from reusable G-Helper logic.**

The WinForms application should continue to be able to reference the shared library.

The WPF application should reference that same shared library.

---

## 4. WPF Conversion Strategy

### Phase 1 — Functional shell

Create a new WPF executable project.

Initially it may contain almost no UI.

Its purpose is simply to prove that:

- the project builds;
- it references the shared G-Helper logic;
- the process starts;
- required services can initialise;
- the application can behave as a tray application.

---

### Phase 2 — Functional equivalence

Rebuild the existing G-Helper WinForms UI as WPF views.

The initial rule is:

> Do not redesign behaviour during translation.

The WPF version should first reproduce what the existing WinForms version already does.

The goal is feature parity before feature expansion.

At the end of this phase, G-Helper.WPF should be capable of replacing the WinForms application for normal use.

---

### Phase 3 — Desired WPF experience

Once parity exists, the WPF application begins intentionally diverging.

The main shell becomes the left-side ROG Ally overlay.

The existing settings and controls remain available, but are hosted inside that shell.

From this point onward, new input-remapping functionality and other WPF-specific improvements are added.

---

## 5. Application Shell

G-Helper.WPF remains a tray application.

### Launch behaviour

On process launch:

- the application initialises normally;
- a G-Helper.WPF tray icon is created;
- the main overlay does not need to behave like a conventional desktop window.

### Opening the application

Opening the application from either:

- the tray icon; or
- a configured hardware/input binding

should show the same WPF overlay.

### Overlay behaviour

The overlay should:

- appear against the left edge of the primary/internal display;
- use a large handheld-friendly layout;
- provide the same controls exposed by the WPF views;
- be suitable for navigation on the ROG Ally;
- be easy to open and close repeatedly;
- replace the arbitrary floating-window behaviour of the existing application.

The intended visual/interaction reference is the **older Armoury Crate SE left-side overlay**, before the Game Bar-oriented redesign.

---

## 6. Generalised Input Binding System

The existing configurable button support is too narrow.

The new system should treat bindings as user-created mappings rather than a small fixed list of hard-coded configurable buttons.

### 6.1 Binding collection

The UI should present bindings as a list or grid.

A user can:

- inspect existing bindings;
- edit an existing binding;
- add another binding using a `+` control;
- remove a custom binding;
- configure the input source;
- configure the action.

The application may ship with sensible default bindings, but the binding collection itself should be extensible by the user.

---

### 6.2 Input sources

The system should support normal controller inputs as configurable sources.

Examples include:

- A
- B
- X
- Y
- D-pad Up
- D-pad Down
- D-pad Left
- D-pad Right
- Left bumper
- Right bumper
- Left trigger
- Right trigger
- View / Select
- Menu / Start
- Left stick click
- Right stick click
- Command Center button
- ROG button
- rear paddle buttons where supported

The exact set should be driven by the inputs that the device/input layer can reliably expose.

### V1 limitation

Complex paddle combinations are explicitly out of scope for the first version of the generalised binding system.

The aim is direct, understandable mappings.

---

## 7. Binding Resolution

Standard controller behaviour remains the default.

For example:

```text
X input -> X controller action
Y input -> Y controller action
Right Trigger -> Right Trigger controller action
```

A user-created binding acts as an override.

For example:

```text
X input -> Y controller action
```

Now pressing physical X produces logical Y.

This allows arbitrary gamepad remapping without requiring every physical control to have a permanently visible hard-coded configuration slot.

---

## 8. Action Types

The action selector should remain conceptually similar to the existing G-Helper action selector, but become context-sensitive.

Only configuration controls relevant to the selected action should be visible.

Unused fields should be collapsed.

---

### 8.1 Existing G-Helper actions

Existing supported actions should remain available.

Examples include the actions G-Helper already exposes for configurable buttons and shortcuts.

No existing useful action should be lost simply because the UI moves to WPF.

---

### 8.2 Custom executable / command action

G-Helper already supports a custom action that accepts a string such as an executable path.

For this action:

- selecting the custom/launch action reveals a text field;
- the user enters the executable or command information there.

For action types that do not consume a string value, this field should be collapsed.

This removes the permanently visible but irrelevant string box from most configurations.

---

### 8.3 Touch keyboard action

Add a new normal action representing the existing touch-keyboard behaviour.

Conceptually:

**Touch Keyboard**

or:

**Open / close touch keyboard**

This action must be available anywhere a normal binding action can be selected.

The existing paddle-based keyboard gesture may remain intact.

This new action simply exposes the existing capability through the configurable binding system.

Example:

```text
Command Center button -> Touch Keyboard
```

---

### 8.4 Gamepad action

Add a new action type:

**Gamepad Action**

When selected, a secondary combo box becomes visible.

That combo box contains logical gamepad outputs such as:

- A
- B
- X
- Y
- D-pad directions
- bumpers
- triggers
- Start/Menu
- Select/View
- left stick click
- right stick click
- other supported standard gamepad outputs

Example:

```text
Input:
    X

Action:
    Gamepad Action

Gamepad output:
    Y
```

Result:

**Pressing physical X produces logical Y.**

This forms the core of arbitrary controller-button remapping.

---

### 8.5 Keyboard action

Add a new action type:

**Keyboard Action**

Selecting it reveals a key-capture control.

The control is not a normal text box.

The interaction should be:

1. user activates the capture field;
2. the application waits for a keyboard key;
3. the next key press is captured;
4. that key becomes the configured action.

Examples:

```text
X -> Backspace
A -> Space
D-pad Up -> W
```

The user should not need to know or manually type virtual-key codes or textual key names.

The UI should display the captured key in readable form.

---

### 8.6 Mouse action

Add a new action type:

**Mouse Action**

Selecting it reveals a secondary combo box containing supported mouse inputs.

Initial useful actions include at least:

- Left Click
- Right Click

The design should allow additional mouse actions to be added later.

This makes existing mouse-emulation choices overrideable.

For example, where the existing behaviour may map:

```text
Right Bumper -> Left Click
Right Trigger -> Right Click
```

the user could configure:

```text
Right Bumper -> Right Click
Right Trigger -> Left Click
```

The user should be able to decide which controller input produces which mouse action.

---

## 9. Context-Sensitive Binding Editor

The binding editor should avoid showing irrelevant controls.

Conceptually:

```text
Input:  [ X                 v ]

Action: [ Gamepad Action    v ]

         [ Y                 v ]
```

For a keyboard action:

```text
Input:  [ X                 v ]

Action: [ Keyboard Action   v ]

Key:    [ Backspace           ]
```

For a custom executable:

```text
Input:  [ Command Center    v ]

Action: [ Launch / Custom   v ]

Target: [ C:\...\tool.exe     ]
```

For an action that requires no parameter:

```text
Input:  [ Command Center    v ]

Action: [ Touch Keyboard    v ]
```

No irrelevant parameter box should remain visible.

---

## 10. Upstream Synchronisation Strategy

The fork will significantly rearrange source files.

That makes future upstream merges harder than a conventional fork.

This problem must be designed for from the beginning rather than dealt with after the source tree has diverged.

### Branch strategy

`main` is treated as the branch used to synchronise with upstream G-Helper.

Project development occurs on:

```text
G-Helper.WPF
```

Conceptually:

```text
upstream G-Helper
      |
      v
    main
      |
      | adapt / merge
      v
G-Helper.WPF
```

Normal feature development should not occur directly on `main`.

---

## 11. Migration / Upstream Adaptation Document

The repository must maintain a living document describing the architectural movement of upstream code.

Its purpose is to help both humans and coding agents answer questions such as:

- Where did this upstream source file move?
- Was this class split?
- Which project now owns this responsibility?
- Was this code made UI-independent?
- Does an upstream WinForms change represent:
  - business logic that belongs in Shared;
  - UI code that needs a WPF equivalent;
  - obsolete WinForms-only behaviour;
  - a hardware fix that must be ported immediately?
- What custom modifications exist around this subsystem?

A useful format is a table such as:

| Upstream location | Fork location | Transformation | Notes |
|---|---|---|---|
| `Foo.cs` | `G-Helper.Shared/Foo.cs` | moved | hardware logic |
| `Form1.cs` logic | `Shared/Services/...` | extracted | UI removed |
| `Form1.cs` UI | `G-Helper.WPF/Views/...` | reimplemented | WPF |
| ... | ... | ... | ... |

This mapping must be updated whenever code is moved, split, replaced or substantially restructured.

---

## 12. Upstream Merge Rules

When synchronising a newer upstream G-Helper version:

1. update `main` from upstream;
2. inspect upstream changes;
3. use the migration map to identify the corresponding fork location;
4. separate UI changes from reusable hardware/business logic changes;
5. port relevant business/hardware fixes into the shared layer;
6. port relevant UI behaviour into WPF rather than blindly copying WinForms changes;
7. update the migration document when the source relationship changes;
8. validate both build and runtime behaviour.

This process should be written into repository context so an agent working months later does not need to rediscover the architecture.

---

# 13. Milestones

## Milestone 1 — Repository and Context Setup

### Goal

Establish a clean, reproducible fork with the documentation and rules required for later architectural work.

No major product behaviour is changed in this milestone.

### Story 1 — Fork the original project

Create the public project fork from the current G-Helper repository.

#### Acceptance criteria

- fork exists;
- upstream remote is identifiable;
- repository can receive future upstream updates;
- local development environment can clone the fork.

---

### Story 2 — Initialise project context

Create the initial project context required by the agentic workflow.

This includes the high-level documentation describing:

- project goals;
- architecture direction;
- WPF migration;
- input remapping goals;
- milestone plan;
- implementation constraints;
- user stories.

This design document forms part of that initial context.

#### Acceptance criteria

- project context directory/structure exists;
- design document is committed;
- stories are represented in the project context;
- later agents can understand the intended end state without requiring the original conversation.

---

### Story 3 — Validate the clean baseline

Before structural changes begin, confirm that the forked upstream source:

- restores dependencies;
- compiles;
- launches;
- performs normally on the target ROG Ally;
- can be used as the behavioural baseline for later comparisons.

#### Acceptance criteria

- clean checkout builds successfully;
- executable launches successfully;
- baseline behaviour is documented;
- any environment-specific setup required for building/running is recorded.

---

### Story 4 — Establish upstream synchronisation rules

Set up the repository so future upstream changes can continue to be consumed after the WPF fork substantially rearranges the codebase.

#### Required work

- preserve `main` for upstream synchronisation;
- create/use `G-Helper.WPF` as the development branch;
- document the upstream merge process;
- create the migration/source-mapping document;
- define the rule that the migration document is updated whenever source ownership/location changes;
- include these rules in the agent context so they remain visible during future work.

#### Acceptance criteria

- branch strategy is documented;
- `G-Helper.WPF` development branch exists;
- migration mapping document exists;
- repository context explicitly requires maintenance of that document;
- future upstream adaptation procedure is documented.

---

## Milestone 2 — WPF Conversion and Architectural Split

### Goal

Separate reusable G-Helper logic from the WinForms front end and establish a functional WPF executable head.

### Story 5 — Extract reusable business logic

Move non-UI behaviour out of the WinForms executable project into a reusable C# class-library project.

The shared library should contain hardware and application logic that does not inherently belong to WinForms.

#### Acceptance criteria

- shared C# project exists;
- reusable hardware/business logic is progressively moved into it;
- WinForms project references the shared project;
- WinForms behaviour remains functional;
- UI framework dependencies do not leak into shared logic unless genuinely unavoidable;
- migration map is updated for every significant source move.

---

### Story 6 — Create `G-Helper.WPF`

Create a new WPF executable project.

#### Acceptance criteria

- project builds;
- project references the shared G-Helper library;
- application launches;
- tray application infrastructure works;
- basic shared services can initialise from the WPF executable.

---

### Story 7 — Preserve WinForms baseline during the split

The architectural split must not silently destroy the existing application while source files are being reorganised.

#### Acceptance criteria

- WinForms head continues compiling during the conversion unless an explicit later decision retires it;
- existing behaviour remains available for comparison;
- migration does not intentionally change hardware behaviour;
- regressions caused by extraction are fixed before proceeding.

---

## Milestone 3 — WPF Equivalence and Product Changes

### Goal

Turn `G-Helper.WPF` from an empty executable head into the desired daily-use ROG Ally control application.

---

### Story 8 — Rebuild existing G-Helper views in WPF

Translate the existing WinForms UI into WPF.

The first version should seek functional equivalence rather than redesign.

#### Acceptance criteria

- existing user-facing G-Helper functionality is accessible from WPF;
- settings represented by the WinForms app have WPF equivalents;
- normal hardware controls continue to work;
- the WPF application is usable as a replacement front end.

---

### Story 9 — Replace the floating window with a left-side overlay

Change the WPF shell so the application opens as a left-anchored handheld control overlay inspired by the original Armoury Crate SE interface.

#### Acceptance criteria

- opening G-Helper.WPF presents the overlay;
- overlay is anchored to the left side of the screen;
- tray behaviour remains intact;
- hardware/input shortcuts can open the same overlay;
- normal desktop floating-window placement is no longer the primary UX.

---

### Story 10 — Expose touch keyboard as a bindable action

Take the existing touch-keyboard behaviour and expose it through the normal action-selection system.

#### Acceptance criteria

- Touch Keyboard appears in the bindable action list;
- it can be assigned to Command Center;
- it can also be assigned to other supported inputs;
- existing hard-coded paddle gesture may continue to work;
- selecting the action requires no irrelevant configuration field.

---

### Story 11 — Generalise configurable input bindings

Replace the small fixed list of configurable buttons with an extensible collection of bindings.

#### Acceptance criteria

- binding UI uses a list/grid;
- user can add a binding using `+`;
- user can remove custom bindings;
- user can choose the physical input source;
- supported normal gamepad buttons can be used as sources;
- existing special ASUS buttons remain available where supported;
- paddle combinations are not required for V1.

---

### Story 12 — Add gamepad-output actions

Allow an input binding to emit another logical gamepad input.

#### Acceptance criteria

- `Gamepad Action` exists;
- selecting it reveals a gamepad-output combo box;
- standard logical buttons are selectable;
- a physical X button can be configured to emit Y;
- equivalent remaps work for other supported controller inputs.

---

### Story 13 — Add keyboard actions

Allow controller inputs to emit keyboard keys.

#### Acceptance criteria

- `Keyboard Action` exists;
- selecting it reveals a keyboard capture field;
- user activates the field and presses a key;
- first captured keystroke becomes the configured key;
- readable key name is shown;
- normal typing into the capture field is not required;
- bindings such as `X -> Backspace` work.

---

### Story 14 — Add mouse actions

Allow controller inputs to emit mouse actions.

#### Acceptance criteria

- `Mouse Action` exists;
- selecting it reveals a mouse-action combo box;
- Left Click is supported;
- Right Click is supported;
- existing mouse-emulation defaults can be overridden with explicit bindings;
- bindings such as `Right Trigger -> Left Click` work.

---

### Story 15 — Make action configuration context-sensitive

Clean up the binding editor so only controls relevant to the selected action are displayed.

#### Acceptance criteria

- custom executable action shows its text/path field;
- actions with no extra data collapse that field;
- Gamepad Action shows the gamepad-output selector;
- Keyboard Action shows the key-capture field;
- Mouse Action shows the mouse-action selector;
- unrelated controls remain collapsed.

---

# 14. V1 Definition of Done

The first useful G-Helper.WPF release is complete when:

- the repository is a maintainable public fork;
- upstream synchronisation rules are established;
- source migration mapping exists and is maintained;
- reusable application logic has been separated from the WinForms UI;
- `G-Helper.WPF` exists and runs;
- the existing G-Helper functionality has been reproduced in WPF;
- the application opens as a left-side ROG Ally overlay;
- touch keyboard is bindable;
- users can add arbitrary direct gamepad bindings;
- gamepad inputs can emit alternate gamepad actions;
- gamepad inputs can emit keyboard keys;
- gamepad inputs can emit mouse clicks;
- the binding editor only shows configuration relevant to the selected action.

Complex paddle combinations are not part of the V1 requirement.
```

## Important Signals

- New decision: project is a WinForms→WPF fork of upstream `seerge/g-helper`, split into `G-Helper.Shared` / `G-Helper` (WinForms) / `G-Helper.WPF`, with `main` reserved for upstream sync and feature work happening on a separate development branch (doc names it `G-Helper.WPF`; the repository's actual current branch performing this role is `wpf`).
- New decision: V1 scope is bounded — three milestones, fifteen stories, explicit V1 Definition of Done, complex paddle combinations explicitly deferred past V1.
- New decision: translation discipline — "do not redesign behaviour during translation" governs Milestone 3 Story 8; feature parity must precede feature expansion.
- New risk: the fork intentionally rearranges source files, which the doc identifies as a threat to future upstream mergeability; it prescribes a living migration/source-mapping document (Section 11) as the mitigation, updated on every significant source move.
- Open question: the doc's Section 10 branch diagram names the development branch `G-Helper.WPF`, but the repository's actual current branch is named `wpf`. Not resolved here — recorded as an open question in Design.
- Open question: the doc does not specify a concrete "upstream" git remote; the repository as inspected during initialization has only an `origin` remote (`JarrydAdaens/g-helper-wpf`). Whether this repo is a registered GitHub fork of `seerge/g-helper`, and whether a dedicated `upstream` remote should be added, is unresolved and needs the user or a GitHub API check to confirm.
- Open question: Section 3's `G-Helper.Shared` component list (hardware communication, device services, power management, fan control, controller/input handling, RGB/Aura logic, battery logic, configuration) is a target shape, not a verified inventory of what exists today in `app/`. The actual extraction boundary needs to be derived from the current WinForms source during Milestone 2 Story 5 planning.
- Possible milestone or story impact: none yet beyond the doc's own three milestones — this document is the whole initial roadmap.

## Integration Notes

- Updated `../design.md` (and its Milestones Index): populated Executive Summary, Core Principles, System Architecture (Shared/WinForms/WPF split), Repository Structure, two Processing Pipelines (binding resolution, upstream merge), Configuration/Security/Observability/Testing/Performance sections, and a three-row Milestones Index.
- Updated `../milestones/`: replaced the `milestone-1.md` placeholder and created `milestone-2.md` and `milestone-3.md`, each with its Story Index and full story detail drawn directly from this document's Section 13.
- Updated `../backlog/`: `backlog-1.md` — all fifteen known stories map directly to Milestones 1-3 with high confidence, so no unscheduled stories were added; the file now records that state instead of carrying a placeholder story.
- Updated `../implementation-plans/`: none created. No story in this seed has enough execution-level detail yet to justify an Implementation Plan; plans are deferred to when a specific story is picked up for delivery, per Milestone 1 Story 2/3 sequencing.
