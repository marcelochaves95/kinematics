using Kinematics.MathModule;
using UnityEngine;
using Vector3 = Kinematics.MathModule.Vector3;

namespace Kinematics.Collision
{
    [AddComponentMenu("Kinematics/Collision/RigidBody")]
    public class Rigidbody : MonoBehaviour
    {
        [SerializeField]
        private float mass = 0.1f;
        public float Mass
        {
            get => mass;
            set
            {
                if (Mathematics.Abs(mass - 0.1f) > Mathematics.Epsilon)
                {
                    mass = value;
                }
            }
        }

        [SerializeField]
        private float drag;

        [SerializeField]
        private bool useGravity;

        [SerializeField]
        private Vector3 gravity = new Vector3(0.0f, -9.81f, 0.0f);

        [SerializeField]
        private bool isKinematic;

        [Header("Freeze Position")]
        [SerializeField]
        private bool x;
        [SerializeField]
        private bool y;
        [SerializeField]
        private bool z;

        private Vector3 velocity = Vector3.Zero;
        private Vector3 position = Vector3.Zero;

        private Collider collider;

        #region Unity Properties
        private float deltaTime;
        private float fixedDeltaTime;

        private UnityEngine.Vector3 pos = Vector3.Zero;
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
                    velocity = collider.GetPhysicsMaterial().CalculateFriction(velocity, Vector3.Zero, Vector3.Magnitude(gravity) * Mass, Mass);
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
                velocity += ((force / Mass) + gravity * (useGravity ? 1 : 0)) * fixedDeltaTime;
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
                Vector3 aux = collider.center;
                collider.center = position;

                for (int i = 0; i < Collider.colliders.Count; i++)
                {
                    if (Collider.CheckCollision(collider, Collider.colliders[i]) != null)
                    {
                        return Collider.CheckCollision(collider, Collider.colliders[i]);
                    }
                }

                collider.center = aux;
            }

            return null;
        }
    }
}