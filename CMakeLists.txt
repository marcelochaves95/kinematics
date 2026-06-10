cmake_minimum_required(VERSION 3.15)
project(kinematics_cpp LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

if(NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE Release)
endif()

# Recover cross-TU inlining lost by the .h/.cpp split (the engine is split, not
# header-only). IPO/LTO is enabled for Release only.
set(CMAKE_INTERPROCEDURAL_OPTIMIZATION_RELEASE ON)

# ── Engine library (static; headers in src/, included as <Math/Vector2.h>) ──
add_library(kinematics STATIC
    src/Math/Mathf.cpp
    src/Math/Vector2.cpp
    src/Math/Vector3.cpp
    src/Collision/AABB.cpp
    src/Collision/PointMass.cpp
    src/Collision/Shape.cpp
    src/Collision/Spring.cpp
    src/Collision/Chain.cpp
    src/Collision/CollisionInfo.cpp
    src/Collision/Collision.cpp
    src/Dynamics/Body.cpp
    src/Dynamics/SpringBody.cpp
    src/Dynamics/PressureBody.cpp
    src/Utils/Bitmask.cpp
    src/Utils/KinematicsController.cpp)
target_include_directories(kinematics PUBLIC
    "${CMAKE_CURRENT_SOURCE_DIR}/src")
target_compile_features(kinematics PUBLIC cxx_std_17)

# ── C ABI shared library (the language-agnostic surface) ────────
# Other languages (C#, Python, Lua, Godot, Rust, WASM) link against this.
add_library(kinematics_c SHARED src/kinematics_c.cpp)
target_link_libraries(kinematics_c PRIVATE kinematics)
set_target_properties(kinematics_c PROPERTIES
    CXX_VISIBILITY_PRESET hidden
    VISIBILITY_INLINES_HIDDEN ON)

# ── Tests ───────────────────────────────────────────────────────
enable_testing()
add_executable(kinematics_tests
    tests/test_main.cpp
    tests/test_math.cpp
    tests/test_collision.cpp
    tests/test_chain.cpp
    tests/test_dynamics.cpp
    tests/test_controller.cpp
    tests/test_capi.cpp
    tests/test_umbrella.cpp
    src/kinematics_c.cpp)
target_link_libraries(kinematics_tests PRIVATE kinematics)
target_include_directories(kinematics_tests PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/tests")

if(MSVC)
    target_compile_options(kinematics_tests PRIVATE /W4 /WX)
else()
    target_compile_options(kinematics_tests PRIVATE -Wall -Wextra -Werror)
endif()

add_test(NAME unit COMMAND kinematics_tests)
