/* C ABI for the Kinematics physics engine.
 *
 * This is the stable, language-agnostic surface: any host that can call C
 * functions (C#, Python ctypes, Lua FFI, JNI, GDExtension, Swift, Rust, WASM)
 * can drive the engine through these functions. Objects are referenced by
 * integer handles; geometry is read back in batch (fill-an-array) to keep the
 * number of FFI boundary crossings low.
 *
 * Handles are indices that stay valid for the lifetime of the world as long as
 * you do not remove objects (this surface is add-only by design).
 */
#ifndef KINEMATICS_C_H
#define KINEMATICS_C_H

#if defined(_WIN32)
#  if defined(KN_BUILD)
#    define KN_API __declspec(dllexport)
#  else
#    define KN_API __declspec(dllimport)
#  endif
#else
#  define KN_API __attribute__((visibility("default")))
#endif

#ifdef __cplusplus
extern "C" {
#endif

typedef struct KnWorld KnWorld;

/* ── World lifecycle ──────────────────────────────────────────── */
KN_API KnWorld* kn_world_create(void);
KN_API void     kn_world_destroy(KnWorld* world);

/* Optional: set world bounds explicitly. If never called, the world derives
 * its bounds from the static bodies on the first step (like the C# engine). */
KN_API void kn_world_set_limits(KnWorld* world, float minX, float minY, float maxX, float maxY);

/* Tuning knobs. */
KN_API void kn_world_set_friction(KnWorld* world, float friction);
KN_API void kn_world_set_elasticity(KnWorld* world, float elasticity);
KN_API void kn_set_max_step(float maxStep); /* global anti-tunneling cap */

/* ── Bodies (handle >= 0 on success, -1 on failure) ───────────────
 * `xy` is a flat array of `count` (x,y) pairs describing the local polygon
 * (it is centered on its centroid). `isStatic` != 0 makes an immovable body
 * (pass mass = INFINITY for true static colliders). */
KN_API int kn_world_add_rigid_body(KnWorld* world, const float* xy, int count,
                                   float mass, float posX, float posY,
                                   float gravX, float gravY, int isStatic);

KN_API int kn_world_add_spring_body(KnWorld* world, const float* xy, int count,
                                    float mass, float edgeK, float edgeDamp,
                                    float shapeK, float shapeDamp,
                                    float posX, float posY,
                                    float gravX, float gravY, int isStatic);

KN_API int kn_world_add_pressure_body(KnWorld* world, const float* xy, int count,
                                      float mass, float gasPressure,
                                      float edgeK, float edgeDamp,
                                      float shapeK, float shapeDamp,
                                      float posX, float posY,
                                      float gravX, float gravY, int isStatic);

/* ── Chains ───────────────────────────────────────────────────── */
KN_API int kn_world_add_chain(KnWorld* world, float ax, float ay, float bx, float by,
                              int count, float k, float damping, float mass);

/* Gravity applied to chain interior masses each (sub)step (chains have no
 * built-in gravity, unlike bodies which carry their own). */
KN_API void kn_world_set_chain_gravity(KnWorld* world, float gx, float gy);

/* ── Simulation ───────────────────────────────────────────────── */
/* Advances by `dt`, internally split into `substeps` (>=1) smaller steps to
 * reduce tunneling, exactly like the demo's 4x substepping. */
KN_API void kn_world_step(KnWorld* world, double dt, int substeps);

KN_API int kn_world_penetration_count(KnWorld* world);

/* ── Read-back (batch) ────────────────────────────────────────── */
KN_API int  kn_body_point_count(KnWorld* world, int body);
/* Fills up to `maxPoints` (x,y) pairs of the body's deformed outline into
 * `outXY` (length >= 2*maxPoints). Returns the number of points written. */
KN_API int  kn_body_get_points(KnWorld* world, int body, float* outXY, int maxPoints);
KN_API void kn_body_get_position(KnWorld* world, int body, float* outX, float* outY);
KN_API void kn_body_apply_force(KnWorld* world, int body, float px, float py, float fx, float fy);

KN_API int kn_chain_point_count(KnWorld* world, int chain);
KN_API int kn_chain_get_points(KnWorld* world, int chain, float* outXY, int maxPoints);

#ifdef __cplusplus
} /* extern "C" */
#endif

#endif /* KINEMATICS_C_H */
