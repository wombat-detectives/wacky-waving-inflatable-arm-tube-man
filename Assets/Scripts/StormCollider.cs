using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StormMover2D : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player;
    public string playerTag = "Player";
    public float playerSpeedSampleRate = 0.1f;

    [Header("Storm Movement")]
    public float baseSpeed = 2f;         // minimum storm speed
    public float maxPerformanceSpeed = 10f;  // max storm boost from performance

    [Header("Difficulty Scaling")]
    public float difficultyMultiplier = 1.2f; //how much players speed influences difficulty
    public float responsivenessUp = 0.4f;     // how fast storm increases difficulty
    public float responsivenessDown = 0.05f;  // how slow storm decreases difficulty

    private float playerLastX;
    private float timeSinceLastSample;

    private float performanceScore = 0f;  // smoothed difficulty driver
    private bool gameOverTriggered = false;

    void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;

        if (player == null)
            player = GameObject.FindGameObjectWithTag(playerTag)?.transform;

        if (player != null)
            playerLastX = player.position.x;
        else
            Debug.LogWarning("storm no player reference found.");
    }

    void Update()
    {
        if (gameOverTriggered || player == null) return;

        UpdatePlayerPerformance();
        MoveStorm();
    }

    
    void UpdatePlayerPerformance()
    {
        timeSinceLastSample += Time.deltaTime;

        if (timeSinceLastSample >= playerSpeedSampleRate)
        {
            float playerX = player.position.x;
            float delta = playerX - playerLastX;

            float playerSpeed = delta / timeSinceLastSample;
            playerLastX = playerX;
            timeSinceLastSample = 0f;

           
            float rawPerformance = Mathf.Clamp(playerSpeed * difficultyMultiplier, 0f, maxPerformanceSpeed);

            
            if (rawPerformance > performanceScore)
            {
                
                performanceScore = Mathf.Lerp(
                    performanceScore,
                    rawPerformance,
                    responsivenessUp
                );
            }
            else
            {
               
                performanceScore = Mathf.Lerp(
                    performanceScore,
                    rawPerformance,
                    responsivenessDown
                );
            }
        }
    }

  
    void MoveStorm()
    {
        float stormSpeed = baseSpeed + performanceScore;
        transform.position += Vector3.right * stormSpeed * Time.deltaTime;
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameOverTriggered) return;

        if (other.CompareTag(playerTag))
        {
            gameOverTriggered = true;
            Debug.Log("game ova");

            if (GameManager.instance != null)
                GameManager.instance.GameOver();
        }
    }
}
