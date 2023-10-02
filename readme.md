# [WIP] asl-help v2

This is the work-in-progress repository of the next iteration of [`asl-help`](https://github.com/just-ero/asl-help).

## Contributing

***Please understand that this is a WORK IN PROGRESS codebase. Your contributions WILL be squashed (credit will obviously still be given in the appropriate places)!***

To contribute, make sure you have the following installed:
* .NET 8.0 SDK (`winget install Microsoft.DotNet.SDK.Preview`)
* .NET Framework 4.8.1 SDK (`winget install Microsoft.DotNet.Framework.DeveloperPack_4`)

After cloning the repository, create a folder named `lib` in it. Place the below files in it. You can find them your `~/LiveSplit` and `~/LiveSplit/Components` folders:
* Irony.dll
* LiveSplit.Core.dll
* LiveSplit.ScriptableAutoSplit.dll
* LiveSplit.Text.dll
* LiveSplit.View.dll
* UpdateManager.dll

To build, simply run `dotnet build src\AslHelp.Build`.  
You can add the option `-c <Debug|Release>` to choose which configuration you would like to build.  
The `Debug` configuration also adds the ability to choose the LiveSplit directory that the built `asl-help` binary should be copied into: `-p:LsDir="C:\Path\To\LiveSplit"`. *Note: this **replaces** the file without asking!*

When your project builds, open a pull request with your changes.

## To-Do

- Do we care about injection (for Unity for example)?
  - Makes finding things a lot easier, but we have to do the external version anyway so
- Struct parser doesn't work man
  - [x] Need to replace before parsing, obviously
  - Need to check whether parent stuff works
- Unit tests
- Benchmarks?
  - Compare against LiveSplit.Core?
- Documentation & tutorials
- Overhaul `Pointer` classes
  - Maybe change from `Make` to `Create`?
  - Allow children to be created from span-like `Pointer`s by accessing one specific index
    - How the FUCK do we do this
- Create common interfaces for interacting with game engines
  - Include a way to dump all available information
    - Create common interfaces for formatting this information
- Mono-related features
  - Figure out how to infer `cattrs` without having to look at memory
  - Improve `Make` abilities
    - Do not require adding inheritance depth, if possible
    - Make it work with IL2CPP
  - Special Unity-related features
    - Improve `SceneManager`
    - `TimeManager`, `PlayerSettings`, `InputManager`
    - Add ability to access `Components` on `Transforms` in the current scene
      - This would mark Unity games 100% solved
  - What About IL2CPP?
    - IL2CPP structs change incredibly frequently without increasing the major version
    - This causes big headaches dealing with when they change and in what ways
    - Possibly the only way to deal with this is to just bite the bullet and compare the game's Unity version
- Unreal-related features
  - Figure out Subsystems
    - It's a TMap in `UGameInstance` (`SubsystemCollection`) (no clue how to find it, it's not a `UPROPERTY`)
  - Figure out Singletons/static anything
  - Figure out UE3 (apple1417 will do it)
- GameMaker-related features
  - Figure out everything
- Emulator-related features
  - Jujstme will do it
- RE Engine-related features
  - ???
- Clickteam Fusion (2.5)-related features
  - ???
