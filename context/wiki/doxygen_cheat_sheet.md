---
name: wiki-doxygen-cheat-sheet
description: Cheat sheet for Doxygen comment syntax, structural commands, and documentation features.
metadata:
  version: "1.1"
  agentic_rails_source_version: "1.1"
  owner: "Your Name"
  repo: "your-repo"
---

# Doxygen Comments Cheat Sheet

[Back to Wiki Home](home.md)

Doxygen parses source files and extracts specially formatted comments to generate reference documentation. Most commands can begin with either a backslash (`\`) or an at-sign (`@`), and the two forms are generally interchangeable.

---

## Supported Languages

Doxygen is commonly used with the following languages:

- C
- C++
- C#
- Objective-C
- Java
- PHP
- Python
- Fortran
- VHDL

It also has support for additional language modes and file formats depending on version and configuration, so exact behavior can vary by project setup.

---

## 1. Comment Block Syntax

You must use recognized documentation comment forms for Doxygen to extract text.

### C-like Languages

For C, C++, C#, Objective-C, PHP, and Java, common forms include:

- **Javadoc style:** `/** ... */`
- **Qt style:** `/*! ... */`
- **Single-line styles:** `///` or `//!`
- **After-member forms:** `/**< ... */`, `/*!< ... */`, `///< ...`, or `//!< ...`

```cpp
/**
 * \brief Adds two numbers.
 * \param left The first operand.
 * \param right The second operand.
 * \return The sum of the inputs.
 */
int Add(int left, int right);
```

### Python

Doxygen can work with Python docstrings and also with specially formatted comments such as `##`, depending on configuration and parsing rules.

### VHDL

Use comment forms such as `--!`.

### Fortran

Common forms include `!>` and `!<` in free format, and `C>` in fixed format.

---

## 2. Structural Indicators

These commands indicate what kind of element is being documented.

- **`\class`**, **`\struct`**, **`\union`**, **`\enum`**: Documents a class, struct, union, or enum.
- **`\fn`**, **`\var`**, **`\property`**, **`\typedef`**: Documents a function, variable, property, or typedef.
- **`\def`**: Documents a macro definition.
- **`\file`**, **`\dir`**: Documents a file or directory.
- **`\namespace`**, **`\package`**, **`\module`**: Documents a namespace, package, or module.
- **`\interface`**, **`\protocol`**, **`\concept`**: Documents an interface, protocol, or concept where supported.
- **`\mainpage`**, **`\page`**, **`\subpage`**: Creates custom pages and page hierarchies.
- **`\headerfile`**: Specifies the header to include for a documented symbol.
- **`\overload`**: Generates text indicating that a member is an overload.

If you want global objects such as macros, typedefs, or free functions to appear under a file, documenting the file with `\file` is often important.

---

## 3. Section Indicators

These commands create specific sections or semantic blocks inside a comment.

- **`\brief`** or **`\short`**: Short summary text.
- **`\details`**: Detailed description text.
- **`\param`**: Describes a parameter, optionally with directions such as `[in]`, `[out]`, or `[in,out]`.
- **`\tparam`**: Describes a template parameter.
- **`\return`**, **`\returns`**, **`\result`**: Describes the return value.
- **`\retval`**: Documents a specific return code or discrete return value.
- **`\exception`**, **`\throw`**, **`\throws`**: Documents exceptions.
- **`\attention`**, **`\warning`**, **`\important`**, **`\note`**, **`\remark`**, **`\remarks`**: Adds callout-style sections.
- **`\author`**, **`\authors`**: Documents authorship.
- **`\copyright`**, **`\version`**, **`\since`**, **`\date`**: Adds metadata.
- **`\bug`**, **`\todo`**, **`\test`**: Adds entries to generated bug, todo, or test lists when configured.
- **`\pre`**, **`\post`**, **`\invariant`**: Documents preconditions, postconditions, and invariants.
- **`\deprecated`**: Marks an entity as deprecated.
- **`\par`**, **`\parblock`**, **`\endparblock`**: Starts custom paragraph sections.
- **`\xrefitem`**: Defines a custom cross-referenced section category.

Some Doxygen setups also support requirement-traceability style commands, but those are more specialized and may depend on project tooling or extensions.

---

## 4. Visibility, Relationships, and Graph Commands

These commands affect visibility, relationships, or generated diagrams.

- **Visibility:** `\public`, `\protected`, `\private`, `\publicsection`, `\protectedsection`, `\privatesection`, `\static`, `\pure`, `\internal`, `\endinternal`
- **Code expansion:** `\showinitializer`, `\hideinitializer`, `\showenumvalues`, `\hideenumvalues`, `\showinlinesource`, `\hideinlinesource`
- **References:** `\showrefby`, `\hiderefby`, `\showrefs`, `\hiderefs`
- **Relationships:** `\extends`, `\implements`, `\memberof`, `\relates`, `\related`, `\relatesalso`, `\relatedalso`
- **Graphs:** `\callgraph`, `\hidecallgraph`, `\callergraph`, `\hidecallergraph`, `\includegraph`, `\hideincludegraph`, `\includedbygraph`, `\hideincludedbygraph`, `\directorygraph`, `\hidedirectorygraph`, `\collaborationgraph`, `\hidecollaborationgraph`, `\inheritancegraph`, `\hideinheritancegraph`, `\groupgraph`, `\hidegroupgraph`

Graph generation depends on Doxygen configuration and, for some diagrams, external tools such as Graphviz.

---

## 5. Grouping Commands

These commands organize members and symbols into logical topics or modules.

- **`\defgroup`**: Defines a new group.
- **`\addtogroup`**, **`\weakgroup`**: Adds items to an existing group.
- **`\ingroup`**: Places a symbol into a group.
- **`\name`**: Creates a custom member-group header.
- **`@{`** and **`@}`**: Start and end grouped regions.
- **`\nosubgrouping`**: Prevents automatic subgrouping under access-level headings.

---

## 6. Visual Enhancements and Formatting

Doxygen supports its own markup commands and can also process Markdown in many contexts.

- **Font styling:** `\a`, `\e`, `\em` for emphasis, `\b` for bold, `\c` and `\p` for code-style inline text
- **Code blocks:** `\code` and `\endcode`
- **Verbatim blocks:** `\verbatim` and `\endverbatim`
- **Copying docs:** `\copydoc`, `\copybrief`, `\copydetails`
- **Images:** `\image`
- **Graphs and diagrams:** `\dot` / `\enddot`, `\msc` / `\endmsc`, `\plantumlfile`, and in newer setups sometimes Mermaid support depending on version and configuration
- **Lists:** `\li` and related list markup
- **Emoji:** Some recent Doxygen versions support `\emoji`, but support depends on version

Support for Markdown, XML-style tags, Mermaid, and related formatting features can differ by Doxygen version and output format.

---

## 7. Links and Indexing

- **`\ref`**: Links to a section, page, anchor, or symbol.
- **`\link`** and **`\endlink`**: Creates a link with custom link text.
- **`\anchor`**: Defines a link target.
- **`\addindex`**: Adds a term to the generated index.
- **`\cite`**: Adds a bibliographic reference.
- **`\section`**, **`\subsection`**, **`\subsubsection`**, **`\paragraph`**, **`\subparagraph`**: Creates headings inside pages or long descriptions.
- **`\tableofcontents`**: Generates a table of contents for a page.
- **`\secreflist`**, **`\endsecreflist`**, **`\refitem`**: Builds a list of section references.

---

## 8. Snippets, Examples, and External Inclusions

You can include external files or fragments directly into documentation.

- **`\example`**: Links to an example source file.
- **`\include`**: Includes a complete source file as formatted code.
- **`\dontinclude`**, **`\line`**, **`\skip`**, **`\skipline`**, **`\until`**: Lets you pull in selected file content in stages.
- **`\snippet`**: Includes a marked fragment from a source file.
- **`\verbinclude`**: Includes a file verbatim.
- **Format-specific includes:** `\htmlinclude`, `\latexinclude`, `\rtfinclude`, `\maninclude`, `\docbookinclude`, `\xmlinclude`

---

## 9. Conditional Documentation

These commands let you include or exclude content depending on configuration or output.

- **`\if`**, **`\elseif`**, **`\else`**, **`\endif`**: Includes content based on enabled section labels.
- **`\ifnot`**: Includes content only when a section label is not enabled.
- **`\cond`** and **`\endcond`**: Excludes sections from normal processing.
- **Output-specific blocks:** `\htmlonly`, `\endhtmlonly`, `\latexonly`, `\endlatexonly`, `\rtfonly`, `\manonly`, `\docbookonly`, `\xmlonly`

---

## Practical Advice

- Prefer a small, consistent subset of Doxygen commands across a codebase rather than using every available feature.
- Use `\brief`, `\param`, `\return`, `\tparam`, and `\note` as a strong default set for API docs.
- Document files with `\file` when you want file-level free functions, macros, or typedefs to appear clearly in output.
- Be careful with advanced features like graphs, conditional blocks, Mermaid, and traceability commands because they can depend on project configuration or Doxygen version.
- When a project already uses Markdown or XML-style comments heavily, verify how that repository's Doxygen config renders them before standardizing on one style.
