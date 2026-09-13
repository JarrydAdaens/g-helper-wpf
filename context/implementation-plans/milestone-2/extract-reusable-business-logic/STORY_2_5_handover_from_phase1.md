---
name: story-2-5-handover-phase1
description: Handover for Milestone 2 Story 5 (extract reusable business logic) after slices 1-5 landed — what's done, what's next, and the traps found along the way.
metadata:
  version: "1.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---
# Story 5 — Handover from Phase 1 (slices 1-5)

[Milestone 2](../../../milestones/milestone-2.md#story-5) · [plan.md](plan.md) · [design.md](../../../design.md) · [upstream-source-mapping.md](../../../wiki/upstream-source-mapping.md)

## 1. What was completed in this phase

Slices 1-5 of Story 5's incremental extraction, all committed on branch `wpf`, all independently build-verified and launch-checked before committing (never trust a sub-agent's self-report — re-diff, re-build, and where practical re-launch yourself):

| Commit | Slice | Summary |
| --- | --- | --- |
| `3dd72449` (pre-existing) | 1 | Created `GHelper.Shared`; moved `Logger`, `DeviceHelper`, `Keystone`; extracted `UserIdentity` out of `ProcessHelper`. |
| `64f249c9` | 2 | Split `ProcessHelper`: process/service utilities moved to new `ProcessUtility` (renamed to dodge a same-namespace collision); `CheckAlreadyRunning`/`RunAsAdmin` stayed (WinForms). |
| `fc5a391b` | 3 | Moved `AppConfig` (830 lines, the file gating most of the rest) whole, keeping its global namespace. Moved `AsusFan` enum and the five `Performance*` constants; split three pure config-reads out of `Modes` into new `ModeConfig`. |
| `7ef26002` | 4 | Moved `Pawn/` (CPU/MSR/SMU access, 4 files) unchanged. Left `IntelMSR.bin`/`RyzenSMU.bin` embedded in `GHelper.csproj` (resource name is derived from the calling assembly at runtime). |
| `38bc9ce4` | 5 | Moved `Peripherals/`'s 40 device files (9,904 lines) plus `AnimeMatrix/Communication/` (forced by a base-class edge) plus `AuraMode`/`AuraSpeed`. Left `PeripheralsProvider.cs` behind — see §5. |

(Repo-wide tidy pass `2524f2ae` moved everything under `source/` between slices 1 and 2; all paths below already assume that.)

Between-slice orchestration note for whoever picks this up: a scheduled wake-up mid-session and a live "keep doing stories" instruction converged on the same in-flight work without conflict — worth knowing this can happen, not a problem to solve.

## 2. What exists in the repository now

- `source/GHelper.Shared/` (net10.0-windows, x64, **no** `UseWindowsForms`/`UseWPF` — hard constraint, a UI leak fails the build) now holds: `Helpers/{Logger,DeviceHelper,Keystone,UserIdentity,ProcessUtility}.cs`, `AppConfig.cs`, `AsusFan.cs`, `Mode/ModeConfig.cs`, `Pawn/{CpuInfo,IntelMsr,PawnIOWrapper,RyzenSmu}.cs`, `Peripherals/{IPeripheral.cs, Keyboard/**, Mouse/**}`, `AnimeMatrix/Communication/{Device,Packet,Platform/UsbProvider,Platform/WindowsUsbProvider}.cs`, `USB/{AuraMode,AuraSpeed}.cs`. csproj references: `System.Management`, `HidSharpCore` 1.3.0, explicit `<Using Include="System.Drawing" />`.
- `source/app/` (WinForms head) keeps: `AsusACPI.cs` (still whole — see below), `Mode/Modes.cs` (delegates 3 members to `ModeConfig`, keeps the rest), `Peripherals/PeripheralsProvider.cs`, `USB/Aura.cs` (minus the two moved enums), `Pawn/{IntelMSR.bin,RyzenSMU.bin}` (embedded resources, deliberately not moved), `Helpers/ProcessHelper.cs` (delegates 7 members, keeps `CheckAlreadyRunning`/`RunAsAdmin`), plus everything else untouched.
- Both solutions build 0 warnings / 0 errors: `dotnet build source\G-Helper.WPF.sln -c Debug` and `dotnet build source\app\GHelper.sln -c Debug`.
- `context/wiki/upstream-source-mapping.md`, `context/milestones/milestone-2.md` (Story 5 section), and `plan.md`'s slice table + Execution Log are all current as of commit `38bc9ce4` — read the Execution Log entries for slices 3-5 before starting new work, they contain the exact reasoning for every edge case below.

