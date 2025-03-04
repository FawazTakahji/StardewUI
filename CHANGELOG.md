# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.3.0] - 2024-11-10

### Added

- [Duck typing conversions](https://focustense.github.io/StardewUI/framework/duck-typing/) for bound properties: internal types like `Sprite` can now be bound without an assembly reference by creating an equivalent type on the caller's side.
- Add-on system, AKA [framework extensions](https://focustense.github.io/StardewUI/framework/extensions/): Create and register custom widgets and type converters that can be included in StarML.
- Translation ("i18n") bindings using the syntax: `attr={#TranslationKey}` for keys in the same mod or `attr={#authorname.ModName:TranslationKey}` for keys in a different mod.
- [Tab widget](https://focustense.github.io/StardewUI/library/standard-views/#tab) to use for top or side navigation.

### Changed

- Improved hot reloading: files can now be edited in the source directory instead of deployed mod directory using the [source sync](https://focustense.github.io/StardewUI/getting-started/hot-reload/#source-sync) feature.
- Layout strings (i.e. in the `layout` attribute) can now specify min and max values using a range similar to C# syntax.
  - For example `layout="50%[600..1200] content[800..]"` specifies a width of 50% constrained between 600px and 1200px and a height that grows with content but has a minimum of 800px.
- View bindings can now dispatch [update ticks](https://focustense.github.io/StardewUI/framework/binding-context/#update-ticks) to bound context (models).
- [Label widget](https://focustense.github.io/StardewUI/library/standard-views/#label) now supports text shadows.
- Text Input widget now only captures on left-click, to allow right-clicks to be used for other behavior such as clearing the text.
- Expanded and improved warnings emitted to SMAPI console for bindings that can't receive updates. Warnings were already shown for types not implementing `INotifyPropertyChanged` but will now also be shown for fields and auto-properties regardless of the declaring type.
- Renamed `Frame` debug class to `TraceFrame` to avoid ambiguity with `Frame` view.

### Fixed

- Button, Wheel and Focus Search events now properly respect the UI scale when set to a value other than 100% or when running in split-screen mode.
- Final row of a `Grid` will no longer be misaligned under specific layout conditions.
- Input events now trigger a forced "re-hover" on menus so that tooltips update and pointer events (e.g. `pointer-enter` and `pointer-leave`) fire when content is scrolled but the actual mouse position has not changed.
- Improved bounds-detection when overlays are combined with floating elements; should avoid premature closure in most cases.

## [0.2.5] - 2024-10-30

### Added

- General button-press event for all views, able to handle any `SButton` as opposed to just mouse-click equivalents. Mainly intended for tabbing/paging implementations built using StarML instead of `IPageable`/`ITabbable`, which will probably be deprecated in a future version.
- View outlets and the `*outlet` attribute for widgets such as the Expander with multiple distinct content areas.
- Value converters for nullable types; support all combinations of nullable and non-nullable for the source and destination.
- Filled out most remaining documentation pages. There are still a few stubs, mostly having to do with as-yet unimplemented features assigned to [Milestone 3](https://github.com/focustense/StardewUI/milestone/3).
- Added XML-to-Markdown doc generator, and generated reference docs for entire API.

### Changed

- Refactored `WrapperView<T>` into `ComponentView<T>` and base class `DecoratorView<T>` to promote reuse for other wrapper types, including the StarML `DocumentView`.
- `DecoratorView<T>` now exposes the inner/root view as a child view, instead of passing through it, so that the inner view can be found by `ViewMenu` and other hierarchy-sensitive areas needing the exact view for tooltips, scrolling, event dispatch, etc.
- `Lane` no longer returns early when it runs out of space for child layout; this is done to prevent any child views from continuing to remain dirty and repeatedly re-triggering layout on every frame.
- `TextInput` chooses useful default sprites, colors and dimensions so that it can be used from StarML without having to specify every attribute explicitly.
- Updated all Core namespaces to reflect the actual path structure. This is a one-time change but will likely break any users that were previously using a source dependency (shared project).

### Fixed

- Use a constant caret size and correct rotation in Expander widget.
- Keybind-related views propagate `SpriteMap` updates immediately; required to see any sprites when created from StarML.

## [0.2.0] - 2024-10-19

## Added

- More predefined sprites, such as left/right navigation arrows.
- Scale setting for `Label` control, to simulate font sizes.
- Enable binding to fields, as opposed to properties only.
- One-time bindings, which prevent update checks and also suppress the `INotifyPropertyChanged` warning.
- Speedscope-compatible performance tracing (default hotkey: <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>F6</kbd>).
- Several more complete documentation pages, including full examples.

### Changed

- Failed asset loading, parsing, binding and other UI-pipeline operations now use throttled retries with exponential backoff to avoid stealing all frames and spamming the error log in the case of malformed or invalid markup.
- Event bindings now support `bool` return values, and set the `Handled` property of a bubbled event accordingly.
- Log warnings when binding to properties of an object that does not implement `INotifyPropertyChanged`.
- `<dropdown>` element now uses a new type, `DynamicDropDownList`, which silently adapts the real `DropDownList<T>` to whichever option type `T` the view model appears to be using.
- Moved constructor arguments (e.g. sprites, or sprite maps) on all standard views to regular properties, so they can be set from StarML. Default sprites will still be used if properties are not set.

### Fixed

- Cached event bindings are no longer (incorrectly) reused across separate view instances; also eliminates a possible memory leak.
- Individual attribute bindings should handle `null` context safely.

## [0.1.0] - 2024-10-12

### Added

- Initial release of framework.
- StarML grammar, asset loader and menu view.
- Initial documentation site based on material-mkdocs; many pages are stubs.
- Syntax highlighting for Visual Studio, VSCode and Notepad++.

[Unreleased]: https://github.com/focustense/StardewUI/compare/v0.3.0...HEAD
[0.3.0]: https://github.com/focustense/StardewUI/compare/v0.2.5...v0.3.0
[0.2.5]: https://github.com/focustense/StardewUI/compare/v0.2.0...v0.2.5
[0.2.0]: https://github.com/focustense/StardewUI/compare/v0.1.0...v0.2.0
[0.1.0]: https://github.com/focustense/StardewUI/tree/v0.1.0
