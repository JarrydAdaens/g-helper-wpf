---
name: app-structure-overview
description: Current top-level layout of app/ as of Milestone 1 completion — a rough, unverified first read on what looks like shared hardware/business logic vs UI, to seed Milestone 2 Story 5 extraction planning.
metadata:
  version: "1.0"
  agentic_rails_source_version: "3.0"
  owner: "Jarryd Adaens"
  repo: "g-helper-wpf"
---

# app/ Structure Overview

[Back to Wiki Home](home.md)

A snapshot of `app/`'s current layout, taken at Milestone 1 close, before Milestone 2 Story 5 extracts `GHelper.Shared`. This is a rough first read to seed that planning — not a verified extraction boundary. Confirm against actual code before relying on any "looks shared" / "looks UI" call below.

> Extraction has since started. For what has actually moved out of `app/`, see [upstream-source-mapping.md](upstream-source-mapping.md); this page is not maintained as files move.

## Top-Level Folders

| Folder | Likely shared logic? | Notes |
| --- | --- | --- |
| `Ally/` | Yes | ROG Ally-specific handheld behaviour (touch-keyboard gesture, etc.) |
| `AnimeMatrix/` | Yes | Hardware control for AniMe Matrix LED displays |
| `AutoUpdate/` | Mixed | Update-check logic is shared; any UI prompt is not |
| `Battery/` | Yes | Battery/charge-limit hardware logic |
| `Display/` | Yes | Display mode/refresh-rate hardware control |
| `Fan/` | Yes | Fan curve hardware control |
| `Gpu/` | Yes | GPU mode/power hardware control |
| `Helpers/` | Yes | Cross-cutting hardware/OS helpers (includes `AsusService.cs`, `Logger.cs`) |
| `Input/` | Yes | Controller/input handling — the layer the new binding system builds on |
| `Mode/` | Yes | Power-mode hardware control |
| `Overlay/` | No | WinForms overlay UI |
| `Pawn/` | Yes | Embedded SMU binaries + interop |
| `Peripherals/` | Yes | External device (mouse/keyboard) hardware control |
| `Properties/` | No | WinForms designer/resources/settings scaffolding |
| `Resources/` | No | Icons/images/strings for the WinForms UI |
| `UI/` | No | WinForms controls and forms |
| `USB/` | Yes | USB-C performance mode hardware control |

## Top-Level Files (app/ root)

Config/settings (`AppConfig.cs`, `Settings.cs` + `.Designer.cs`, `OverlayConfig.cs`, `AsusKeyboardSettings.cs`, `AsusMouseSettings.cs`, `Matrix.cs`, `Fans.cs`, `Extra.cs`, `Slash.cs`, `Handheld.cs`, each with a paired `.Designer.cs`) sit at the root alongside hardware/ACPI access (`AsusACPI.cs`, `HardwareControl.cs`), `Program.cs` (entry point / `StartupObject`), `NativeMethods.cs`, and update logic (`Updates.cs`, `UpdatesController.cs`). The `.Designer.cs` files are WinForms/`.settings` codegen and stay with their UI-adjacent counterparts; the plain `.cs` files alongside them are mostly config data classes, not UI, but that hasn't been individually verified.

## Why This Exists

Milestone 2 Story 5 needs to derive the real `G-Helper.Shared` boundary from the actual code (per [design.md](../design.md#open-questions) — this folder list is not a verified inventory). This page exists so that story starts from a rough map instead of a blank read of ~17 folders and ~25 root files.
