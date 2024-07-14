# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

### Changed

### Removed

## [2.0.11] - 2024-07-13

### Added

- Sample plugin updated, showing how to inject a dalamud mock service

### Fixed

- Plugin startup failure will be logged.

## [2.0.12] - 2024-07-14

### Added

- Dalamud configuration can be overridden

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