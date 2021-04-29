# Springy

[![Unity 2018.4+](https://img.shields.io/badge/Unity-2018.4+-informational)][1]
[![License: MIT](https://img.shields.io/badge/License-MIT-informational)](LICENSE.md)
[![PRs welcome](https://img.shields.io/badge/PRs-welcome!-brightgreen)](#contributing)

Springy is a Unity editor extension that automatically collapses folders in the
project window when it is in the one-column layout.

## Installation

### Requirements

Springy has been tested and confirmed to work with Unity 2018.4 through 2020.3 
but you are welcome to try newer/older versions.

If you identify a compatible version that isn't already listed here or you 
add support for an incompatible version, please create a [PR](#contributing).

### Unity Package Manager

You can install Springy by git url in the Package Manager:

`https://github.com/aestheticianlabs/springy.git`


See [this guide](https://docs.unity3d.com/Manual/upm-ui-giturl.html) for more detail.

## Usage

If a folder isn't selected or [pinned](#pinning-folders), then Springy will 
automatically keep it collapsed. This helps keep the project window from getting 
cluttered, especially after using project search.

### Pinning folders

Pinned folders will not be automatically collapsed. 

To pin a folder, right click on the folder in the project window and click `Pin`. 
To unpin a folder, right click it and select `Unpin`.

>Pinned folders are project-specific and are saved locally on your computer in 
[EditorPrefs][2].

### Preferences

You can configure the following options in in the 
[Preferences](https://docs.unity3d.com/Manual/Preferences.html) window.

| Option |	Description | Default |
|-|-|:-:|
| Collapse Folders | Enables/disables folder auto-collapse | ✅ |
| Auto-expand Pinned | Enables/disables automatic expansion of pinned folders. <br/> _Pinned folders will still not be automatically collapsed if this option is disabled._ | ✅ |


> Springy preferences are saved locally on your computer in [EditorPrefs][2] 
and are shared across all projects.

## Reporting an issue or asking a question

> Before submitting a new issue, please [search for existing issues][5].

If you can't find your issue in the existing issues, please [submit an issue][5].
With your submission, please include information about your environment:

- OS and version (e.g. Windows 10, macOS Big Sur 11.2)
- Unity version (e.g. 2020.3.2f1)

Please try to be as descriptive as possible in your issue.

## Contributing

Springy uses [reflection](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/reflection) 
to access internal Unity editor features, which makes 
it liable to break with newer versions of Unity. We'll try our best to keep it 
up to date, but we're a small team. 

For this reason, individual contributions to Springy are very welcome!
Changes don't just have to be fixes, please feel free to extend Springy with new
features too!

### Forking Springy

1. Click the "Fork" button in the upper right corner of the [repository][3].
2. Create a new Unity project for development. 
3. Clone your forked repository and add it to your new project. 
	- We recommend setting the forked package up as an [embedded package](https://docs.unity3d.com/Manual/CustomPackages.html#EmbedMe).
4. Create a new branch for your change:
	- `git checkout -b <YOUR_BRANCH>`
	- A good name for the branch describes what you are working on (e.g. `fix-unity-2021.1`, `auto-pin-list`, etc.)
5. Please format your commit messages following the [Conventional Commit](#conventional-commits)
standard.

### Testing your changes

Please be sure to test your changes in all [supported editor versions](#requirements). 

Springy does not currently have any tests (hey, that's not a bad idea for a PR).

### Submitting your PR

When you're done testing your changes, please submit your PR to [the Springy repository][3].

Here are some tips for your PR:

- Add a descriptive title
- Link to related issues
- Consider starting with this template:

```md

### Reason for change

Why is this change important?

This is a good place to link to issues this PR addresses:

- Resolves #99997
- Resolves #99999

### Description

What does this change do?

```

## Conventional Commits

This repository follows the [Conventional Commits specification][4]. 
PRs with commit messages that do not follow the specification will be rejected.

### The conventional format

The Conventional Commit specification generally follows this format: 
`type(scope?): description` (scope is optional, hence the `?`). 

Here are some example commit messages for reference:

`feat: add pinned folders`

`fix(reflection): update types for Unity 2021.1`

`perf: improve project window GUI performance`

For more detail, see the [Conventional Commits specification][4].

### Common types

Here is a list of common supported commit types:

- **feat**: A new feature
- **fix**: A bug fix
- **chore**: A change to the project that doesn't affect the build 
	(i.e. a merge commit, version increase, etc)
- **build**: A change that affects the build system or dependencies 
	(i.e. adding/changing package dependencies)
- **perf**: A code change that improves performance
- **style**: Semantic changes that don't affect the meaning of the code
- **refactor**: A code change that neither fixes a bug nor adds a feature

[1]: https://unity3d.com/get-unity/download
[2]: https://docs.unity3d.com/ScriptReference/EditorPrefs.html
[3]: https://github.com/aestheticianlabs/springy
[5]: https://github.com/aestheticianlabs/springy/issues
[4]: https://www.conventionalcommits.org/en/v1.0.0/