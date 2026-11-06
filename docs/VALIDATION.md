# Validation record

Authoring host: Windows, November 6, 2026. Requirements: C# / .NET 8 SDK or newer; Windows, macOS, Linux; modern browser.

The README verification command is `dotnet run -- --self-test`. The companion GitHub Actions workflow describes reproducible build/test checks. Merely including that workflow does not mean it ran successfully. Host-specific verification results are recorded before publication.

Manual acceptance steps are in README.md. GUI behavior, hardware sensors, permissions, audio output, and OS isolation need verification on their target platform; compilation alone cannot establish these properties.

## Authoring result

Passed: dotnet build with zero warnings and zero errors; generation self-test confirms seed determinism, tile count, and differing seeds. Browser exploration and SVG rendering remain manual checks.