## 3. Explicit next steps

Per `plan.md`'s slice table, row 6+, cheapest first:

1. **`PeripheralsProvider.cs`** — cannot be shimmed like `ProcessHelper` was. Its `Program.settingsForm`/`Program.inputDispatcher` callbacks fire from *inside* its connect/disconnect/battery-event flows, not at clean edges. This needs a real design decision (an interface or event the head implements and the shared type calls back into) before it can move — treat it as its own slice, possibly its own sub-agent with room to think about the seam rather than just mechanically moving files.
2. **`AsusACPI.cs`** (980 lines) — no UI dependency, but five of its own instance methods call back into the head's `Program.acpi` static. Rewriting those to `this` is probably correct but needs to be verified behaviour-preserving (previous phase deliberately declined to do this without more certainty — don't skip that verification just because it looks obvious).
3. Rest of `USB/` and `AnimeMatrix/` (the four files not moved in slice 5).
4. `Battery/`, `Fan/`, `Display/`, `Gpu/`, rest of `Mode/`, `Ally/`, `Input/`, `HardwareControl.cs`, `NativeMethods.cs`.

Story 5 stays open-ended by design (see `plan.md` Estimates section) — do not try to force a single final slice; keep following dependency direction, not folder tidiness.

## 4. Key decisions and rationale (read before repeating the analysis)

