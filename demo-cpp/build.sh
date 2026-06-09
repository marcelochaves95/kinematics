#!/usr/bin/env bash
# Build the Kinematics C++ engine + demo scenes to WebAssembly via Emscripten.
# Output: demo-cpp/kinematics.js + kinematics.wasm (loaded by index.html).
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
OUT="$ROOT/demo-cpp/kinematics.js"

# emcc must be on PATH (brew install emscripten, or source emsdk_env.sh).
command -v emcc >/dev/null 2>&1 || { echo "error: emcc not found on PATH"; exit 1; }

EXPORTS='_kn_world_create,_kn_world_destroy,_kn_world_set_limits,_kn_world_set_friction,_kn_world_set_elasticity,_kn_set_max_step,_kn_world_set_drag,_kn_world_apply_impulse,_kn_world_add_rigid_body,_kn_world_add_spring_body,_kn_world_add_pressure_body,_kn_world_add_chain,_kn_world_set_chain_gravity,_kn_world_step,_kn_world_penetration_count,_kn_world_body_count,_kn_world_chain_count,_kn_body_is_static,_kn_body_point_count,_kn_body_get_points,_kn_body_get_position,_kn_body_apply_force,_kn_chain_point_count,_kn_chain_get_points,_kn_demo_softbody,_kn_demo_pressure,_kn_demo_chain,_malloc,_free'

emcc \
    "$ROOT/src/CApi/kinematics_c.cpp" \
    "$ROOT/demo-cpp/demo_scenes.cpp" \
    -std=c++17 -O3 \
    -I"$ROOT/src" \
    -sMODULARIZE=1 \
    -sEXPORT_NAME=createKinematics \
    -sALLOW_MEMORY_GROWTH=1 \
    -sENVIRONMENT=web \
    -sEXPORTED_FUNCTIONS="$EXPORTS" \
    -sEXPORTED_RUNTIME_METHODS=ccall,cwrap,getValue,setValue,HEAPF32 \
    -o "$OUT"

echo "built: $OUT (+ kinematics.wasm)"
