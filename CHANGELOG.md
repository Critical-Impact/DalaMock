# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

### Changed

### Removed

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