using Kinematics.MathModule;

using UnityEditor;

using UnityEngine;

using Vector3 = Kinematics.MathModule.Vector3;

namespace Kinematics.CollisionModule
{
    [RequireComponent(typeof(RigidBody))]
    [AddComponentMenu("PhysicsEngine/CollisionModule/PhysicsMaterial")]
    public class PhysicsMaterial : MonoBehaviour
    {
        [SerializeField] private float ue = 0.3f;
        [SerializeField] private float uc = 0.3f;

        private MathModule.Vector3 fe;
        private MathModule.Vector3 fc;

        public MathModule.Vector3 CalculateFriction(MathModule.Vector3 velocity, MathModule.Vector3 force, float normal, float mass)
        {
            var time = Time.fixedDeltaTime;
            var acceleration = new MathModule.Vector3();

            fe = ue * -MathModule.Vector3.Normalize(velocity) * normal;
            fc = uc * -MathModule.Vector3.Normalize(velocity) * normal;

            if (MathModule.Vector3.Magnitude(velocity) > 0f)
                acceleration = (force + fc / mass);
            else
                acceleration = (force + fe / mass);

            if (((MathModule.Vector3.Magnitude(force) - MathModule.Vector3.Magnitude(fe)) > 0f) || (MathModule.Vector3.Magnitude(velocity) > 0f))
                velocity += acceleration * time;
            else
                velocity = MathModule.Vector3.Zero;

            return velocity;
        }
    }
}