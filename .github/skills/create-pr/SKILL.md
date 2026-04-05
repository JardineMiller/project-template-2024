name: create-pr

description: |
Create or update a Pull Request (PR) using the GitHub CLI and a temporary PR body file.
The workflow below shows how to prepare code, use a temporary Markdown file for the PR body
(without committing it), and then delete that file. If the file is accidentally committed,
instructions for removing it are included.

steps:

- Ensure prerequisites:
  - `gh` (GitHub CLI) installed and authenticated: `gh auth login`.
  - `dotnet` SDK available (backend tasks use .NET).

- Prepare your change locally:
  1. Run formatting & tests locally (required before creating PR):
     - `dotnet tool restore`
     - `dotnet tool run csharpier -- format .` (or use your repo's preferred formatting command)
     - `dotnet build` and `dotnet test`
  2. Create and switch to a feature branch:
     - `git checkout -b feature/your-descriptive-name`
  3. Stage & commit changes:
     - `git add -A`
     - `git commit -m "feat: short description of change"`
  4. Push branch:
     - `git push -u origin HEAD`

- Create a temporary PR body file (do NOT `git add`/commit this file):
  - Bash example:

    ```bash
    cat > pr_body.md <<'PR'
    ## Summary

    Describe your change here.

    ## Testing

    - `dotnet test` passes

    PR
    ```

  - PowerShell example:

    ```powershell
    $body = @"
    ## Summary

    Describe your change here.

    ## Testing

    - `dotnet test` passes
    "@
    Set-Content -Path .\pr_body.md -Value $body -NoNewline
    ```

- Create or update the PR using the temporary file:
  - Create PR:

    ```bash
    gh pr create --title "chore: short title" --body-file pr_body.md --base main --head feature/your-descriptive-name
    ```

  - Edit an existing PR's description (without committing a file):
    ```bash
    gh pr edit <PR_NUMBER_OR_URL> --body-file pr_body.md
    ```

- Cleanup (delete the temporary file locally; do NOT commit it):
  - Bash: `rm pr_body.md`
  - PowerShell: `Remove-Item .\pr_body.md`

- If the temporary file was accidentally committed to the branch:
  1. Remove the committed file, commit, and push the removal:
     ```bash
     git rm path/to/pr_body.md
     git commit -m "chore: remove temporary PR body file"
     git push
     ```

notes:

- Use this skill to avoid committing non-source PR artifacts to the repo. Create the temporary
  file only locally (or in your system temp dir), use it with `gh` to set the PR body, then
  delete it immediately.
