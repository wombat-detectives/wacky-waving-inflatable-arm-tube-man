using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float targetSpeed;

    public Rigidbody2D rb;

    InputAction diveAction;

    private WavePoint wavePointBelow;
    private WavePoint wavePointPrevious;
    private float previousSlope = -10f;

    // State machine
    private bool ridingWaveDown;
    private bool ridingWaveUp;
    private bool diving;

    // Stats
    public float downForce = 1f;
    public float downSlopeForce = 1.1f;
    public float upSlopeForce = 1.1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        diveAction = InputSystem.actions.FindAction("Dive");
    }

    // Update is called once per frame
    void Update()
    {
        if (diveAction.IsPressed()){
            diving = true;
            
        } else
        {
            diving = false;
        }
    }

    private void FixedUpdate()
    {
        // State machine
        if (ridingWaveDown)
        {
            WaveRideDown();
        } else if (ridingWaveUp)
        {
            WaveRideUp();
        } else if (diving)
        {
            Dive();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(diving && collision.collider.tag == "Water" && !ridingWaveUp && !ridingWaveDown)
        {
            HitWater(collision);
        }
    }

    private void Dive()
    {
        rb.AddForce(Vector2.down * downForce);
    }

    private void HitWater(Collision2D collision)
    {
        wavePointBelow = collision.transform.GetComponent<WavePoint>();
        wavePointPrevious = wavePointBelow.previousPoint;

        float slope = GetSlope();

        // Bad Hit
        if (slope > 0)
        {
            rb.AddForceX(upSlopeForce * Mathf.Abs(slope));
        }
        else // Good Hit
        {
            rb.AddForceX(downSlopeForce * Mathf.Abs(slope));
            StartWaveRideDown();
        }
    }

    private void SetWavePointBelow()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 99f, LayerMask.GetMask("Water"));
        // if raycast hits water
        if (hit && hit.transform.GetComponent<WavePoint>() != null)
        {
            wavePointBelow = hit.transform.GetComponent<WavePoint>();
            wavePointPrevious = wavePointBelow.previousPoint;
        }
    }

    private void StartWaveRideDown()
    {
        ridingWaveDown = true;

        SetWavePointBelow();

        // get current velocity
        float currVel = rb.linearVelocity.magnitude;
        // get slope
        float slope = GetSlope();

        // set target speed based on current velocity, downslopeforce, and slope
        targetSpeed = currVel * Mathf.Abs(slope) * downSlopeForce;
    }

    private void StartWaveRideUp()
    {
        ridingWaveDown = false;
        ridingWaveUp = true;

        SetWavePointBelow();
        previousSlope = -10f;
    }

    private void WaveRideDown()
    {
        SetWavePointBelow();

        
        float slope = GetSlope();
        if (slope <= 0)
        {
            // Use slope to get vector for direction
            Vector2 dir = (new Vector2(1, slope)).normalized;

            Debug.DrawRay(transform.position, dir * 10, Color.red);

            //translate towards that point, adding speed if holding down
            if (diving)
            {
                rb.linearVelocity = dir * (rb.linearVelocity.magnitude * downSlopeForce);
            }
                //rb.AddForce(dir * downSlopeForce);
        }
        else
        {
            //if slope goes up, stop adding speed, exit condition
            StartWaveRideUp();
        }
    }

    private void WaveRideUp()
    {
        SetWavePointBelow();

        float slope = GetSlope();

        if(slope >= previousSlope)
        {
            Vector2 dir = (new Vector2(1, slope)).normalized;
            Debug.DrawRay(transform.position, dir * 10, Color.green);

            if (!diving)
            {
                rb.linearVelocity = dir * (rb.linearVelocity.magnitude * upSlopeForce);
            }
                //rb.AddForce(dir * upSlopeForce);
        } else
        {
            ExitWaveRide();
        }
        // translate towards that point normalized, maintaining speed if not holding down, reducing speed if holding down
        // if a new point is hit, update next point
        // if current slope value is less than previous, exit

        previousSlope = slope;
    }

    private void ExitWaveRide()
    {
        ridingWaveDown = false;
        ridingWaveUp = false;

    }


    private float GetSlope()
    {
        float slope;
        if(wavePointBelow != null && wavePointPrevious != null)
        {
            slope = (wavePointBelow.waveJoint.connectedBody.position.y - wavePointPrevious.waveJoint.connectedBody.position.y) / (wavePointBelow.waveJoint.connectedBody.position.x - wavePointPrevious.waveJoint.connectedBody.position.x);
        } else
        {
            slope = 0.0f;
        }

        return slope;
    }
}
