using UnityEngine;

public class Bird : MonoBehaviour
{
    public float mass = 5;
    public float maxVelocity;

    public Vector3 velocity = Vector3.zero;
    public Vector3 force;
    private Vector3 position;
    private Vector3 gravity = new Vector3(0, -9.81f);

    private void Start()
    {
        position = transform.position;
    }

    private void FixedUpdate()
    {
        Move();
        UpdateVelocity();
        transform.position = position;
    }

    private void UpdateVelocity()
    {
        Vector3 acc = force / mass + gravity;
        velocity = velocity + acc * Time.deltaTime;
        position = position + velocity * Time.deltaTime;
        force = Vector3.zero;
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            if (velocity.y < 3.0f)
                force = new Vector3(0, 3000, 0);
        
        this.Death();
    }

    public void Death()
    {
        if (this.transform.position.y < -3)
        {
            print("Morreu!");
            Time.timeScale = 0;
        }
    }
}