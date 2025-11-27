using UnityEngine;
using TMPro;

public class DistanceTracker2D : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI distanceText;
    public bool useFixedUpdate = true;
    public int decimalPlaces = 1;
    public string prefix = "Distance: ";

    private float lastX;
    public float distanceTraveled { get; private set; }

    void Start()
    {
        if (player == null)
        {
            var obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) player = obj.transform;
        }

        if (player != null)
            lastX = player.position.x;
    }

    void Update()
    {
        if (!useFixedUpdate)
            Track();
    }

    void FixedUpdate()
    {
        if (useFixedUpdate)
            Track();
    }

    private void Track()
    {
        if (player == null) return;

        float currentX = player.position.x;
        float deltaX = currentX - lastX; 

        distanceTraveled += deltaX;
        lastX = currentX;

        if (distanceText != null)
        {
            distanceText.text = prefix +
                distanceTraveled.ToString("F" + decimalPlaces);
        }
    }

    public void ResetDistance()
    {
        distanceTraveled = 0f;
        if (player != null)
            lastX = player.position.x;
    }
}
