---
name: plan-extract-reusable-business-logic
description: Implementation plan for Milestone 2 Story 5 — incrementally extracting UI-independent logic out of the WinForms app/ project into GHelper.Shared.
metadata:
  version: "1.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---
# Story 5 — Extract reusable business logic

[Milestone 2](../../../milestones/milestone-2.md#story-5) · [design.md](../../../design.md#how-the-pieces-fit-together) · [upstream-source-mapping.md](../../../wiki/upstream-source-mapping.md)

## Objective

Move UI-independent logic out of `source/app/` into `GHelper.Shared`, one safe slice at a time, with `source/app/GHelper.sln` compiling and running correctly after every slice (Story 7).

## Estimates

Deliberately not scored per-slice. The story is open-ended by design; each slice is small enough to execute and verify in one pass, and the slice list below is the real unit of work.

## Approach

Slices are chosen by dependency direction, not by folder tidiness. A file can only move once nothing it calls is left behind in `source/app/`. Two files currently gate almost everything: `AppConfig` (uses WinForms `Application.StartupPath`, and references `AsusACPI` and `GHelper.Mode`) and `ProcessHelper` (uses `MessageBox`, `Application.Exit`, `Application.ExecutablePath`).

Where a type is part UI-specific and part not, split it and leave one-line delegating members behind in `source/app/` rather than churning every call site. Delete the shims when the remainder of the type moves.

## Slices

| # | Slice | Status |
| --- | --- | --- |
| 1 | Create `GHelper.Shared`; move `Logger`, `DeviceHelper`, `Keystone`; extract `UserIdentity` out of `ProcessHelper` | Done |
| 2 | Split `ProcessHelper`: move the process/service utilities, leave `CheckAlreadyRunning` and `RunAsAdmin` in the head | Done |
| 3 | Move `AppConfig` (replace `Application.StartupPath`; resolve the `AsusACPI` / `GHelper.Mode` edges first). Folded in: move the `AsusFan` enum, and split `Modes`' config reads out as `ModeConfig` | Done |
| 4 | Move `Pawn/`'s four `.cs` files (no UI references); leave `IntelMSR.bin` / `RyzenSMU.bin` embedded in `GHelper.csproj` | Done |
| 5 | Move `Peripherals/`'s 40 device files plus the `AnimeMatrix/Communication/` transport and the `AuraMode` / `AuraSpeed` enums they need; leave `PeripheralsProvider.cs` in the head | Done |
| 6+ | Remaining hardware modules, cheapest first: `PeripheralsProvider` (needs a callback seam), the rest of `USB/` and `AnimeMatrix/`, then `Battery/`, `Fan/`, `Display/`, `Gpu/`, the rest of `Mode/`, `Ally/`, `Input/` | Not started |

## Validation (per slice)

1. `dotnet build source\G-Helper.WPF.sln -c Debug` → 0 errors, 0 warnings.
2. Launch `source\app\bin\x64\Debug\net10.0-windows\GHelper.exe`, confirm the panel opens with live sensor readings and the log is still being written.
3. Update [upstream-source-mapping.md](../../../wiki/upstream-source-mapping.md) with a row for every path that stops mapping 1:1.

## Risk mitigation

- `GHelper.Shared` sets neither `UseWindowsForms` nor `UseWPF`, so any UI leak into the shared layer fails the build rather than passing review.
- Moves use `git mv` so history follows the file and upstream merges can still track it.
- The WinForms head is never allowed to be red between slices.

## Execution Log

**Slice 1 (2026-09-12).** Created `GHelper.Shared`; `git mv`'d `Logger.cs`, `DeviceHelper.cs`, `Keystone.cs` from `app/Helpers/`; added `UserIdentity.cs` holding the `IsRunningAsSystem` / `IsUserAdministrator` implementations lifted from `ProcessHelper`, which now delegates. Added the `ProjectReference` to `app/GHelper.csproj` and a root `G-Helper.WPF.sln`. Solution builds 0/0; WinForms head launches and behaves normally.

Slice 1 is intentionally small: it is the only set of files with no remaining dependency on anything that has to stay in `app/`, and moving `Logger` first is what unblocks every later slice.

**Slice 2 (2026-09-12).** Moved the remaining non-UI members of `ProcessHelper` (`KillByName`, `KillSmartDisplayControl`, `KillByProcess`, `StopDisableService`, `StartEnableService`, `RunCMD`, `SetPriority`) into a new `GHelper.Shared/Helpers/ProcessUtility.cs`. Named it `ProcessUtility` rather than reusing `ProcessHelper`, since both types share the `GHelper.Helpers` namespace and identical names across the app assembly and the `GHelper.Shared` assembly it references would make every unqualified call site ambiguous (CS0104) — the same naming pattern slice 1 already established for `UserIdentity`. `ProcessHelper` keeps one-line delegating shims for all seven, so its ~50 existing call sites across `source/app/` needed no changes. `CheckAlreadyRunning` and `RunAsAdmin` stayed in `app/` as real logic (WinForms `Application`/`MessageBox`). Updated the `ProcessHelper.cs` mapping row. Both solutions build 0/0; the WinForms head's behaviour is unaffected (delegation only, no logic changes).

**Slice 3 (2026-09-12).** `git mv`'d `AppConfig.cs` into `GHelper.Shared` whole, keeping its global namespace so all ~1000 `AppConfig.` call sites across `source/app/` resolve unchanged — the opposite of the shim pattern used in slices 1 and 2, and the right call here precisely because `AppConfig` is the file every later slice depends on. Shimming it would have forced a rename at every one of those call sites as each module moved.

Three edges resolved rather than shimmed:

- **`Application.StartupPath`** → `AppContext.BaseDirectory`. On modern .NET `Application.StartupPath` *is* `AppContext.BaseDirectory` (both include the trailing separator, which is what the existing `.Trim('\\')` was there for), and `AppContext.BaseDirectory` is also the only one of the two candidates that still resolves under a single-file publish — `Assembly.GetEntryAssembly().Location` returns an empty string there, which would have silently relocated the portable config. Confirmed at runtime: the launched head logs `Config loaded from C:\Users\<user>\AppData\Roaming\GHelper\config.json`, the same path as before.
- **`AsusACPI`** — only two things were actually needed: the `AsusFan` enum and the `PerformanceTurbo` / `PerformanceSilent` constants. `AsusFan` moved to `GHelper.Shared/AsusFan.cs` (global namespace, so its ~100 call sites are untouched), and the five `Performance*` constants moved to `ModeConfig` with `AsusACPI` keeping them as one-line aliases, so the values stay single-sourced and its own 20 call sites are untouched. The `AsusACPI` class itself did **not** move: it calls `Program.acpi` — the head's entry point — in five of its own instance methods. Rewriting those to `this` would probably be correct but is not provably behaviour-preserving, so it was left for a later slice.
- **`GHelper.Mode`** — `Modes` and `AppConfig` are mutually recursive. Only three `Modes` members are involved (`GetCurrent`, `GetBase`, `GetCurrentBase`) and all three are pure `AppConfig` reads, so they moved into a new `GHelper.Shared/Mode/ModeConfig.cs` with `Modes` keeping delegating members. The rest of `Modes` stays in the head, where it needs `Properties.Strings` and `Program.acpi`.

`System.Management` was added to `GHelper.Shared.csproj` for the WMI model detection that arrived with `AppConfig`. Both solutions build 0/0. Noted but deliberately untouched: `AsusMode` and `AsusGPU` in `AsusACPI.cs` are declared and never used anywhere in the repository — pre-existing dead code, not this slice's to delete.

**Slice 4 (2026-09-12).** Verified `Pawn/` first: grepped the folder for `System.Windows.Forms`, `MessageBox`, `Application.`, `Program.acpi`, `Properties.Strings` — zero matches, confirming the plan's "no UI references" claim still held. `git mv`'d the four `.cs` files (`CpuInfo.cs`, `IntelMsr.cs`, `PawnIOWrapper.cs`, `RyzenSmu.cs`) into `GHelper.Shared/Pawn/`, keeping the `PawnIO` namespace unchanged — call sites across `source/app/` use both `PawnIO.X` (`Extra.cs`, `HardwareControl.cs`, `Overlay/HardwareOverlay.cs`) and `using PawnIO;` (`Fans.cs`, `Mode/ModeControl.cs`, `Display/AmdDisplay.cs`, `Display/VisualControl.cs`, `Gpu/NVidia/NvidiaSmi.cs`), and neither needed edits since only the physical file location changed.

Found one entanglement the plan didn't anticipate: the folder also holds `IntelMSR.bin` and `RyzenSMU.bin`, embedded via `<EmbeddedResource Include="Pawn\...bin">` in `GHelper.csproj` with `LogicalName` `GHelper.IntelMSR.bin` / `GHelper.RyzenSMU.bin`. `IntelMsr.Initialize(Assembly)` and `RyzenSmuService.Initialize(Assembly)` derive the resource name from the *calling* assembly's own name at runtime (`assembly.GetName().Name + ".IntelMSR.bin"`), and both current callers (`HardwareControl.cs`, `Mode/ModeControl.cs`) pass the app's own assembly — so the `.bin` files only resolve correctly while embedded in `GHelper.exe`. Moving them into `GHelper.Shared/Pawn/` would have meant rewriting the `EmbeddedResource` paths in `GHelper.csproj` to reach across projects for no behavioural benefit, so they were left in `source/app/Pawn/` — a deliberate partial move, not an oversight. `GHelper.csproj` already had the `ProjectReference` to `GHelper.Shared` from slice 1; no csproj changes were needed for the `.cs` move itself.

Both solutions build 0/0. Launched `GHelper.exe` with its own directory as the working directory (so the auto-open-on-first-run path triggers); the panel opened titled `G-Helper - ROG Ally RC71L` showing live sensor readings (CPU 46°C / Fan 3200RPM, GPU 42°C / Fan 2800RPM — the AMD path through `PawnIO.CpuInfo.IsAMD`), and `log.txt` kept appending normally. Closed the process afterward.

**Slice 5 (2026-09-12).** Checked the plan's "UI references in one file of 41" claim before moving. It is accurate about *UI* references — `Program.settingsForm` / `Program.inputDispatcher` appear only in `PeripheralsProvider.cs` — but it understated the folder's coupling, because three non-UI edges also had to be resolved first:

- **`GHelper.AnimeMatrix.Communication.Device`** — `AsusKeyboard` and `AsusMouse` both derive from it, so the HID transport is a base class, not a neighbour. Moved `Device.cs`, `Packet.cs`, `Platform/UsbProvider.cs` and `Platform/WindowsUsbProvider.cs` into `GHelper.Shared/AnimeMatrix/Communication/`, namespaces unchanged. `Packet`'s constructor went `internal` → `protected` so `AnimeMatrixPacket` and `SlashPacket`, which stay in the head, can still derive from it. The other four `AnimeMatrix/` files did not move.
- **`GHelper.USB.AuraMode` / `AuraSpeed`** — used by the keyboard and mouse lighting sync. Moved into `GHelper.Shared/USB/`, keeping the `GHelper.USB` namespace, so call sites on both sides are untouched; the `Aura` class itself stayed (`Properties.Strings`, `Program.acpi`, `InputDispatcher`). Had these been left behind, `Peripherals` in Shared referencing `GHelper.USB` in the head would have been a circular assembly reference.
- **`System.Windows.Forms.Keys`** — one genuine WinForms dependency the plan did not list, in `AsusMouse`'s twelve default combo commands (`Hex(Keys.LMenu, Keys.Tab)`). Replaced with a private `Vk` byte-constant holder in the same file. Verified rather than assumed: a throwaway WinForms console printed the twelve original strings, and the twelve produced by the moved assembly match byte for byte (`0xA4 0x09`, `0xA4 0x73`, …).

Given that, the slice boundary was drawn at `PeripheralsProvider.cs`: 40 of 41 files (9,904 lines) `git mv`'d to `GHelper.Shared/Peripherals/`, and the provider left in `source/app/`. It is not shimmable the way `ProcessHelper` was — `UpdateSettingsView` and `RefreshHotkeys` are called from inside its own connect/disconnect/battery flows, not from its edges — so moving it needs a callback seam the WPF head can implement too, which is a decision worth its own slice rather than a rider on this one. Nothing inside the 40 moved files references `PeripheralsProvider`, so the split is clean in the dependency direction that matters.

`GHelper.Shared.csproj` gained `HidSharpCore` (already used by the head at the same version) and an explicit `<Using Include="System.Drawing" />`. The latter is not a new dependency: `System.Drawing.Color` is in the base framework and the head resolved it only through the implicit using that `UseWindowsForms` adds. Declaring it project-wide keeps the ~50 `Color` references across twelve moved files free of `using`-block churn that would conflict on an upstream merge.

Both solutions build 0/0. Validation beyond the build: launched `GHelper.exe`, panel opened titled `G-Helper - ROG Ally RC71L` with the normal startup log — including `AuraMode: AuraStatic` and the `Aura 1ABE` writes, which exercise the relocated enum — and no new errors. No ASUS mouse or keyboard is attached to this machine, so `DetectAllAsusMice` / `DetectAllAsusKeyboards` had nothing to connect to and that path runs inside a fire-and-forget `Task` in `Program.cs`, which would swallow a type-load failure silently. So a stronger check was run instead: a throwaway console referencing `GHelper.Shared.dll` reflected over the assembly and constructed **all 103 concrete `IPeripheral` implementations** (mice and keyboards) plus their static tables — 103 ok, 0 failed, `BindingCodes` 124 entries, `AuraKeyboardLayouts.Keys` 106 entries.
