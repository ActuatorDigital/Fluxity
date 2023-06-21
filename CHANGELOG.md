# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Ability to set starting State of a feature, during `RegisterFeature`.
- ObjectData sample, uses reference type in its state and is initialised during `RegisterFeature`.
- Fluxity->Stores editor window. View the values in stores at runtime.
- StoresWindow example, has a number of sample cases (Features) for the Stores window to demonstrate.
- Simple Runtime History window, shows most recent commands dispatched by Dispatcher.
- Simple Runtime Binding window, shows the currently bound reducers and effects in the system.
- Add simple case insensitive search filter to editor windows
- Add details of selected command in History Window, similar to that found in the Stores Window.
- Add Fluxity user preferences, allows toggling on and off logging of command history to Logs folder and the history limit.

### Changed

- Effects are now bound via internal mechanism during CreateEffect.

### Removed

- EffectT, replaced by internal binding done by EffectBinding.

## [0.1.0]

### Added

- Initial public release
