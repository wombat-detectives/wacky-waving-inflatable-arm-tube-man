using UnityEngine;

public class WavePoint : MonoBehaviour
{

    public WavePoint previousPoint;
    public WavePoint nextPoint;

    public Rigidbody2D rb;

    public SpringJoint2D waveJoint;
    public SpringJoint2D previousJoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpringJoint2D[] springs = GetComponents<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();

        waveJoint = springs[0];
        previousJoint = springs[1];
    }

    public void SetPreviousPoint(WavePoint point)
    {
        if(previousPoint != null)
        {
            previousPoint = point;
            previousJoint.connectedBody = point.rb;
            previousPoint.enabled = true;
        } else
        {
            previousPoint.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
