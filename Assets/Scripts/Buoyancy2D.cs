using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Buoyancy2D : MonoBehaviour
{
    public SinWave water;             
    public float buoyancyStrength = 15f;
    public float damping = 3f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (water == null)
            return;

        float waterY = water.GetSurfaceHeightAtX(transform.position.x);

        if (transform.position.y < waterY)
        {
            float depth = waterY - transform.position.y;

            // upward buoyancy
            rb.AddForce(Vector2.up * depth * buoyancyStrength);

            //water drag
            rb.AddForce(-rb.linearVelocity * damping);
        }
    }
}
