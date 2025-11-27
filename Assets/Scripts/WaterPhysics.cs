using UnityEngine;

public class WavePhysics : MonoBehaviour
{
    public LinePointWave wave;
    public float buoyancy = 15f;       // upward force when submerged
    public float waterDrag = 2f;       // horizontal slow down when inside water
    public float waterAngularDrag = 2f;
    public float objectDepthOffset = 0f; // make object float slightly under water

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.LogError("WavePhysics requires a rigidbody.");
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (wave == null)
            return;

        float waterSurfaceY = SampleWaveHeight(transform.position.x);
        float objectY = transform.position.y + objectDepthOffset;

        float submergedDepth = waterSurfaceY - objectY;

      
        if (submergedDepth > 0f)
        {
            float force = submergedDepth * buoyancy;
            rb.AddForce(Vector3.up * force, ForceMode.Force);

            
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x * (1f - Time.fixedDeltaTime * waterDrag),
                rb.linearVelocity.y,
                rb.linearVelocity.z * (1f - Time.fixedDeltaTime * waterDrag)
            );

            rb.angularVelocity *= (1f - Time.fixedDeltaTime * waterAngularDrag);
        }

      
    }

    float SampleWaveHeight(float worldX)
    {
        var pts = wave.GetPoints();
        if (pts == null || pts.Count < 2)
            return 0f;

        for (int i = 0; i < pts.Count - 1; i++)
        {
            float x1 = pts[i].position.x;
            float x2 = pts[i + 1].position.x;

            if (worldX >= x1 && worldX <= x2)
            {
                float t = (worldX - x1) / (x2 - x1);
                return Mathf.Lerp(pts[i].position.y, pts[i + 1].position.y, t);
            }
        }

        if (worldX < pts[0].position.x) return pts[0].position.y;
        return pts[pts.Count - 1].position.y;
    }
}
