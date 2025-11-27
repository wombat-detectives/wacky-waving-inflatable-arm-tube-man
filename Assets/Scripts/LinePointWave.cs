using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LinePointWave : MonoBehaviour
{
    public bool updateEveryFrame = true;
    public float refreshInterval = 0.1f;  

    private LineRenderer line;
    private List<Transform> points = new List<Transform>();
    private float refreshTimer;
 


    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
    }

    void Start()
    {
        RefreshPointsList();
        UpdateLine();
    }

    void LateUpdate()
    {
        if (updateEveryFrame)
            UpdateLine();

        refreshTimer += Time.deltaTime;
        if (refreshTimer >= refreshInterval)
        {
            refreshTimer = 0f;
            RefreshPointsList();
        }
    }

    void RefreshPointsList()
    {
        points.Clear();

        // find all objects named wavePoint(Clone) (change if prefab name is changed)
        GameObject[] found = GameObject.FindObjectsOfType<GameObject>();

        foreach (var go in found)
        {
            if (go.name == "wavePoint(Clone)")
            {
                points.Add(go.transform);
            }
        }

        // sort by x so the line flows left to right
        points.Sort((a, b) => a.position.x.CompareTo(b.position.x));
    }

    void UpdateLine()
    {
        if (points.Count < 2)
            return;

        line.positionCount = points.Count;

        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i].position);
        }
    }
    public List<Transform> GetPoints()
    {
        return points;
    }

}
