using UnityEngine;
using Vector3 = Kinematics.MathModule.Vector3;

namespace Kinematics.CollisionModule
{
    [AddComponentMenu("PhysicsEngine/CollisionModule/RigidBody")]
    public class RigidBody : MonoBehaviour
    {
        [SerializeField, GetSet("Mass")]
        private float mass = 0.1f;
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }
        
        [SerializeField] private float Drag = 0.0f;

        [SerializeField] private bool UseGravity = false;

        [SerializeField] private Vector3 Gravity = new Vector3(0.0f, -9.81f, 0.0f);

        [SerializeField] private bool IsKinematic = false;

        [Header("Freeze Position")]
        [SerializeField] private bool X = false;
        [SerializeField] private bool Y = false;
        [SerializeField] private bool Z = false;

        private Vector3 velocity = Vector3.Zero;
        private Vector3 position = Vector3.Zero;

        new Collider collider;

        /* UnityEngine Properties */
        private float deltaTime;
        private float fixedDeltaTime;

        UnityEngine.Vector3 pos = Vector3.Zero;

        private void Start()
        {
            deltaTime = Time.deltaTime;
            fixedDeltaTime = Time.fixedDeltaTime;

            collider = GetComponent<Collider>();
            position.X = transform.position.x;
        }

        private void Update()
        {
            velocity += Gravity * fixedDeltaTime * (UseGravity ? 1 : 0);

            if (!IsKinematic)
                velocity *= (1 - (Drag * fixedDeltaTime));

            velocity = (Vector3.Right * velocity.X * (X ? 0 : 1)) + (Vector3.Up * velocity.Y * (Y ? 0 : 1)) + (Vector3.Forward * velocity.Z * (Z ? 0 : 1));

            if (CheckCollision(Vector3.Up * velocity.Y * deltaTime) != null)
            {
                if (collider.GetPhysicsMaterial() != null)
                    velocity = collider.GetPhysicsMaterial().CalculateFriction(velocity, Vector3.Zero, Vector3.Magnitude(Gravity) * Mass, Mass);

                if (CheckCollision(Vector3.Up * velocity.Y * deltaTime).GetCenter().Y + CheckCollision(Vector3.Up * velocity.Y * deltaTime).GetSize().Y <= collider.GetCenter().Y)
                    velocity.Y = 0;
            }

            position += velocity * deltaTime;
            pos = position;
            transform.position = pos;
        }

        public void AddForce(Vector3 force)
        {
            if (!IsKinematic)
                velocity += ((force / Mass) + Gravity * (UseGravity ? 1 : 0)) * fixedDeltaTime;
        }

        public void SetVelocity(Vector3 velocity)
        {
            this.velocity = velocity;
        }

        public Collider CheckCollision(Vector3 position)
        {
            if (collider != null)
            {
                var aux = collider.center;
                collider.center = position;

                for (int i = 0; i < Collider.colliders.Count; i++)
                {
                    if (Collider.CheckCollision(collider, Collider.colliders[i]) != null)
                        return Collider.CheckCollision(collider, Collider.colliders[i]);
                }

                collider.center = aux;
            }

            return null;
        }
    }
}
