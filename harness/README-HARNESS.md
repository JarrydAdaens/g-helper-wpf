---
name: harness-readme
description: Guidance for the project harness — one self-contained module per verifier, gate, guardrail seam, sensor, or actuator that lets agents operate, test, and validate this project's real behavior.
metadata:
  version: "3.2"
  agentic_rails_source_version: "3.2"
  owner: "Your Name"
  repo: "your-repo"
---
# Harness

`harness/` is the project's agentic test harness in the classic CI sense: the
rig that lets automation run checks against the real application and collect
logs, evidence, and verdicts. It holds the machinery agents use to operate,
observe, and validate this project — verifiers, gates, guardrail seams,
sensors, and actuators. It is machinery, not narrative: per-story evidence and
outcomes belong in the relevant implementation plan under `context/`, never
here.

## The shape

One self-contained, kebab-case folder per module. A module is one verifier,
gate, guardrail seam, sensor, or actuator — the unit of independent adoption
and removal. The anatomy of a runnable module:

```text
harness/
├── README-HARNESS.md              # this file: home rules, module contract, module index
├── HARNESS_MODULE_TEMPLATE.md     # copy into <module-name>/README.md when starting a module
│
└── <module-name>/                 # one folder per module, named for what it checks or does
    ├── README.md                  # the module's entry point and contract, from the template
    ├── config.json                # the tuning seam: thresholds, paths, patterns, commands
    ├── run.ps1                    # the one entry script an agent runs (or scripts/ for a family)
    ├── .gitignore                 # module-local: ignores the module's own runtime output
    ├── truth/                     # committed reference data the module compares against,
    │                              #   when it has any — also seen as goldens/ or fixtures/
    └── runs/                      # git-ignored: one timestamped folder per run, each
                                   #   holding report.json and any captured evidence
```

Not every module has every piece. In practice the anatomy flexes at three
points, and only these:

- **Entry point.** A single `run.ps1` at the module root when one command
  covers it; a `scripts/` folder when the module is a family of related
  entry points (capture, normalize, gate, self-test). Either way the README's
  Contract section names exactly one command per job.
- **Committed truth.** Only comparison modules carry it. Name the folder for
  what it holds — `truth/` (reference images), `goldens/` (approved
  screenshots), `fixtures/` (recorded inputs).
- **Instrument-only modules.** A sensor or actuator that takes readings or
  drives a device rather than issuing a verdict can be as small as
  `README.md` plus `scripts/` — no config, no runs.

Runtime output stays inside the module and out of git: `runs/` (timestamped
per-run folders, often with a `-latest` convenience copy), `last-run/`, or
`captures/`, ignored by the module's own `.gitignore` so adopting or removing
the module never touches the repository root ignore file.

## Starting a module

1. Create `harness/<module-name>/` — kebab-case, named for what it checks or
   does.
2. Copy `HARNESS_MODULE_TEMPLATE.md` to `<module-name>/README.md` and fill it
   in: trigger, contract, setup, self-test, seam.
3. Put the tunable values in `config.json` and the logic in the entry script;
   agents tune the config, not the code.
4. Add the module-local `.gitignore` for its output folders.
5. Prove the self-test passes without the real app or hardware.
6. Add the module to the index table at the bottom of this file, and wire its
   trigger line into `AGENTS.md` §0.2 so agents know when they must run it.

## The three-home rule

Every piece of harness machinery has exactly one home:

| Reusable across projects? | Has a lifecycle (hooks, engine updates)? | Home |
| --- | --- | --- |
| Yes | Yes | A marketplace plugin (`agentic_rails_marketplace`) — this folder holds only its per-project seam |
| Yes | No (inert procedure or knowledge) | A tooling-repo skill or agent, deployed by Kung Fu |
| No — project-specific | — | A full local module in this folder |

The promotion path: a check is born here as a local module, proves itself,
and graduates through the tooling repo's proposals inbox into the
marketplace. Its folder here then shrinks to just the seam, keeping the same
module name.

## Module rules

- **One self-contained folder per module**, kebab-case, named for what it
  checks or does. Everything the module needs — README, config, scripts,
  fixtures, truth/golden data — lives inside it, so the module can be
  adopted, disabled, or removed without leaving pieces elsewhere in the
  repository. No shared `scripts/` grab-bag between modules.
- **A seam module for an installed plugin** contains exactly what that
  plugin's README prescribes (typically `config.json`, defaults, drivers,
  goldens) and nothing else; its engine updates through the marketplace.
- **Runtime output is git-ignored inside the module**: `runs/`, `state/`,
  `last-run/`, `captures/`, `reports/`. Tracked files define the module;
  ignored files preserve local durability across turns.
- **No secrets.** Tokens and credentials live at the user level (environment
  variables, dotnet user-secrets, OS keychains), never in a module folder.

## Module contract

Every runnable module meets this contract, stated in its README:

1. **Trigger** — when an agent must run it, in one or two sentences.
2. **Stable exit codes** — `0` pass, `1` fail, and documented codes for
   distinct infrastructure failures (bad arguments, missing tooling, capture
   failure). "Couldn't check" is always a failure, never a silent pass.
3. **Agent-parseable output** — one compact `PASS`/`FAIL` line with the
   reason, plus a JSON report in the module's ignored output folder that says
   *why* it failed, not just that it failed.
4. **Config, not code, as the tuning seam** — thresholds, paths, regions,
   and commands live in a config file so agents tune behavior without editing
   scripts.
5. **A self-test** that proves the module's logic without the real
   app/hardware, so the harness itself is verifiable after setup.

Evaluator-run gates (procedure documents executed by a sub-agent rather than
a script) meet the same spirit: explicit trigger, pass criteria, attempt
loop, and a fixed report format precise enough that any evaluator produces
the same verdict.

## Wiring

Modules only run if agents know they must. `AGENTS.md` §0.2 carries the
standing trigger lines; a module whose trigger is genuinely enforced (not
advisory) must come from a marketplace guardrail plugin's hooks — never from
hand-edited tool settings.

## Installed modules

Keep this table current; it is the harness's index.

| Module | Kind | Engine | Purpose |
| --- | --- | --- | --- |
| _(none yet)_ | | | |
