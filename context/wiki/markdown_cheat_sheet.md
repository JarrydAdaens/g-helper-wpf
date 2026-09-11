---
name: wiki-markdown-cheat-sheet
description: Cheat sheet for core Markdown syntax, common extensions, and major flavours.
metadata:
  version: "1.1"
  agentic_rails_source_version: "1.1"
  owner: "Your Name"
  repo: "your-repo"
---

# Markdown Cheat Sheet

[Back to Wiki Home](home.md)

Markdown is a lightweight markup language used to structure plain text. In an agentic IDE workflow, it is one of the main formats used to encode context clearly for both humans and agents.

---

## Basic Syntax

**Headings:** Use 1 to 6 hash marks (`#`) for ATX-style headings, or underlines (`=` or `-`) for Setext-style headings.

```markdown
# Heading 1
## Heading 2

Heading 1
=========
```

**Emphasis:** Wrap text in single asterisks `*` or underscores `_` for *italics*, and double `**` or `__` for **bold** text.

```markdown
*Italic text* and **Bold text**
```

**Lists:** Create unordered lists using `*`, `+`, or `-`. Create ordered lists using numbers followed by periods (`1.`, `2.`). Some processors also accept parentheses such as `1)`, but periods are more widely portable.

```markdown
* Unordered item
1. Ordered item
```

**Links:** Create inline links with `[text](url "optional title")` and reference links with `[text][id]` paired with `[id]: url` elsewhere in the document.

```markdown
[OpenAI](https://openai.com)
```

**Images:** Use an exclamation mark before a link, like `![alt text](url)`.

```markdown
![Logo](image.png "Optional Title")
```

**Blockquotes:** Precede lines with a greater-than sign `>`.

```markdown
> This is a blockquote.
```

**Code:** Wrap inline code in backticks `` `code` ``. For code blocks, either indent lines by 4 spaces or 1 tab, or use fenced code blocks with triple backticks.

```markdown
`inline code`
```

**Horizontal Rules:** Use three or more hyphens `---`, asterisks `***`, or underscores `___` on a single line.

**Line Breaks:** End a line with two or more spaces, or use a backslash `\` at the end of the line.

---

## Extended Syntax

Not all Markdown processors support these features, but they are common in many modern applications.

**Tables:** Use pipes `|` to separate columns and hyphens `-` for the header row. Colons `:` can align columns.

```markdown
| Syntax | Description |
| :--- | :---: |
| Left aligned | Center aligned |
```

**Fenced Code Blocks:** Wrap blocks of code in triple backticks ````` ``` ````` or tildes `~~~`, optionally specifying a language name for syntax highlighting.

**Strikethrough:** Wrap text in double tildes `~~text~~` to strike through words.

**Task Lists:** Use brackets to create checkboxes: `- [ ]` for incomplete tasks and `- [x]` for complete tasks.

**Footnotes:** Add `[^1]` in text and define it with `[^1]: definition` at the bottom of your document.

**Definition Lists:** Type the term on one line, and a colon followed by a space on the next line for the definition.

```markdown
Term
: Definition of the term
```

---

## Flavours

Over the years, several variants of Markdown have been developed to adapt the language to different platforms. Here are three major flavours of Markdown and the syntax they commonly add or emphasize.

### 1. GitHub Flavored Markdown (GFM)

GFM is the dialect used on GitHub and is a strict superset of CommonMark.

- **Autolink literals:** Bare URLs and some email addresses are automatically linked.
- **Tables:** GFM formally supports pipe table syntax.
- **Task lists:** GFM supports checkbox list items such as `- [ ]` and `- [x]`.
- **Strikethrough:** GFM supports `~~strikethrough~~`.
- **Alerts:** GitHub supports callout-style blockquotes like `> [!NOTE]`, `> [!WARNING]`, and `> [!CAUTION]` in many surfaces.
- **Mentions and references on GitHub:** In GitHub itself, `@username` and references like `#123` can link to users, issues, or pull requests depending on context.

### 2. Markdown Extra

Markdown Extra is a variant originally implemented in PHP that adds several advanced features.

- **Definition lists:** Adds the `Term` followed by `: Definition` syntax.
- **Footnotes:** Supports inline footnote references and footnote definitions.
- **Tables:** Adds pipe table support.
- **Markdown inside HTML:** Some block-level HTML tags can process Markdown when marked with `markdown="1"`.
- **Abbreviations:** Define reusable abbreviations with `*[HTML]: HyperText Markup Language`.

### 3. MultiMarkdown

MultiMarkdown extends standard Markdown for more complex document formatting and publishing workflows.

- **Document metadata:** Supports metadata blocks at the top of a document.
- **Tables and definition lists:** Includes richer support for structured document syntax.
- **Footnotes and citations:** Expands support for scholarly-style referencing.
- **Math support:** Often used with workflows that support LaTeX-style math notation.
- **Superscript and subscript:** Commonly supports `^superscript^` and `~subscript~`.

---

## Practical Advice

- Prefer standard Markdown when portability matters.
- Use GitHub Flavored Markdown for repository docs and wiki content unless a tool requires another dialect.
- Be cautious with advanced features like definition lists, footnotes, alerts, and metadata because support varies by renderer.
- When Markdown is used as agent context, optimize for clarity, stable headings, short sections, and explicit examples.
