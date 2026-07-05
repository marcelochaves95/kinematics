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

KN_API KnWorld* kn_world_create(void);
KN_API void     kn_world_destroy(KnWorld* world);

KN_API void kn_world_set_limits(KnWorld* world, float minX, float minY, float maxX, float maxY);

KN_API void kn_world_set_friction(KnWorld* world, float friction);
KN_API void kn_world_set_elasticity(KnWorld* world, float elasticity);
KN_API void kn_set_max_step(float maxStep);

KN_API void kn_world_set_drag(KnWorld* world, float drag);

KN_API void kn_world_apply_impulse(KnWorld* world, float wx, float wy,
                                   float radius, float strength);

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

KN_API int kn_world_add_chain(KnWorld* world, float ax, float ay, float bx, float by,
                              int count, float k, float damping, float mass);

KN_API void kn_world_set_chain_gravity(KnWorld* world, float gx, float gy);

KN_API void kn_world_step(KnWorld* world, double dt, int substeps);

KN_API int kn_world_penetration_count(KnWorld* world);

KN_API int kn_world_body_count(KnWorld* world);
KN_API int kn_world_chain_count(KnWorld* world);
KN_API int kn_body_is_static(KnWorld* world, int body);

KN_API int  kn_body_point_count(KnWorld* world, int body);

KN_API int  kn_body_get_points(KnWorld* world, int body, float* outXY, int maxPoints);
KN_API void kn_body_get_position(KnWorld* world, int body, float* outX, float* outY);
KN_API void kn_body_apply_force(KnWorld* world, int body, float px, float py, float fx, float fy);

KN_API int kn_chain_point_count(KnWorld* world, int chain);
KN_API int kn_chain_get_points(KnWorld* world, int chain, float* outXY, int maxPoints);

#ifdef __cplusplus
}
#endif

#endif
