# Agent Skill — Repo Operator Guide

Purpose

- Document what the Copilot agent may do in this repository and how humans can control agent behavior.

Capabilities

- Create branches, run local builds/tests, add code and tests, scaffold features, and open PRs for review.

Restrictions

- Agent must not commit secrets or change GitHub repository secrets.
- Agent must not merge PRs to protected branches without explicit human approval.

Escalation

- If an agent-created PR needs urgent human attention, tag `@repo-owner` or the on-call maintainer.
