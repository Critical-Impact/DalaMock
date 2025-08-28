# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

### Changed

### Removed

## [3.0.6] - 2025-08-28

### Added
- Added RegisterTransientSelf to ContainerBuilderExtensions


## [3.0.5] - 2025-08-26

### Fixed
- A very important typo

## [3.0.4] - 2025-08-26

### Changed
- The MockContainer now outputs more logging when EXD_DATA_DIR is specified along with outputting the game path when booting

### Fixed
- Correct BC7 handling as Lumina has now fixed the issue

## [3.0.3] - 2025-08-21

### Added
- HostedPlugin now supports IHostedService registration along with a facility to replace those services when mocking/unit testing

## [3.0.2] - 2025-08-13

### Added
- Added EXD_DATA_DIR environment variable, allowing for the exd path to be provided for CI
- Added DALAMOCK_SAVE_DIR environment variable
- DalaMock can be configured to not spawn a window and provide a null texture provider and ui builder allowing for use in CI

## [3.0.1] - 2025-08-09

### Changed
- Revert ChatLinkHandler updates
- Add DtrBar OnClick
- Handle BC/BC7/DXT pixel formats. This currently relies on an unreleased Lumina PR to support BC5/BC7.

## [3.0.0] - 2025-08-06

### Changed
- Initial support for API13

## [2.3.1] - 2025-06-17

### Added
- Added DalaMock.PluginTemplate allowing you to boostrap a hosted/di/mock driven plugin quickly

## [2.3.0] - 2025-06-16

### Added
- Improvements to HostedPlugin including
  - Will now emit events when built, starting, stopping and stopped
  - Can now be configured to register a MediatorService for you
  - HostingAwareService added which will provide automatic registration to plugin events

### Changed

- Updated dependencies
- The game path and plugin path will be automatically resolved if not provided
- MockDalamudPluginInterface will now return the real manifest if available
- DalaMock.Sample is now more opinionated

## [2.2.8] - 2025-05-29

### Changed

- DalaMock.Host now injects a dalamud ILogger provider
- Improvements to how DalaMock handles keyboard input from SDL2


## [2.2.7] - 2025-05-11

### Changed

- Add stub for ITextureProvider.CreateDrawListTexture and ITextureProvider.CreateFromClipboardAsync

## [2.2.6] - 2025-05-03

### Fixed

- MockWindowSystem now implements it's own draw logic instead of inheriting from DalamudWindowSystem allowing it to avoid any breakages occured by changes to that class.

## [2.2.5] - 2025-05-01

### Fixed

- Fixed slow dispose on MediatorService

## [2.2.4] - 2025-04-22

### Changed

- Updated all nuget packages
- Updated included cimgui so/dll files

## [2.2.3] - 2025-04-06

### Changed

- DalaMock.Host
    - Stop MediatorService from accepting empty message lists

## [2.2.2] - 2025-04-03

### Changed

- DalaMock.Host
  - Use semaphore for MediatorService to make message queue efficient

## [2.2.1] - 2025-03-29

### Added

- Bump for latest dalamud patch


## [2.2.0] - 2025-03-26

### Added

- Initial support for API12/7.2


## [2.1.7] - 2024-12-30

### Added

- ImGui asserts will be caught and logged

## [2.1.6] - 2024-11-23

### Fixed

- Use correct dalamud branch


## [2.1.5] - 2024-11-23

### Fixed

- Implement missing IChatGui methods

## [2.1.4] - 2024-11-18

### Fixed

- Use dalamud Serilog when possible


## [2.1.3] - 2024-11-18

### Fixed

- Use same nuget packages that dalamud does

## [2.1.2] - 2024-11-16

### Fixed

- Make GameData single instance

## [2.1.1] - 2024-11-13

### Fixed

- Add/update mocks for API11

## [2.1.0-alpha] - 2024-10-21

### Fixed

- Initial support for API11

## [2.0.28] - 2024-09-30

### Fixed

- Added missing ICallGateProvider property

## [2.0.27] - 2024-09-26

### Added

- Use MS logging for mocks

## [2.0.26] - 2024-08-20

### Added

- Added GetFileAsync to MockDataManager

## [2.0.25] - 2024-08-04

### Added

- Added IDtrBar mock service

### Fixed

- Use Microsoft.Extensions.Logging internally in combination with serilog
- Updated Serilog to latest stable release

## [2.0.24] - 2024-08-04

### Fixed

- Provide the correct directory for saving plugin configs

## [2.0.23] - 2024-08-01

### Fixed

- Provide the correct assembly name to the mock plugin interface


## [2.0.22] - 2024-07-28

### Fixed

- Support latest dalamud release
- Use staging zip for release

## [2.0.21] - 2024-07-28

### Fixed

- Support latest dalamud release

## [2.0.20] - 2024-07-28

### Added

- Have sample actually load/save a configuration

### Fixed

- Fix incorrect save path


## [2.0.19] - 2024-07-28

### Added

- DalaMock will ask you for a sqpack directory and plugin config directory if none are provided or can be loaded from the DalaMock configuration file

### Fixed
- Plugin config files/directories should match dalamuds layout

## [2.0.18] - 2024-07-28

### Added

- Added missing properties/methods to dalamud mock services
- Merged MockTextureProvider and MockTextureManager
- Assembly Location can be set via plugin load settings (Styr1x)
- Device and WindowHandlePtr access from UiBuilder (Styr1x)
- CreateFromRaw implementation for TextureProvider (Styr1x)
- Dispose for MockTextureMap (Styr1x)


## [2.0.17] - 2024-07-19

### Added

- Added missing method for MockObjectTable


## [2.0.16] - 2024-07-16

### Added

- Implemented most of MockChatGui

## [2.0.15] - 2024-07-15

### Added

- Add rudimentary font loading to support IFonts FontAwesome font

## [2.0.14] - 2024-07-14

### Added

- MockContainer can accept a list of replacement dalamud services

## [2.0.13] - 2024-07-14

### Added

- MockContainer can have its services overriden before being built

## [2.0.12] - 2024-07-14

### Added

- Dalamud configuration can be overridden

## [2.0.11] - 2024-07-13

### Added

- Sample plugin updated, showing how to inject a dalamud mock service

### Fixed

- Plugin startup failure will be logged.

## [2.0.10] - 2024-07-11

### Added

- Added IFileDialogManager, a wrapper for dalamuds file dialog manager, made to avoid font crashes
- DalaMock will now save and load a global game path and plugin config path

### Fixed

- Block plugin loading when paths are invalid and stop paths being edit if plugin is running

## [2.0.9] - 2024-07-11

### Added

### Fixed

### Changed

### Removed

## [2.0.8] - 2024-07-11

### Added

### Fixed

- Reworked parts of MockDalamudPluginInterface and allowed statics to be injected

### Changed

### Removed


## [2.0.7] - 2024-07-10

### Added

- Implemented Create and Inject for MockDalamudPluginInterface
- Added a changelog ;)

### Fixed

### Changed

### Removed