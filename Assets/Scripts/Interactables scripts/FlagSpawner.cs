using System.Collections.Generic;
using UnityEngine;

public class FlagSpawner : MonoBehaviour
{
    [SerializeField] private int flagsToSpawn;
    [SerializeField] private GameObject flag;
    [SerializeField] private LayerMask flagMask;
    [SerializeField] private List<Transform> flagSpawnPositions;

    private void Awake()
    {
        flagSpawnPositions = new List<Transform>();

        foreach (Transform child in transform)
        {
            flagSpawnPositions.Add(child);
        }

        if (flagSpawnPositions.Count < flagsToSpawn)
            Destroy(this);
    }

    private void Start()
    {
        for (int i = 0; i < flagsToSpawn; i++)
        {
            SpawnFlag();
        }
    }

    private void SpawnFlag()
    {
        int randomSpawn = Random.Range(0, flagSpawnPositions.Count);

        Collider[] spawnCheck = Physics.OverlapSphere(flagSpawnPositions[randomSpawn].position, 0.1f, flagMask);

        if (spawnCheck.Length == 0)
            Instantiate(flag, flagSpawnPositions[randomSpawn].position, Quaternion.identity);
        else
            SpawnFlag();
    }
}
