# [WIP] asl-help v2

This is the work-in-progress repository of the next iteration of [`asl-help`](https://github.com/just-ero/asl-help).

## To-Do

- Overhaul `Pointer` classes
  - Maybe change from `Make` to `Create`?
  - Allow children to be created from span-like `Pointer`s by accessing one specific index

- Create common interfaces for interacting with game engines
  - Include a way to dump all available information
    - Create common interfaces for formatting this information

- Mono-related features
  - Figure out how to infer `cattrs` without having to look at memory
  - Improve `Make` abilities
    - Do not require adding inheritance depth, if possible
    - Make it work with IL2CPP
  - Special Unity-related features
    - Improved `SceneManager`
    - `TimeManager`, `PlayerSettings`, `InputManager`
    - Add ability to access `Components` on `Transforms` in the current scene
      - This would mark Unity games 100% solved
  - What About IL2CPP?
    - IL2CPP structs change incredibly frequently without increasing the major version
    - This causes big headaches dealing with when they change and in what ways
    - Possibly the only way to deal with this is to just bite the bullet and compare the game's Unity version

- Unreal-related features
  - Figure out Subsystems
  - Figure out Singletons/static anything

- GameMaker-related features
  - Figure out everything

- Emulator-related features
  - Jujstme will do it
