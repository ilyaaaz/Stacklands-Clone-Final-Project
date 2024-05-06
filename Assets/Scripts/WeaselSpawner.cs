using System.Collections;
using UnityEngine;

public class WeaselSpawner : MonoBehaviour
{
    public GameObject weaselPrefab;
    public float minSpawnDelay = 5f;
    public float maxSpawnDelay = 15f;
    private Camera mainCam;
    private bool hasSpawned = false;

    void Start()
    {
        mainCam = Camera.main;
        StartCoroutine(SpawnWeasel());
    }

    IEnumerator SpawnWeasel()
    {
        while (!hasSpawned)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            if (GameManager.instance.currentState == GameManager.STATE.Stop && !GameManager.instance.duringFeed)
            {
                Vector3 spawnPosition = GetSpawnPosition();
                GameObject weaselInstance = Instantiate(weaselPrefab, spawnPosition, Quaternion.identity);
                hasSpawned = true; //prevent further spawning
            }
        }
    }

    Vector3 GetSpawnPosition()
    {
        float yPos = Random.Range(-5.7f, 3.2f); //ensure it spawns within vertical bounds
        float xPos = mainCam.ViewportToWorldPoint(new Vector3(1.1f, 0, 0)).x; //start just outside the right of the camera view
        return new Vector3(xPos, yPos, 0);
    }
}
