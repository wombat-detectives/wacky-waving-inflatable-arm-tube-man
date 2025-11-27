using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SinWave : MonoBehaviour
{
    public delegate float WaveFormula(float x, float time, float amplitude, float frequency);
    public WaveFormula customFormula;

    public Material wavePointMaterial;
    public LineRenderer lineRenderer;
    public int points;
    public float amplitude = 1.0f;
    public float frequency = 1.0f;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;

    public GameObject wavePointPrefab;
    public GameObject waveAnchorPrefab;

    private WavePoint[] wavePoints;
    private WavePoint firstWavePoint;
    private WavePoint lastWavePoint;
    private List<Rigidbody2D> waveAnchors;
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

    private void FixedUpdate()
    {
        //make the last few points chill tf out
        float x = xLimits.y; // upper limit
        float y = amplitude * Mathf.Sin((2 * MathF.PI * frequency * x) + Time.timeSinceLevelLoad * movementSpeed);
        Vector3 newPos = new Vector3(x, y, 0);

        lastWavePoint.rb.MovePosition(waveAnchors[points-1].position);
        lastWavePoint.rb.linearVelocity = Vector2.zero;

        lastWavePoint.previousPoint.rb.MovePosition(waveAnchors[points - 2].position);
        lastWavePoint.previousPoint.rb.linearVelocity = Vector2.zero;
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
            float time = Time.timeSinceLevelLoad * movementSpeed;

            float y;

            if (customFormula != null)
            {
                // Use external formula
                y = customFormula(x, time, amplitude, frequency);
            }
            else
            {
                // Default sine wave
                y = amplitude * Mathf.Sin((Tau * frequency * x) + time);
            }

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

        Rigidbody2D[] waveAnchorsArray = new Rigidbody2D[points.Length];
        waveAnchors = new List<Rigidbody2D>(waveAnchorsArray);
        
        WavePoint previousPoint = null;

        for (int i=0; i < points.Length; i++)
        {
            // Create wavePoint and anchor at each point
            wavePoints[i] = Instantiate(wavePointPrefab, points[i], Quaternion.identity).GetComponent<WavePoint>();
            waveAnchors[i] = Instantiate(waveAnchorPrefab, points[i], Quaternion.identity).GetComponent<Rigidbody2D>();

            // Apply material to each point
            Renderer r = wavePoints[i].GetComponent<Renderer>();
            if (r != null && wavePointMaterial != null)
            {
                r.material = wavePointMaterial;
            }

            // Assign spring targets
            wavePoints[i].SetPreviousPoint(previousPoint);
            wavePoints[i].SetWaveJoint(waveAnchors[i]);

            previousPoint = wavePoints[i];

            if (i == 0)
            {
                firstWavePoint = wavePoints[i];
            } else if (i == points.Length - 1)
            {
                lastWavePoint = wavePoints[i];
            }
        }

        WavePoint nextPoint = null;

        for (int i=points.Length-1; i >= 0; i--)
        {
            wavePoints[i].SetNextPoint(nextPoint);

            nextPoint = wavePoints[i];
        }

        isWaveBuilt = true;
    }

    [ContextMenu("Move Wave 1 point")]
    public void MoveWave()
    {
        WavePoint pointToMove = firstWavePoint;

        // handle anchors
        Rigidbody2D waveAnchorToMove = waveAnchors[0];
        waveAnchors.RemoveAt(0);
        waveAnchors.Add(waveAnchorToMove);

        // handle line renderer
        float moveDistance = (xLimits.y - xLimits.x) / points;
        xLimits = new Vector2(xLimits.x + moveDistance, xLimits.y + moveDistance);

        //find new location
        float x = xLimits.y; // upper limit
        float y = amplitude * Mathf.Sin((2 * MathF.PI * frequency * x) + Time.timeSinceLevelLoad * movementSpeed);
        Vector3 newPos = new Vector3(x, y, 0);

        // move anchor
        waveAnchorToMove.MovePosition(newPos);

        //move first point to new location
        pointToMove.SetPreviousPoint();
        pointToMove.rb.MovePosition(newPos);
        //pointToMove.transform.position = waveAnchorToMove.position;
        pointToMove.rb.linearVelocity = Vector3.zero;

        Debug.Log("distance after move: " + pointToMove.previousJoint.distance);

        // handle wavePoint
        firstWavePoint = firstWavePoint.nextPoint;
        firstWavePoint.SetPreviousPoint();
        pointToMove.SetPreviousPoint(lastWavePoint);
        lastWavePoint.SetNextPoint(pointToMove);
        lastWavePoint = pointToMove;

        //pointToMove.previousJoint.autoConfigureDistance = false;
        //pointToMove.previousJoint.distance = (pointToMove.previousPoint.waveJoint.connectedBody.position - pointToMove.waveJoint.connectedBody.position).magnitude;
        
        Debug.Log("distance after all: " + pointToMove.previousJoint.distance);


    }
    public float GetSurfaceHeightAtX(float x)
    {
        float progress = Mathf.InverseLerp(xLimits.x, xLimits.y, x);
        int index = Mathf.RoundToInt(progress * (points - 1));

        index = Mathf.Clamp(index, 0, points - 1);

        return waveAnchors[index].position.y;
    }

}
