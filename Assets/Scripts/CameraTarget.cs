using UnityEngine;

public class CameraTarget : MonoBehaviour
{

    private MultipleTargetCamera multiCam;
    private bool onCam = false;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        multiCam = Camera.main.gameObject.GetComponent<MultipleTargetCamera>();

        if(GetComponent<Rigidbody2D>() != null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleTargeting();
    }


    private void HandleTargeting()
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPoint.x >= 0f && viewportPoint.x <= 1f &&
            viewportPoint.y >= 0f && viewportPoint.y <= 1f)
        {
            // Object is on the screen

            if (!onCam)
            {
                multiCam.AddTarget(transform);
                onCam = true;
            }

        }
        else
        {
            // Object is not on the screen
            if (onCam)
            {
                multiCam.RemoveTarget(transform);
                onCam = false;
            }
        }
    }
}
