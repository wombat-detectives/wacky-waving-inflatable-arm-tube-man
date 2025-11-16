using Unity.VisualScripting;
using UnityEngine;

public class MoveWithPlayer : MonoBehaviour
{
    public Transform player;

    private Vector2 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = player.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(player.position.x - offset.x, transform.position.y);
    }
}
