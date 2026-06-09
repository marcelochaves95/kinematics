# Kinematics — C++ / WebAssembly demo

The same 2D physics lab as `demo/` (Blazor), but running entirely on the **C++**
engine compiled to WebAssembly. All physics *and* scene construction live in C++
(`src/` + `demo_scenes.cpp`); `app.js` is just a thin shell that renders to canvas
and forwards click/tap input as a radial impulse.

## Build

Requires [Emscripten](https://emscripten.org) (`emcc` on PATH — `brew install emscripten`).

```bash
./build.sh        # -> kinematics.js + kinematics.wasm  (~52 KB wasm)
```

## Run

Serve the folder over HTTP (WASM can't load from `file://`):

```bash
node serve.mjs    # http://localhost:8100
# or: python3 -m http.server 8100
```

## How it maps to the C++ engine

- `kn_demo_softbody/pressure/chain(world)` (in `demo_scenes.cpp`) build each scene
  — arena walls, body/chain, tuning, and the chain's 300-frame pre-simulation —
  a faithful C++ port of `demo/Scenes/*.cs`.
- `kn_world_step(world, dt, substeps)` runs the engine with 4× substepping and
  applies per-frame chain gravity / drag (the C# scenes' `PreUpdate`).
- `kn_world_apply_impulse(...)` is the click-to-push interaction.
- Geometry is read back in batch via `kn_body_get_points` / `kn_chain_get_points`.

Everything goes through the C ABI in `src/CApi/kinematics_c.h` — the exact same
surface any other language (Python, Lua, Godot, Rust, …) would use.