- **Shim vs. whole-move vs. rename**, decided per-file by call-site count and namespace collision risk:
  - Few call sites, WinForms-entangled remainder → **shim** (`ProcessHelper`→`ProcessUtility`, `Modes`→`ModeConfig`, `AsusACPI`'s `Performance*` consts).
  - Same-namespace collision between the head assembly and `GHelper.Shared` → **rename the shared-side type** (`ProcessUtility`, not `ProcessHelper`; `ModeConfig`, not `Modes`), because two identically-named types across two referenced assemblies make every unqualified call site ambiguous (CS0104).
  - Huge call-site count, no reason to rename → **move whole, keep the same (often global) namespace** (`AppConfig`, `AsusFan`) — shimming would have forced a rename at every call site as later slices landed.
- **`Application.StartupPath` → `AppContext.BaseDirectory`**: verified equivalent on modern .NET (both include the trailing separator), and the only one of the two realistic candidates that survives a single-file publish (`Assembly.GetEntryAssembly().Location` returns `""` there). Don't reach for `Assembly.Location` as an alternative without re-deriving this.
- **Base-class/interface edges force the whole hierarchy to move together** — `AsusKeyboard`/`AsusMouse` deriving from `AnimeMatrix.Communication.Device` is why that transport moved in slice 5 even though it wasn't in `Peripherals/`. Check inheritance, not just `using` directives, before scoping a slice.
- **`internal` → `protected`**: when a base type moves to `GHelper.Shared` but some subclasses stay in `source/app/`, `internal` members become invisible across the assembly boundary. `Packet`'s constructor needed `protected` for exactly this reason — check for this pattern on any future base-class move.
- **WinForms enum/type leaks get replaced with local equivalents, not architected around**: `AsusMouse.cs` used `System.Windows.Forms.Keys` to spell 12 combo commands. Replaced with a private `byte`-constant holder (`Vk`) using the same underlying VK codes — verified byte-identical output before committing, not just assumed. Look for the same pattern (an innocuous WinForms enum used only for its numeric values) elsewhere before assuming a file is "no UI dependency."
- **Package/csproj additions get documented inline**: `GHelper.Shared.csproj` needed `HidSharpCore` and an explicit `System.Drawing` `<Using>` (the type itself is base-framework; only the *implicit using* came from `UseWindowsForms`). Both are commented in the csproj — keep that practice for future additions so a reviewer doesn't have to guess why a dependency showed up.
- **Model selection used for delegation** (relevant if you're orchestrating rather than doing the work by hand): Sonnet for contained/mechanical slices (`ProcessHelper` split, `Pawn/` move), Opus for slices with real architectural judgment or large blast radius (`AppConfig`, `Peripherals/`). `PeripheralsProvider` and `AsusACPI` both look Opus-tier — they require a design decision, not just a dependency-following move.

## 5. Known issues, risks, or open questions

- **`AsusACPI`'s `Program.acpi` coupling is unresolved** — this is the real remaining blocker for that file, flagged twice now (slice 3, slice 5) rather than force-fixed. Needs its own careful pass.
- **`PeripheralsProvider` needs a callback seam design** — worth thinking about whether the seam should anticipate the WPF head's eventual needs (Milestone 3) or stay minimal for now; not decided yet.
- **No physical ASUS mouse/keyboard was available during slice 5's verification** — HID connect/sync/lighting *traffic* through the moved `Peripherals/` classes is unexercised. Constructor/static-table integrity was verified by reflecting over all 103 concrete `IPeripheral` implementations (103 ok), but that's not the same as a live device round-trip. If you get access to real hardware, that's a gap worth closing.
- **Single-file-publish behaviour of `AppContext.BaseDirectory`** was reasoned about, not exercised — this machine only ran normal (non-single-file) Debug builds.
- **No test project exists in this repo.** Validation is: build both solutions 0/0, launch `GHelper.exe` manually, check the log and (where relevant) do a targeted behavioural check (e.g. the `Keys`→`Vk` byte comparison, the reflection-based `IPeripheral` construction sweep). Keep using that pattern — don't introduce a test project as a side effect of a slice unless asked.
- **Pre-existing dead code noticed, not touched**: `AsusMode` and `AsusGPU` enums in `AsusACPI.cs` are declared and referenced nowhere in the repo. Not this story's job to delete (per repo convention: mention, don't remove unasked-for dead code).

## 6. Important files and sections to read next

1. `context/laws.md`, `context/agenticworkflow.md` — always first, per `AGENTS.md`.
2. `context/design.md` — architecture and the Milestones Index; Story 5's Shared-layer inventory here was kept current through slice 3 and should be re-verified/extended for slices 4-5 if it hasn't been already (check the "G-Helper.Shared" subsection under "Application Layers").
3. `context/milestones/milestone-2.md`, Story 5 section — current status, "Moved so far" / "Still to do" bullets.
4. `plan.md` (this folder) — the slice table (row 6+) and the full Execution Log (slices 1-5) for the exact reasoning behind every non-obvious edge resolved so far. Read this before re-deriving anything above from scratch.
5. `context/wiki/upstream-source-mapping.md` — every path that stopped mapping 1:1 to upstream; update it for any new move.
6. `~/.claude/rules/csharp_rules.md` (global, not in-repo) — Allman braces, one-type-per-file, naming; match existing repo style over introducing new conventions when they conflict (e.g. this repo doesn't use the global rule's `in_` parameter prefix convention — one new file in slice 3 did, inconsistently; not worth a special pass to fix, but don't propagate it further).
7. Standing process notes, not written elsewhere in `context/`: proactively close file-lock-holding processes (build servers, an open Visual Studio instance) rather than stopping to ask; verify every sub-agent's work yourself (`git status`, `git diff`, an independent rebuild) before committing on its behalf.
