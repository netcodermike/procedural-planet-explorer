# Procedural Planet Explorer

Generate a deterministic 2D planet from a numeric seed, explore biome sectors, inspect weather and life, and export an SVG terrain map.

**Language:** C# / .NET 8 SDK or newer  
**Platform:** Windows, macOS, Linux; modern browser  
**Status:** First-release learning project. See implemented scope and limitations below; this is not a production-completeness claim.

## Features

- 72 × 36 sector maps driven by seeded trigonometric terrain.
- Ocean, ice, mountain, desert, and forest biomes.
- Clickable map with elevation, weather, and native-life descriptions.
- Scalable SVG export with per-sector titles.
- Same seed produces the same map within the same runtime implementation.

## Requirements and quick start

Clone the repository, enter its folder, and install the language/platform tools listed above. Commands run from the repository root. No credentials are required except for Backup Vault's explicitly generated secrets.

```sh
git clone https://github.com/netcodermike/procedural-planet-explorer.git
cd procedural-planet-explorer
dotnet run -- --seed 42
# Open http://localhost:5080
dotnet run -- --seed 99 --export planet.svg
```

## Verification

```sh
dotnet run -- --self-test
```

Run the self-test, export seed 42 twice, and compare the files. Export seed 43 and confirm it differs. Start the UI and inspect ocean, forest, and ice tiles. Request an out-of-range tile (`x=999`) and verify HTTP 400.

See [validation notes](docs/VALIDATION.md) for what was actually executed during authoring. A committed CI workflow is a future check until its run is observed successful.

## Architecture and data

`Program.cs` includes the `Tile` record, pure `Generate` and `Svg` functions, and an `HttpListener` server. Terrain combines longitudinal and latitudinal waves; temperature and elevation determine biomes. `GET /planet.svg` returns the map, and `GET /tile?x=0&y=0` returns sector details. The root page converts map clicks into tile coordinates. The server listens on localhost:5080 and has no external dependencies.

## Limitations and next steps

This is a 2D exploration prototype, with descriptive weather and life rather than a simulated ecosystem. No 3D navigation, saved journeys, multiplayer, or dynamic weather. The HTTP server is synchronous and intended for local use. Cross-version `System.Random` implementation changes may affect seed reproducibility. Terrain uses an equirectangular map rather than a geodesic sphere.

## Troubleshooting

Install the .NET 8 SDK/runtime if the target framework cannot run. If port 5080 is occupied, stop the other listener or change the prefix in `Program.cs`. On systems with URL ACL restrictions, use the SVG export mode or configure a localhost URL reservation. `--seed` requires an integer; `--export` requires an output path. Stop with Ctrl+C.

## Development

Read the source entry point described above, make a focused change, run the verification command, and exercise the relevant manual scenario. [Contributing](CONTRIBUTING.md) explains bug reports and pull requests; [security notes](SECURITY.md) describe the trust assumptions. Runtime/generated files are ignored. No telemetry or paid API dependencies are included.

## License

[MIT](LICENSE), copyright 2026 netcodermike.
