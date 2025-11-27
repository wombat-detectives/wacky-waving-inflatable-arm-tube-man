using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WaterWavePhysics : MonoBehaviour
{
    public Material wavePointMaterial;
    public LineRenderer lineRenderer;
    public int points = 20;

    public float amplitude = 1f;
    public float frequency = 1f;
    public float movementSpeed = 1f;

    public float springForce = 60f;     // replaces anchor springs
    public float damping = 4f;          // smooths jitter

    public GameObject wavePointPrefab;

    private WavePoint[] wavePoints;
    private WavePoint firstWavePoint;
    private WavePoint lastWavePoint;

    private bool isBuilt = false;

    public Vector2 xLimits = new Vector2(0, 1);

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        DrawSineOnly();
        BuildPoints();
    }

    void Update()
    {
        DrawSineOnly();
    }

    void FixedUpdate()
    {
        ApplySineSpringForce();
    }

    // ---------------------------------------------------------
    // DRAW THE SINE (NO MORE ANCHORS)
    // ---------------------------------------------------------
    void DrawSineOnly()
    {
        lineRenderer.positionCount = points;

        float xStart = xLimits.x;
        float xEnd = xLimits.y;
        float tau = 2f * Mathf.PI;

        for (int i = 0; i < points; i++)
        {
            float p = (float)i / (points - 1);
            float x = Mathf.Lerp(xStart, xEnd, p);
            float y = amplitude * Mathf.Sin((tau * frequency * x) + Time.time * movementSpeed);

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    // ---------------------------------------------------------
    // BUILD PHYSICAL POINTS (WavePoint Prefabs)
    // ---------------------------------------------------------
    void BuildPoints()
    {
        Vector3[] sinePositions = new Vector3[points];
        lineRenderer.GetPositions(sinePositions);

        wavePoints = new WavePoint[points];

        WavePoint prev = null;

        for (int i = 0; i < points; i++)
        {
            WavePoint wp = Instantiate(wavePointPrefab, sinePositions[i], Quaternion.identity).GetComponent<WavePoint>();
            wavePoints[i] = wp;

            Renderer r = wp.GetComponent<Renderer>();
            if (r && wavePointMaterial) r.material = wavePointMaterial;

            wp.SetPreviousPoint(prev);

            prev = wp;

            if (i == 0) firstWavePoint = wp;
            if (i == points - 1) lastWavePoint = wp;
        }

        // next pointer chain
        WavePoint next = null;
        for (int i = points - 1; i >= 0; i--)
        {
            wavePoints[i].SetNextPoint(next);
            next = wavePoints[i];
        }

        isBuilt = true;
    }

    // ---------------------------------------------------------
    // APPLY SPRING FORCE TOWARD SINE (REPLACES ANCHORS)
    // ---------------------------------------------------------
    void ApplySineSpringForce()
    {
        if (!isBuilt) return;

        float xStart = xLimits.x;
        float xEnd = xLimits.y;
        float tau = 2f * Mathf.PI;

        for (int i = 0; i < points; i++)
        {
            WavePoint wp = wavePoints[i];

            float p = (float)i / (points - 1);
            float targetX = Mathf.Lerp(xStart, xEnd, p);
            float targetY = amplitude * Mathf.Sin((tau * frequency * targetX) + Time.time * movementSpeed);

            float displacement = wp.rb.position.y - targetY;
            float force = -displacement * springForce - wp.rb.linearVelocity.y * damping;

            wp.rb.AddForce(Vector2.up * force);
        }
    }

    // ---------------------------------------------------------
    // SCROLLING THE WAVE (keeps same behavior)
    // ---------------------------------------------------------
    [ContextMenu("Move Wave 1 point")]
    public void MoveWave()
    {
        float moveDist = (xLimits.y - xLimits.x) / points;
        xLimits = new Vector2(xLimits.x + moveDist, xLimits.y + moveDist);

        WavePoint moved = firstWavePoint;

        firstWavePoint = firstWavePoint.nextPoint;
        firstWavePoint.SetPreviousPoint(null);

        moved.SetPreviousPoint(lastWavePoint);
        lastWavePoint.SetNextPoint(moved);
        lastWavePoint = moved;
    }

    // For buoyancy queries
    public float GetSurfaceY(float x)
    {
        int i = Mathf.RoundToInt((x - xLimits.x) / ((xLimits.y - xLimits.x) / (points - 1)));
        i = Mathf.Clamp(i, 0, points - 1);
        return wavePoints[i].rb.position.y;
    }

    public void Splash(float x, float force)
    {
        int i = Mathf.RoundToInt((x - xLimits.x) / ((xLimits.y - xLimits.x) / (points - 1)));
        if (i < 0 || i >= points) return;

        wavePoints[i].rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }
}
