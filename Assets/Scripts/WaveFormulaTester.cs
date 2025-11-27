using UnityEngine;

public class WaveFormulaTester : MonoBehaviour
{
    public SinWave sinWave;

    public enum WaveType
    {
        SimpleSine,
        LayeredSine,
        Noise,
        NoisePlusSine,
        Square,
        Triangle,
        Custom1,
        Custom2
    }

    [Header("Wave Type")]
    public WaveType waveType = WaveType.SimpleSine;

    [Header("General Settings")]
    public float amplitudeMultiplier = 1f;
    public float frequencyMultiplier = 1f;

    [Header("Layered Sine Settings")]
    public float layer2Strength = 0.5f;
    public float layer2Frequency = 2f;

    public float layer3Strength = 0.25f;
    public float layer3Frequency = 0.5f;

    [Header("Noise Settings")]
    public float noiseScale = 1.3f;
    public float noiseSpeed = 0.4f;
    public float noiseStrength = 1f;

    [Header("Custom Formula 1")]
    public float customA = 1f;
    public float customB = 2f;

    [Header("Custom Formula 2")]
    public float customC = 0.4f;
    public float customD = 3f;

    void Start()
    {
        if (sinWave != null)
            sinWave.customFormula = EvaluateWave;
    }


    float EvaluateWave(float x, float time, float amplitude, float frequency)
    {
        amplitude *= amplitudeMultiplier;
        frequency *= frequencyMultiplier;

        switch (waveType)
        {
            case WaveType.SimpleSine:
                return SimpleSine(x, time, amplitude, frequency);

            case WaveType.LayeredSine:
                return LayeredSine(x, time, amplitude, frequency);

            case WaveType.Noise:
                return Noise(x, time, amplitude);

            case WaveType.NoisePlusSine:
                return NoisePlusSine(x, time, amplitude, frequency);

            case WaveType.Square:
                return SquareWave(x, time, amplitude, frequency);

            case WaveType.Triangle:
                return TriangleWave(x, time, amplitude, frequency);

            case WaveType.Custom1:
                return CustomWave1(x, time, amplitude, frequency);

            case WaveType.Custom2:
                return CustomWave2(x, time, amplitude, frequency);

            default:
                return 0f;
        }
    }

    

    float SimpleSine(float x, float time, float amp, float freq)
    {
        return amp * Mathf.Sin(freq * x + time);
    }

    float LayeredSine(float x, float time, float amp, float freq)
    {
        float w1 = amp * Mathf.Sin(freq * x + time);
        float w2 = amp * layer2Strength * Mathf.Sin(freq * layer2Frequency * x + time * 1.2f);
        float w3 = amp * layer3Strength * Mathf.Sin(freq * layer3Frequency * x + time * 0.7f);

        return w1 + w2 + w3;
    }

    float Noise(float x, float time, float amp)
    {
        float n = Mathf.PerlinNoise(x * noiseScale, time * noiseSpeed);
        return (n - 0.5f) * noiseStrength * amp;
    }

    float NoisePlusSine(float x, float time, float amp, float freq)
    {
        float baseWave = amp * Mathf.Sin(freq * x + time);
        float n = Mathf.PerlinNoise(x * noiseScale, time * noiseSpeed);
        return baseWave + (n - 0.5f) * noiseStrength * amp;
    }

    float SquareWave(float x, float time, float amp, float freq)
    {
        float s = Mathf.Sin(freq * x + time);
        return amp * Mathf.Sign(s);
    }

    float TriangleWave(float x, float time, float amp, float freq)
    {
        float s = Mathf.Sin(freq * x + time);
        return amp * (Mathf.Abs(s) * 2f - 1f);
    }

    
    float CustomWave1(float x, float time, float amp, float freq)
    {
        return amp * Mathf.Sin(freq * x + time * customA) *
                     Mathf.Cos(freq * customB * x);
    }

    float CustomWave2(float x, float time, float amp, float freq)
    {
        float n = Mathf.PerlinNoise(x * customC, time * customD);
        return amp * (n - 0.5f) * 2f;
    }
}
