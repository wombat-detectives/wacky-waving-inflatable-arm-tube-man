using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class WaveWater : MonoBehaviour
{
    public LinePointWave linePoints;
    public float bottomY = -2f;

    [Range(0f, 1f)]
    public float smoothing = 0.5f;

    [Header("Spike Control")]
    public float maxWaveHeight = 2f;
    public float minWaveHeight = -2f;

    private Mesh waterMesh;
    private MeshFilter meshFilter;
    private EdgeCollider2D edge;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        waterMesh = new Mesh();
        waterMesh.MarkDynamic();
        meshFilter.sharedMesh = waterMesh;

        edge = GetComponent<EdgeCollider2D>();
    }

    void Update()
    {
        BuildWaterMesh();
    }

    void BuildWaterMesh()
    {
        if (linePoints == null)
            return;

        List<Transform> pts = linePoints.GetPoints();
        if (pts == null || pts.Count < 2)
            return;

        int count = pts.Count;

        Vector3[] verts = new Vector3[count * 2];
        int[] tris = new int[(count - 1) * 6];

        
        Vector2[] colliderPts = new Vector2[count];

        for (int i = 0; i < count; i++)
        {
            Vector3 top = pts[i].position;
            if (i > 0 && i < count - 1)
            {
                Vector3 prev = pts[i - 1].position;
                Vector3 next = pts[i + 1].position;
                top = Vector3.Lerp(top, (prev + next) * 0.5f, smoothing);
            }

            top.y = Mathf.Clamp(top.y, minWaveHeight, maxWaveHeight);

            verts[i] = transform.InverseTransformPoint(top);
            colliderPts[i] = new Vector2(verts[i].x, verts[i].y);
        }

   
        edge.points = colliderPts;

        
        for (int i = 0; i < count; i++)
        {
            Vector3 topWorld = pts[i].position;
            Vector3 bottomWorld = new Vector3(topWorld.x, bottomY, 0);

            verts[i + count] = transform.InverseTransformPoint(bottomWorld);
        }

        
        int t = 0;
        for (int i = 0; i < count - 1; i++)
        {
            int tl = i;
            int tr = i + 1;
            int bl = i + count;
            int br = i + 1 + count;

            tris[t++] = tl;
            tris[t++] = tr;
            tris[t++] = bl;

            tris[t++] = bl;
            tris[t++] = tr;
            tris[t++] = br;
        }

        waterMesh.Clear();
        waterMesh.vertices = verts;
        waterMesh.triangles = tris;
        waterMesh.bounds = new Bounds(Vector3.zero, new Vector3(9999f, 9999f, 1f));
    }
}
