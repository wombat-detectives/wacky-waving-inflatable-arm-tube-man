using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    public float downForce = 1f;

    public Rigidbody2D rb;

    InputAction diveAction;

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
}
