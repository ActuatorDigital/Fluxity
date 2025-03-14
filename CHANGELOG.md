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
- Example GameSession, empty scene with Fluxity setup showing common scenario for game state, current score, and high scores.
- FluxityFlumeRegisterContext and FluxityFeatureContext. Used during Fluxity setup to move this logic out into it's own location.
- `FeatureAggregateObserver`, mechanism for bundling change notification from multiple sources. Replacing that use case previously in `Presenter`.
- Effects can now be set up in a similar way to Feature, Reducer binding.
- Correctly show `decimal` type values in the StoresWindow.
- Added FeatureHandle for when full observer functionality in FeatureObserver is not needed, e.g. just needing feature value.

### Changed

- Effects are now bound via internal mechanism during CreateEffect.
- Simplified Example folder structures.
- Many tests now directly use existing Fluxity type rather than a dummy.
- Feature<TState>s are no longer added to the ServiceInstaller. To get direct access to a feature either request the `Store` and `GetFeatureView` or use a `FeatureObserver`.
- Renamed Initialise to CreateEffects in the FluxityInitialiser, as that is it's actual use in practice.
- FluxityInitialiser is now also the ServiceInstaller
- FluxityInitialiser methods renamed, RegisterFluxity and RegisterServices
- Dispatcher now supports dispatching via base ICommand type
- Dispatcher now queues, allowing nested dispatching.

### Removed

- EffectT, replaced by internal binding done by EffectBinding.
- IFeaturePresenterBinding replaced with IFeatureObserver and FeatureObserver
- DispatchingPresenter, not being used.
- StyleCop package, was not being used.
- Presenter, had multiple responsibilities enforced a pattern of usage that was oft ill-fitting.
- FluxityFlumeRegisterContext and ServiceInstallerExt, the FluxityInitialiser handles these more directly now.

## [0.1.0]

### Added

- Initial public release
