using UnityEngine;

public class WaveFormulaManager : MonoBehaviour
{
    [Header("References")]
    public DistanceTracker2D distanceTracker;
    public WaveFormulaTester waveTester;

    [Header("Distance Breakpoints")]
    public float[] distanceThresholds;   // Example 10, 25, 50, 100

    [Header("Wave Types To Switch To")]
    public WaveFormulaTester.WaveType[] waveTypes;
    // Same length as thresholds

    [Header("Amplitude Changes")]
    public float[] amplitudeMultipliers;     

    [Header("Frequency Changes")]
    public float[] frequencyMultipliers;    

    private int lastIndex = -1;

    void Update()
    {
        if (distanceTracker == null || waveTester == null) return;

        float dist = distanceTracker.distanceTraveled;

        for (int i = 0; i < distanceThresholds.Length; i++)
        {
            if (lastIndex == i) continue; 
            if (dist >= distanceThresholds[i])
            {
                ApplyWaveSettings(i);
                lastIndex = i;
            }
        }
    }

    void ApplyWaveSettings(int index)
    {
        // Change wave type
        if (index < waveTypes.Length)
            waveTester.waveType = waveTypes[index];

        // Change amplitude
        if (index < amplitudeMultipliers.Length)
            waveTester.amplitudeMultiplier = amplitudeMultipliers[index];

        // Change frequency
        if (index < frequencyMultipliers.Length)
            waveTester.frequencyMultiplier = frequencyMultipliers[index];

        Debug.Log($"[WaveFormulaManager] Applied preset #{index} at distance {distanceThresholds[index]}");
    }
}
