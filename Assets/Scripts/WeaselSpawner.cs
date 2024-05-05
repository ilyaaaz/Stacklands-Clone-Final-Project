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
        while (!hasSpawned) //ensure weasel spawns only once
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            if (GameManager.instance.currentState == GameManager.STATE.Stop)
            {
                Vector3 spawnPosition = GetSpawnPosition();
                GameObject weaselInstance = Instantiate(weaselPrefab, spawnPosition, Quaternion.identity);
                hasSpawned = true; //no further spawns
                weaselInstance.GetComponent<WeaselController>().InitializeMovement();
            }
        }
    }

    Vector3 GetSpawnPosition()
    {
        // Ensure spawn is always horizontal and offscreen
        float yPos = Random.Range(mainCam.ViewportToWorldPoint(new Vector3(0, 0, 0)).y, mainCam.ViewportToWorldPoint(new Vector3(0, 1, 0)).y);
        float xPos = Random.value > 0.5 ? mainCam.ViewportToWorldPoint(new Vector3(1.1f, 0, 0)).x : mainCam.ViewportToWorldPoint(new Vector3(-0.1f, 0, 0)).x;
        return new Vector3(xPos, yPos, 0);
    }
}
