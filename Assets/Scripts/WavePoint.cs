using UnityEngine;

public class WavePoint : MonoBehaviour
{

    public WavePoint previousPoint;

    public Rigidbody2D rb;

    public SpringJoint2D waveJoint;
    public SpringJoint2D previousJoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SpringJoint2D[] springs = GetComponents<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();

        waveJoint = springs[0];
        previousJoint = springs[1];
    }

    public void SetPreviousPoint(WavePoint point)
    {
        if(point != null)
        {
            previousPoint = point;
            previousJoint.connectedBody = point.rb;
            previousJoint.enabled = true;
        } else
        {
            previousJoint.enabled = false;
        }
    }

    public void SetPreviousPoint()
    {
        previousPoint.enabled = false;
    }

    public void SetWaveJoint(Rigidbody2D waveAnchor)
    {
        if (waveAnchor != null) 
        {
            waveJoint.connectedBody = waveAnchor;
            waveJoint.enabled = true;

            //waveJoint.autoConfigureDistance = false;
            //waveJoint.distance = 0.005f;
        } else
        {
            waveJoint.enabled = false;
        }
            
    }

}
