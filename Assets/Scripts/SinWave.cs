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
        }
    }

    private void BuildWave()
    {
        Debug.Log("count: " + lineRenderer.positionCount);
        Vector3[] points = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(points);

        Transform[] wavePoints = new Transform[points.Length];

        for (int i=0; i < points.Length; i++)
        {
            wavePoints[i] = Instantiate(wavePointPrefab, points[i], Quaternion.identity).transform;
            SpringJoint thisSpring = wavePoints[i].AddComponent<SpringJoint>();
        }
    }
}
