# Kinematics

A lightweight 2D soft-body physics engine in **C++** (header-only core, C++17,
no external dependencies).

## How it works

Bodies are made of **PointMasses** connected by **Springs**. Each frame, spring
forces and gravity are applied to the point masses, which are then integrated
with semi-implicit Euler.

Three body types are available:

| Type | Description |
|------|-------------|
| `Body` | Rigid body (no internal springs) |
| `SpringBody` | Soft body with edge and shape springs |
| `PressureBody` | Soft body with internal gas pressure |

## Coordinate system

Screen-space: **Y increases downward**. Gravity points in the `+Y` direction.

## Layout

```
src/            header-only engine (Math/, Collision/, Dynamics/, Utils/)
                + the C ABI (kinematics_c.h / .cpp)
tests/          unit tests (zero-dependency harness)
demo/           WebAssembly demo (Emscripten) — see demo/README.md
CMakeLists.txt  build for the C ABI shared library + tests
```

## Usage (C++)

```cpp
#include "Collision/Shape.hpp"
#include "Dynamics/SpringBody.hpp"
#include "Utils/KinematicsController.hpp"
using namespace kinematics;

Shape shape;
shape.Begin(true);
shape.Add(Vector2(-0.5f, -0.5f));
shape.Add(Vector2( 0.5f, -0.5f));
shape.Add(Vector2( 0.5f,  0.5f));
shape.Add(Vector2(-0.5f,  0.5f));
shape.End();

auto body = std::make_shared<SpringBody>(shape, /*mass*/ 1.0f,
    /*edgeK*/ 150.0f, /*edgeDamp*/ 10.0f,
    /*shapeK*/ 200.0f, /*shapeDamp*/ 15.0f);
body->Position = Vector2(0.0f, 0.0f);
body->Gravity  = Vector2(0.0f, 9.8f);

KinematicsController controller;
controller.Add(body);

// game loop
controller.Update(elapsed);
```

## Using from other languages

The engine ships a stable **C ABI** (`src/kinematics_c.h`) built into a shared
library, so any host that can call C functions (Python `ctypes`, Lua FFI, JNI,
Godot GDExtension, Rust, WebAssembly, …) can drive it through integer handles and
batch read-back. See `demo/` for a WebAssembly example.

## Build & test

```bash
# tests (needs a C++17 compiler)
c++ -std=c++17 -Isrc -Itests tests/*.cpp src/kinematics_c.cpp -o /tmp/kn_tests && /tmp/kn_tests

# or via CMake (builds the C ABI shared library + tests)
cmake -S . -B build && cmake --build build && ctest --test-dir build
```
