using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{

    public List<Transform> targets;
    public Vector3 offset;

    public float minZoom = 20;
    public float maxZoom = 8;
    public float zoomLimiter = 50f;
    public float zoomSpeedMod = 2f;

    private Vector3 velocity;
    public float smoothTime = 0.5f;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Move();

        Zoom();
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime * zoomSpeedMod);
    }

    Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i=0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }


    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        if(bounds.size.x > bounds.size.y)
            return bounds.size.x;
        else
            return bounds.size.y;
    }

    public void AddTarget(Transform targ)
    {
        targets.Add(targ);
    }

    public void RemoveTarget(int index)
    {
        if( targets.Count > 2 ) // Do not remove the first 2 targets (player and cursor)
        {
            targets.RemoveAt(index);
        }
    }

    public void RemoveTarget(Transform targ)
    {
        if (targets.Count > 2) // Do not remove the first 2 targets (player and cursor)
        {
            targets.Remove(targ);
        }
    }
}
