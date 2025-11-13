using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SinWave : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int points;
    public float amplitude = 1.0f;
    public float frequency = 1.0f;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;

    public GameObject wavePointPrefab;
    public GameObject waveAnchorPrefab;

    private WavePoint[] wavePoints;
    private Rigidbody2D[] waveAnchors;
    private bool isWaveBuilt = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Draw();
        BuildWave();
    }

    void Update()
    {
        Draw();
    }

    void Draw()
    {
        float xStart = xLimits.x;
        float Tau = 2 * Mathf.PI;
        float xFinish = xLimits.y;

        lineRenderer.positionCount = points;

        for(int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = amplitude * Mathf.Sin((Tau * frequency * x) + Time.timeSinceLevelLoad * movementSpeed);
            lineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));

            //Move Rigidbodies
            if (isWaveBuilt)
            {
                waveAnchors[currentPoint].MovePosition(new Vector3(x, y, 0));
            }
        }
    }

    private void BuildWave()
    {
        // Get Positions of points created by linerenderer
        Vector3[] points = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(points);

        wavePoints = new WavePoint[points.Length];
        waveAnchors = new Rigidbody2D[points.Length];
        WavePoint previousPoint = null;

        for (int i=0; i < points.Length; i++)
        {
            // Create wavePoint and anchor at each point
            wavePoints[i] = Instantiate(wavePointPrefab, points[i], Quaternion.identity).GetComponent<WavePoint>();
            waveAnchors[i] = Instantiate(waveAnchorPrefab, points[i], Quaternion.identity).GetComponent<Rigidbody2D>();

            // Assign spring targets
            wavePoints[i].SetPreviousPoint(previousPoint);
            wavePoints[i].SetWaveJoint(waveAnchors[i]);

            previousPoint = wavePoints[i];
        }

        isWaveBuilt = true;
    }
}
