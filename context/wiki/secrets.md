---
name: wiki-secrets
description: Placeholder guidance for documenting secret and sensitive configuration handling.
metadata:
  version: "0.2"
  agentic_rails_source_version: "0.2"
  owner: "Your Name"
  repo: "your-repo"
---

# Secrets and Sensitive Configuration

This page is a placeholder. Replace it with the real secret-management approach for the project that adopts this template.

## Baseline Rules

- Never commit secrets to source control.
- Keep local development secrets outside the repository.
- Document the chosen secret workflow once the stack is known.
- Rotate and replace secrets through the real platform workflow, not by editing committed files.

## Common Options

Choose the approach that fits the actual project:

- Platform-native secret stores such as cloud secret managers
- Local development tools such as `.env` support or framework-specific secret storage
- CI/CD secret injection for deployment environments

## Minimum Documentation to Add Later

When the real project is defined, document:

1. Where secrets live in development
2. Where secrets live in deployed environments
3. How developers bootstrap a new machine
4. Which keys or credentials are required
5. How rotation and revocation are handled
