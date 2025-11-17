using UnityEngine;

public class WavePoint : MonoBehaviour
{

    public WavePoint previousPoint;
    public WavePoint nextPoint;

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
            previousJoint.autoConfigureDistance = false;
            previousJoint.enabled = true;
        } else
        {
            previousJoint.autoConfigureDistance = false;
            previousJoint.distance = 0.5f;
            previousJoint.enabled = false;
        }
    }

    public void SetPreviousPoint()
    {
        previousJoint.autoConfigureDistance = false;
        previousJoint.distance = 0.5f;
        previousJoint.enabled = false;
    }

    public void SetNextPoint(WavePoint point)
    {
        if (point != null)
        {
            nextPoint = point;
        }
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
