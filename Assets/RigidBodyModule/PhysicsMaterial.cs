using PhysicsEngine.MathModule;

using UnityEditor;

using UnityEngine;

using Vector3 = PhysicsEngine.MathModule.Vector3;

namespace PhysicsEngine.CollisionModule
{
    public class PhysicsMaterial : MonoBehaviour
    {
        [SerializeField] private float ue = 0.3f;
        [SerializeField] private float uc = 0.3f;

        private Vector3 fe;
        private Vector3 fc;

        public Vector3 CalculateFriction(Vector3 velocity, Vector3 force, float normal, float mass)
        {
            float time = Time.fixedDeltaTime;
            var acceleration = new Vector3();

            fe = ue * -Vector3.Normalize(velocity) * normal;
            fc = uc * -Vector3.Normalize(velocity) * normal;

            if (Vector3.Magnitude(velocity) > 0f)
                acceleration = (force + fc / mass);
            else
                acceleration = (force + fe / mass);

            if (((Vector3.Magnitude(force) - Vector3.Magnitude(fe)) > 0f) || (Vector3.Magnitude(velocity) > 0f))
                velocity += acceleration * time;
            else
                velocity = Vector3.Zero;

            return velocity;
        }
    }
}