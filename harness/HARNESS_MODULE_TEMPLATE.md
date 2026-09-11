---
name: harness-module-template
description: Copy into harness/<module-name>/README.md and fill in. The README is the module's entry point and contract.
metadata:
  version: "3.2"
  agentic_rails_source_version: "3.2"
  owner: "Your Name"
  repo: "your-repo"
---
<!-- After copying: replace the frontmatter above with the module's own
     name, one-line description, and metadata (version, owner, repo). -->
# <Module Name>

<One paragraph: what this module checks or does, and what kind of module it
is (verifier | gate | guardrail seam | sensor | actuator). If it is the seam
for an installed marketplace plugin, name the plugin and link its README.>

## Trigger

<When an agent must run this module. One or two sentences, concrete.>

## Contract

```bash
<the one command an agent runs>
```

| Exit code | Meaning |
| --- | --- |
| 0 | pass |
| 1 | fail (<what a real failure means>) |
| 2 | invalid arguments / config |
| 3 | missing required tooling |
| <n> | <other distinct infrastructure failures> |

Output: one compact `PASS`/`FAIL` line with the reason, plus a JSON report at
`<module>/runs/...` (git-ignored) explaining why.

## Setup

<What must exist before the first run: config file to fill in, truth/golden
data to capture, drivers to replace, gitignore lines to add.>

## Self-test

```bash
<command that proves the module's logic without the real app or hardware>
```

## Seam

<Exactly which files are project-specific and meant to be edited, versus the
stable core that is not touched. For a plugin seam module: everything here is
the seam; the engine lives in the plugin.>
