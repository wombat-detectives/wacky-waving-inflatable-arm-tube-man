using UnityEngine;

public class WaveDifficultyController : MonoBehaviour
{
    [Header("SinWave script")]
    public SinWave sinWave;   

    [Header("Difficulty Timing")]
    public float difficultyStartDelay = 2f;  
    public float difficultyRampSpeed = 0.1f; 

    private float difficultyTimer = 0f;

    [Header("Amplitude Settings")]
    public bool scaleAmplitude = true;
    public float startingAmplitude = 1f;
    public float maxAmplitude = 3f;

    [Header("Frequency Settings")]
    public bool scaleFrequency = true;
    public float startingFrequency = 1f;
    public float maxFrequency = 3f;

    [Header("Wave Speed Settings")]
    public bool scaleMovementSpeed = true;
    public float startingMovementSpeed = 1f;
    public float maxMovementSpeed = 4f;

    void Start()
    {
        if (sinWave == null)
        {
            Debug.LogError("WaveDifficultyController no sinwave reference assigned");
            enabled = false;
            return;
        }

      
        sinWave.amplitude = startingAmplitude;
        sinWave.frequency = startingFrequency;
        sinWave.movementSpeed = startingMovementSpeed;
    }

    void Update()
    {
        if (Time.time < difficultyStartDelay)
            return;

       
        difficultyTimer += Time.deltaTime * difficultyRampSpeed;

        float t = Mathf.Clamp01(difficultyTimer);

     
        if (scaleAmplitude)
            sinWave.amplitude = Mathf.Lerp(startingAmplitude, maxAmplitude, t);

       
        if (scaleFrequency)
            sinWave.frequency = Mathf.Lerp(startingFrequency, maxFrequency, t);

        
        if (scaleMovementSpeed)
            sinWave.movementSpeed = Mathf.Lerp(startingMovementSpeed, maxMovementSpeed, t);
    }
}
