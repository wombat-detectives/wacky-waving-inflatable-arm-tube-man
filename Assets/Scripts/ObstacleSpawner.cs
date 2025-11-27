using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    //if we decide to use obstacles use this, but issues with blocking player and reflecting player seems to make it not worth it
    [Header("References")]
    public SinWave wave;             
    public Transform player;
    public GameObject[] spawnPrefabs; // enemies, obstacles, anything here

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;    // time between spawns
    public float spawnAheadDistance = 8f; // how far in front of player to spawn
    public float spawnHeightOffset = 1.5f;// how much above the wave
    public Vector2 randomOffsetY = new Vector2(0f, 1f); //randomness

    private float timer = 0f;

    void Update()
    {
        if (wave == null || player == null || spawnPrefabs.Length == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        // Pick a point ahead of the player
        float spawnX = player.position.x + spawnAheadDistance;

        // Get the wave height at that X
        float waveY = wave.GetSurfaceHeightAtX(spawnX);

        // Spawn above the wave
        float spawnY = waveY + spawnHeightOffset + Random.Range(randomOffsetY.x, randomOffsetY.y);

        // Pick a random prefab from the list
        GameObject prefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];

        // Spawn it
        Instantiate(prefab, new Vector3(spawnX, spawnY, 0f), Quaternion.identity);
    }
}
