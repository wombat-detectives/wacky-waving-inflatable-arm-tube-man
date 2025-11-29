using Unity.VisualScripting;
using UnityEngine;

public class BoatRotator : MonoBehaviour
{
    public float upperRotLimit = 60f;
    public float lowerRotLimit = -60f;

    public float rotationRate = 1.0f;

    public float velThreshold = 1f;

    public Rigidbody2D parentRB;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float currentRot = transform.rotation.eulerAngles.z;
        parentRB = GetComponentInParent<Rigidbody2D>();

        //desiredRotation = new Vector3();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( parentRB.linearVelocityX > velThreshold )
        {
            DoRotate();
        } else
        {
            UnRotate();
        }
    }

    void DoRotate()
    {
        Vector2 v = parentRB.linearVelocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        if(angle > upperRotLimit)
            angle = upperRotLimit;
        else if (angle < lowerRotLimit)
            angle = lowerRotLimit;

        float currAngle = transform.rotation.eulerAngles.z;

        //float desiredAngle = Mathf.Lerp(transform.rotation.eulerAngles.z, angle, Time.deltaTime);
        Debug.Log(transform.rotation.eulerAngles.z);

        //transform.rotation = Lerp(transform.rotation, v, angle);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotationRate * Time.deltaTime);
    }

    void UnRotate()
    {
        Quaternion rot = transform.rotation;
        rot.eulerAngles = Vector3.zero;

        transform.rotation = rot;

    }
}
