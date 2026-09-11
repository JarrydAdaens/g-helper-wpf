---
name: wiki-mermaid-chart-cheat-sheet
description: Cheat sheet for Mermaid class diagram syntax, relationships, styling, and practical usage.
metadata:
  version: "1.1"
  agentic_rails_source_version: "1.1"
  owner: "Your Name"
  repo: "your-repo"
---

# Mermaid Class Diagram Cheat Sheet

[Back to Wiki Home](home.md)

Mermaid class diagrams let you describe object-oriented structure in text that is easy to diff, review, and keep close to the code. They work well for conceptual modeling, design discussions, domain modeling, and lightweight technical documentation.

---

## When to Use Class Diagrams

Use class diagrams when you want to describe structure rather than runtime behavior.

- **Conceptual modeling:** Sketch the major domain concepts and how they relate before implementation details settle.
- **Detailed design:** Capture classes, members, and relationships in a form that can guide code structure.
- **Data and domain modeling:** Show entities, ownership, and multiplicity in a compact format.
- **Documentation close to code:** Keep architecture notes in Markdown rather than in external drawing tools.

If you need to describe order of execution, message flow, or process steps, a sequence diagram or flowchart is often a better fit.

---

## Basic Structure

Every Mermaid class diagram starts with the `classDiagram` keyword.

There are two main ways to define classes:

- **Explicit declaration:** `class Animal`
- **Implicit declaration through a relationship:** `Vehicle <|-- Car`

```mermaid
classDiagram
    class Animal
    Vehicle <|-- Car
```

### Class Names and Labels

Class names should normally use letters, numbers, underscores, or dashes. If you need a friendlier display label or special characters, use a label or escape the class name with backticks.

```mermaid
classDiagram
    class Animal["Animal with a label"]
    class `Car Class!`

    Animal --> `Car Class!`
```

---

## Members

Mermaid treats entries with parentheses `()` as methods and entries without parentheses as attributes.

You can define members one line at a time with `:` or group them inside `{}`.

```mermaid
classDiagram
    class BankAccount
    BankAccount : +String owner
    BankAccount : +BigDecimal balance
    BankAccount : +deposit(amount)
    BankAccount : +withdraw(amount) bool
```

```mermaid
classDiagram
    class BankAccount {
        +String owner
        +BigDecimal balance
        +deposit(amount)
        +withdraw(amount) bool
    }
```

### Visibility and Classifiers

Use these prefixes and suffixes on members:

| Marker | Meaning |
| :--- | :--- |
| `+` | Public |
| `-` | Private |
| `#` | Protected |
| `~` | Package or internal |
| `*` | Abstract method suffix |
| `$` | Static member suffix |

Examples:

```mermaid
classDiagram
    class Shape {
        +draw()*
        +getDefault() Shape$
        -int cacheSize$
    }
```

### Return Types

Return types go after the closing `)` and must be separated by a space.

```mermaid
classDiagram
    class Parser {
        +parse(text) Result
    }
```

### Generic Types

Generics use tildes `~type~`.

- Nested generics are supported.
- Generics with commas are not currently supported.
- When a class is declared as generic, the generic part is not treated as part of the class identity when you reference that class elsewhere.

```mermaid
classDiagram
    class List~T~ {
        +T[] items
        +add(item) void
    }

    class Map~K_V~

    class Repository {
        -List~string~ names
        +getAll() List~string~
    }
```

---

## Relationships

Mermaid supports these class relationship operators:

| Syntax | Meaning |
| :--- | :--- |
| `<|--` | Inheritance |
| `*--` | Composition |
| `o--` | Aggregation |
| `-->` | Association |
| `--` | Solid link |
| `..>` | Dependency |
| `..|>` | Realization |
| `..` | Dashed link |

