---
name: wiki-xml-documentation-comments-cheat-sheet
description: Cheat sheet for C# XML documentation comment syntax, tags, and writing conventions.
metadata:
  version: "1.1"
  agentic_rails_source_version: "1.1"
  owner: "Your Name"
  repo: "your-repo"
---

# XML Documentation Comments Cheat Sheet

[Back to Wiki Home](home.md)

XML documentation comments are used to generate API documentation from source code and to power IntelliSense in .NET tooling. They are officially supported by the C# compiler.

---

## Basic Syntax

Documentation comments should immediately precede the type or member they document.

- **Single-line comments:** Start with `///`.
- **Multi-line comments:** Start with `/**` and end with `*/`, though `///` is the normal C# convention and is more commonly used.

```csharp
/// <summary>
/// Initializes a new instance of the class.
/// </summary>
```

---

## Core Structure Tags

These tags define the primary information about a type or member.

- **`<summary>`**: Provides the main high-level description of a type or member. This text is commonly shown in IntelliSense.
- **`<remarks>`**: Adds supplementary information beyond the summary.
- **`<value>`**: Describes the value represented by a property.
- **`<permission>`**: Documents security access requirements for a member. This tag exists, but it is relatively uncommon in modern C# codebases.

---

## Method and Parameter Tags

These tags are especially important for documenting method behavior.

- **`<param name="name">`**: Describes a parameter for a method, constructor, or indexer.
- **`<paramref name="name"/>`**: Refers inline to a parameter name.
- **`<returns>`**: Describes a method return value.
- **`<exception cref="member">`**: Documents exceptions that can be thrown and when they occur.
- **`<typeparam name="name">`**: Describes a generic type parameter on a type or method.
- **`<typeparamref name="name"/>`**: Refers inline to a generic type parameter.

---

## Linking and Reusing Text

These tags help cross-reference other APIs or reuse existing documentation.

- **`<see cref="member"/>`**: Creates an inline reference to another code element.
- **`<see href="url">text</see>`**: Creates an inline external hyperlink.
- **`<see langword="keyword"/>`**: Formats a C# language keyword such as `true`, `false`, `null`, or `await`.
- **`<seealso cref="member"/>`**: Adds an item to a "See Also" section in generated documentation.
- **`<inheritdoc cref="member"/>`**: Inherits documentation from a base member, interface member, or other referenced symbol, depending on tooling support.
- **`<include file="filename" path="xpath" />`**: Includes documentation from an external XML file using XPath.

---

## Formatting and Layout Tags

These tags help organize rendered documentation.

- **`<para>`**: Splits text into paragraphs.
- **`<c>`**: Formats a short inline code fragment.
- **`<code>`**: Formats a block of code or output.
- **`<example>`**: Marks an example section, often containing a `<code>` block.
- **`<list>`**: Creates a bullet list, number list, or table using nested `<item>` elements, and optionally `<listheader>`, `<term>`, and `<description>`.

Some basic HTML tags are also commonly accepted by tooling, including:

- **`<b>`**, **`<i>`**, **`<u>`**: Basic text formatting.
- **`<br/>`**: Inserts a line break.
- **`<a>`**: Creates a hyperlink in some documentation renderers.

Support for HTML tags can vary by compiler, IDE, and documentation generator, so XML doc tags are generally more portable.

---

## Best Practices for Writing C# Comments

These summary patterns are widely used in .NET codebases.

- End summaries and descriptions with a period.
- For classes, start summaries with phrases like `Represents...` or `Provides...`.
- For constructors, use `Initializes a new instance of the ... class.`
- For properties, use `Gets or sets...`, `Gets...`, or `Sets...` as appropriate.
- For boolean properties, use `Gets or sets a value indicating whether...`
- For methods, begin with a present-tense verb such as `Creates`, `Calculates`, or `Returns`.
- For events, start with `Occurs when...`
- For boolean return values, use `true if CONDITION; otherwise, false.`

---

## Practical Advice

- Keep XML docs focused on intent, behavior, and edge cases rather than restating the method name.
- Document exceptions only when they are meaningful to callers.
- Use `<paramref>` and `<typeparamref>` so parameter and type parameter names are rendered consistently.
- Prefer `<inheritdoc/>` when implementations intentionally mirror inherited behavior.
- Treat XML documentation as part of the public API contract for reusable libraries and shared framework code.
