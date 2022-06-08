# HP Roller

:heart::game_die: An  HP dice roller for D&D 5e

## Prerequisites
- :factory: .NET Core 3.1 SDK
- :computer: .NET CLI

## Installation
1. :arrow_down_small: Download the repo
1. `dotnet publish`
1. Add `${PROFILE}\bin\HP_Roller` to your PATH
1. Try it out!
    - `hrp -h` for help
    - `hpr {#} {#} {#}d{#}+{#}`, for example `hpr 2 4 3d4+3`, gives you:
        ```
        Monster # | Hp Rolled | Initiative
        ----------------------------------
        ----------------------------------
            1     |   14      |    18
        ----------------------------------
            2     |   14      |    12
        ----------------------------------
        ```