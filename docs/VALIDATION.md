# Validation record

Authoring host: Windows, November 6, 2026. Requirements: C# / .NET 8 SDK or newer; Windows, macOS, Linux; modern browser.

The README verification command is `dotnet run -- --self-test`. The companion GitHub Actions workflow describes reproducible build/test checks. Merely including that workflow does not mean it ran successfully. Host-specific verification results are recorded before publication.

Manual acceptance steps are in README.md. GUI behavior, hardware sensors, permissions, audio output, and OS isolation need verification on their target platform; compilation alone cannot establish these properties.

## Authoring result

Passed: dotnet build with zero warnings and zero errors; generation self-test confirms seed determinism, tile count, and differing seeds. Browser exploration and SVG rendering remain manual checks.

## Observed GitHub Actions result

Passed on November 6, 2026: [build/check run](https://github.com/netcodermike/procedural-planet-explorer/actions/runs/42196084132).

Verified source commit: `3ca1de3ec9213f29a1f61aaa45bd6091a82e3840`.

C# build, deterministic generation checks, and SVG export.

This record may be committed after the linked source-validation run; the linked SHA identifies exactly what was checked. Native GUI/hardware acceptance remains manual as described in README.md.
