using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    public float downForce = 1f;

    public Rigidbody2D rb;

    InputAction diveAction;

    private WavePoint wavePointBelow;
    private WavePoint wavePointPrevious;

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
            rb.AddForce(Vector2.down * downForce * Time.deltaTime);
            Debug.Log("down");
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 99f, LayerMask.GetMask("Water"));
        Debug.DrawRay(transform.position, Vector2.down * 99f, Color.red);
        
        //Debug.DrawLine(transform.position, Vector2.down * 99, Color.green);
        

        // if raycast hits water
        if (hit && hit.transform.GetComponent<WavePoint>() != null)
        {
            Debug.Log("hit? " + hit.transform.name);
            wavePointBelow = hit.transform.GetComponent<WavePoint>();
            wavePointPrevious = wavePointBelow.previousPoint;

            Debug.Log(GetSlope());
        }
    }

    private float GetSlope()
    {
        float slope;
        if(wavePointBelow != null && wavePointPrevious != null)
        {
            slope = (wavePointBelow.transform.position.y - wavePointPrevious.transform.position.y) / (wavePointBelow.transform.position.x - wavePointPrevious.transform.position.x);
        } else
        {
            slope = 0.0f;
        }

        return slope;
    }
}
