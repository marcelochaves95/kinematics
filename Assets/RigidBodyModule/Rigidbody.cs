using PhysicsEngine.MathModule;

namespace PhysicsEngine.CollisionModule
{
    public class Rigidbody : UnityEngine.MonoBehaviour
    {
        private float deltaTime = UnityEngine.Time.deltaTime;
        private float fixedDeltaTime = UnityEngine.Time.fixedDeltaTime;

        Collider collider;

        [UnityEngine.SerializeField] private float mass = 0.1f;
        [UnityEngine.SerializeField] private float drag = 0.0f;
        [UnityEngine.SerializeField] private bool useGravity = false;
        [UnityEngine.SerializeField] Vector3 gravity = -Vector3.Up * 9.81f;

        [UnityEngine.SerializeField] private bool isKinematic = false;

        [UnityEngine.Header("Freeze Position")]
        [UnityEngine.SerializeField] private bool pX = false;
        [UnityEngine.SerializeField] private bool pY = false;
        [UnityEngine.SerializeField] private bool pZ = false;

        Vector3 velocity = Vector3.Zero;
        Vector3 position = Vector3.Zero;
        UnityEngine.Vector3 pos = UnityEngine.Vector3.zero;

        private void Start()
        {
            collider = this.GetComponent<Collider>();
            position.X = this.transform.position.x;
        }

        private void Update()
        {
            velocity += gravity * fixedDeltaTime * (useGravity ? 1 : 0);

            if (!isKinematic)
                velocity *= (1 - (drag * fixedDeltaTime));

            velocity = (Vector3.Right * velocity.X * (pX ? 0 : 1)) + (Vector3.Up * velocity.Y * (pY ? 0 : 1)) + (Vector3.Forward * velocity.Z * (pZ ? 0 : 1));

            if (CheckCollision(Vector3.Up * velocity.Y * deltaTime) != null)
            {
                if (collider.GetPhysicsMaterial() != null)
                    velocity = collider.GetPhysicsMaterial().CalculateFriction(velocity, Vector3.Zero, Vector3.Magnitude(gravity) * mass, mass);

                if (CheckCollision(Vector3.Up * velocity.Y * deltaTime).GetCenter().Y + CheckCollision(Vector3.Up * velocity.Y * deltaTime).GetSize().Y <= collider.GetCenter().Y)
                    velocity.Y = 0;
            }

            position += velocity * deltaTime;
            pos.x = position.X;
            pos.y = position.Y;
            pos.z = position.Z;
            this.transform.position = pos;
        }

        public void AddForce(Vector3 force)
        {
            if (!isKinematic)
                velocity += ((force / mass) + gravity * (useGravity?1 : 0)) * fixedDeltaTime;
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

                for (int i = 0; i < Collider.rb_Colliders.Count; i++)
                {
                    if (Collider.CheckCollision(collider, Collider.rb_Colliders[i]) != null)
                        return Collider.CheckCollision(collider, Collider.rb_Colliders[i]);
                }

                collider.center = aux;
            }

            return null;
        }
    }
}