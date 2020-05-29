using UnityEngine;
using Vector3 = Kinematics.Mathematics.Vector3;

namespace Kinematics.Collision
{
    [AddComponentMenu("Kinematics/Collision/RigidBody")]
    public class Rigidbody : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private float mass = 0.1f;
        [SerializeField, HideInInspector]
        private float drag;
        [SerializeField, HideInInspector]
        private bool useGravity;
        [SerializeField, HideInInspector]
        private bool isKinematic;

        [Header("Freeze Position")]
        [SerializeField, HideInInspector]
        private bool x;
        [SerializeField, HideInInspector]
        private bool y;
        [SerializeField, HideInInspector]
        private bool z;

        [SerializeField, HideInInspector]
        public Vector3 velocity = Vector3.Zero;
        [SerializeField, HideInInspector]
        public Vector3 position = Vector3.Zero;
        [SerializeField, HideInInspector]
        public Vector3 rotation = Vector3.Zero;

        [SerializeField]
        private RigidbodyConstraints constraints;
        
        private Collider collider;
        private Vector3 gravity = new Vector3(0.0f, -9.81f, 0.0f);

        #region Unity Properties
        private float deltaTime;
        private float fixedDeltaTime;

        private Vector3 pos = Vector3.Zero;
        #endregion

        private void Start()
        {
            deltaTime = Time.deltaTime;
            fixedDeltaTime = Time.fixedDeltaTime;

            collider = GetComponent<Collider>();
            position.X = transform.position.x;
        }

        private void Update()
        {
            velocity += gravity * fixedDeltaTime * (useGravity ? 1 : 0);

            if (!isKinematic)
            {
                velocity *= (1 - (drag * fixedDeltaTime));
            }

            velocity = (Vector3.Right * velocity.X * (x ? 0 : 1)) + (Vector3.Up * velocity.Y * (y ? 0 : 1)) + (Vector3.Forward * velocity.Z * (z ? 0 : 1));

            if (CheckCollision(Vector3.Up * velocity.Y * deltaTime) != null)
            {
                if (collider.GetPhysicsMaterial() != null)
                {
                    velocity = collider.GetPhysicsMaterial().CalculateFriction(velocity, Vector3.Zero, Vector3.Magnitude(gravity) * mass, mass);
                }

                if (CheckCollision(Vector3.Up * velocity.Y * deltaTime).GetCenter().Y + CheckCollision(Vector3.Up * velocity.Y * deltaTime).GetSize().Y <= collider.GetCenter().Y)
                {
                    velocity.Y = 0;
                }
            }

            position += velocity * deltaTime;
            pos = position;
            transform.position = pos;
        }

        public void AddForce(Vector3 force)
        {
            if (!isKinematic)
            {
                velocity += (force / mass + gravity * (useGravity ? 1 : 0)) * fixedDeltaTime;
            }
        }

        public void SetVelocity(Vector3 velocity)
        {
            this.velocity = velocity;
        }

        public Collider CheckCollision(Vector3 position)
        {
            if (collider != null)
            {
                Vector3 aux = collider.Center;
                collider.Center = position;

                foreach (Collider collider in Collider.Colliders)
                {
                    if (Collider.CheckCollision(collider, collider) != null)
                    {
                        return Collider.CheckCollision(collider, collider);
                    }
                }

                collider.Center = aux;
            }

            return null;
        }
    }
}