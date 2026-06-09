cmake_minimum_required(VERSION 3.15)
project(kinematics_cpp LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

if(NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE Release)
endif()

# ── Engine library ──────────────────────────────────────────────
# C++ sources live in src/ alongside the C# files, in the same folders
# (e.g. src/Math/Vector2.hpp next to src/Math/Vector2.cs). The include root is
# src/, so headers are included as "Math/Vector2.hpp". The two toolchains ignore
# each other's files (.cs vs .hpp/.cpp) during the migration.
#
# Phase 1 (Math) is header-only, so this is an INTERFACE target for now.
# It becomes a STATIC library once Phase 2+ adds .cpp translation units
# (change INTERFACE -> STATIC and list the sources).
add_library(kinematics INTERFACE)
target_include_directories(kinematics INTERFACE
    "${CMAKE_CURRENT_SOURCE_DIR}/src")
target_compile_features(kinematics INTERFACE cxx_std_17)

# ── Tests ───────────────────────────────────────────────────────
enable_testing()
add_executable(kinematics_tests
    tests/test_main.cpp
    tests/test_math.cpp)
target_link_libraries(kinematics_tests PRIVATE kinematics)
target_include_directories(kinematics_tests PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/tests")

if(MSVC)
    target_compile_options(kinematics_tests PRIVATE /W4 /WX)
else()
    target_compile_options(kinematics_tests PRIVATE -Wall -Wextra -Werror)
endif()

add_test(NAME math COMMAND kinematics_tests)
