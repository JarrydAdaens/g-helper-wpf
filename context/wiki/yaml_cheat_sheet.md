---
name: wiki-yaml-cheat-sheet
description: Cheat sheet for YAML syntax, data types, collections, and practical usage caveats.
metadata:
  version: "1.1"
  agentic_rails_source_version: "1.1"
  owner: "Your Name"
  repo: "your-repo"
---

# YAML Cheat Sheet

[Back to Wiki Home](home.md)

YAML is a human-readable data serialization format commonly used for configuration, metadata, and structured context files. In agentic workflows, it is useful because it is compact, hierarchical, and easy for both humans and tools to scan.

---

## Basic Syntax

**Key-Value Pairs:** Maps, also called dictionaries, use the form `key: value`.

**Indentation:** YAML relies on indentation to denote structure. Use spaces only, typically 2 spaces per level, and never tabs.

**Comments:** Comments start with `#` and continue to the end of the line.

```yaml
# This is a comment
person:
  name: John Doe
  age: 30
```

---

## Data Types (Scalars)

YAML often auto-detects scalar types, but exact behavior can vary between parsers and YAML versions.

**Strings:** Can be unquoted, single-quoted `''`, or double-quoted `""`. Double quotes allow escape sequences like `\n`.

**Numbers:** Can be integers or floats. Many parsers also support hexadecimal and binary forms.

**Booleans:** In YAML 1.2, the most portable boolean values are `true` and `false`. Some parsers also accept older YAML 1.1 forms such as `yes`, `no`, `on`, and `off`.

**Nulls:** Common null forms include `null`, `~`, or an empty value.

```yaml
string_unquoted: Hello World
string_quoted: "Hello\nWorld"
integer: 42
float: 3.14159
infinity: .inf
boolean_true: true
boolean_legacy: yes
null_value: ~
```

---

## Collections

**Lists (Sequences):** Ordered collections where each item starts with `- `.

**Mappings (Dictionaries):** Key-value collections.

**Flow Style:** A compact JSON-like syntax that uses `[]` for lists and `{}` for mappings.

```yaml
# Block Style List
fruits:
  - Apple
  - Mango
  - Banana

# Flow Style List and Mapping
flow_fruits: [Apple, Mango, Banana]
flow_person: {name: John, age: 30}
```

---

## Multi-Line Strings

YAML provides two main block scalar styles for longer text.

**Literal Style (`|`):** Preserves line breaks.

**Folded Style (`>`):** Folds most single line breaks into spaces, while preserving paragraph breaks.

```yaml
literal_block: |
  This preserves
  all line breaks
  exactly as written.

folded_block: >
  This folds single newlines
  into spaces, making it a
  single string.
```

**Block Chomping Indicators:** Add a suffix to control trailing newlines and blank lines.

- **`-` (Strip):** Removes the final newline and trailing blank lines, as in `|-` or `>-`.
- **`+` (Keep):** Keeps the final newline and trailing blank lines, as in `|+` or `>+`.

---

## Advanced Features

**Anchors (`&`) and Aliases (`*`):** Use an anchor like `&name` to label a node, and an alias like `*name` to reuse it elsewhere.

**Merge Keys (`<<:`):** Some parsers support merging one mapping into another with `<<:`. This is widely used in practice, but support can vary because it is not consistently treated across YAML tooling.

```yaml
default_settings: &defaults
  timeout: 30
  retries: 3

service_a:
  <<: *defaults
  retries: 5
```

**Explicit Typing (Tags):** Tags can force a value to be treated as a specific type, such as `!!str`, `!!int`, `!!float`, or `!!bool`.

```yaml
forced_string: !!str yes
forced_int: !!int "42"
```

**Multiple Documents:** Separate multiple YAML documents in one file with `---`. A document can optionally end with `...`.

```yaml
---
document: one
---
document: two
...
```

---

## Practical Advice

- Prefer `true` and `false` over `yes` and `no` unless you know the parser expects YAML 1.1 behavior.
- Keep indentation consistent throughout a file.
- Quote values when they could be misread as booleans, numbers, dates, or nulls.
- Be cautious with anchors, aliases, merge keys, and tags because support differs across tools.
- When YAML is used as agent context, favor stable keys, small nested structures, and clear examples over clever compression.