```mermaid
classDiagram
    Animal <|-- Duck : inherits
    Order *-- LineItem : contains
    Team o-- Player : groups
    Customer --> Ticket : buys
    Service ..> Logger : depends on
    Repository ..|> IRepository : implements
```

### Labels and Direction

Add a label after `:` to explain the relationship. Reverse arrowheads are also allowed, and Mermaid supports two-way relations when you need a bidirectional association.

```mermaid
classDiagram
    Student <--> Course : attends
    Animal <|-- Bird : extends
```

### Cardinality or Multiplicity

Put multiplicities in quotes near the ends of the relationship.

Common values:

- `1`
- `0..1`
- `1..*`
- `*`
- `n`
- `0..n`
- `1..n`

```mermaid
classDiagram
    Customer "1" --> "*" Ticket : buys
    Professor "1" --> "1..*" Course : teaches
```

### Lollipop Interfaces

Lollipop notation connects a class to an interface-shaped endpoint.

```mermaid
classDiagram
    PaymentGateway ()-- CheckoutService
    CheckoutService --() LoggerPort
```

Each lollipop interface is unique in Mermaid and is not intended to be reused across several classes.

---

## Annotations and Structure

### Stereotypes

Use `<< >>` annotations to mark intent such as `interface`, `abstract`, `service`, or `enumeration`.

```mermaid
classDiagram
    class Shape {
        <<interface>>
        +draw()
    }

    class BaseHandler <<abstract>>
    class Color <<enumeration>>
```

### Namespaces

Use `namespace` to group related classes.

```mermaid
classDiagram
    namespace UserManagement {
        class User
        class Role
        class Permission
    }
```

### Notes

Use notes for short clarifications. Standalone notes apply to the diagram; `note for` attaches a note to a class.

```mermaid
classDiagram
    note "Keep notes short and diagram-focused"

    class Dog
    note for Dog "Good place for a quick design caveat"
```

### Comments

Lines that start with `%%` are comments and are ignored by the parser.

```mermaid
classDiagram
    %% This is a comment
    class Service
```

---

## Layout, Interaction, and Styling

### Direction

Use `direction` to control layout.

- `TB` top to bottom
- `BT` bottom to top
- `LR` left to right
- `RL` right to left

```mermaid
classDiagram
    direction LR

    Animal <|-- Duck
    Animal <|-- Fish
```

### Interaction

You can attach links or callbacks to classes. This only works when Mermaid is configured with `securityLevel='loose'`.

```mermaid
classDiagram
    class MermaidDocs

    click MermaidDocs href "https://mermaid.js.org/syntax/classDiagram.html" "Open the official Mermaid class diagram docs"
```

### Styling

You can style a node directly, define reusable style classes, or define a `default` class for all nodes.

```mermaid
classDiagram
    class ImportantClass
    class User:::userStyle

    style ImportantClass fill:#f96,stroke:#333,stroke-width:2px
    classDef userStyle fill:#ddf,stroke:#00f,stroke-width:2px
    classDef default fill:#eee,stroke:#333
```

Notes and namespaces cannot be styled individually, though they still respond to theme-level styling.

### Configuration

One useful class diagram setting is `hideEmptyMembersBox`, which removes empty member compartments from classes that only show a name.

```mermaid
%%{init: {'class': {'hideEmptyMembersBox': true}}}%%
classDiagram
    class EmptyClass
    class NamedOnly
```

---

## Practical Advice

- Keep class diagrams focused. Split large domains into multiple diagrams instead of forcing everything into one view.
- Prefer relationship labels that explain meaning, such as `owns`, `creates`, or `depends on`.
- Use notes for caveats, not for paragraphs of prose.
- Keep names stable so diagram diffs stay readable over time.
- Mermaid support varies by host application and embedded Mermaid version. If valid syntax fails, check the renderer version.
- Test complex diagrams in the [Mermaid Live Editor](https://mermaid.live/) or against the [official class diagram docs](https://mermaid.js.org/syntax/classDiagram.html).
