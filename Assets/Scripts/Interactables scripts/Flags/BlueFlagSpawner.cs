using System.Collections.Generic;
using UnityEngine;

public class BlueFlagSpawner : MonoBehaviour
{
    [SerializeField] private GameObject flag;
    [SerializeField] private LayerMask flagMask;
    private List<Transform> flagSpawnPositions;
    private int forCount;

    private void OnEnable()
    {
        GameManager.spawnNewBlueFlag += SpawnFlag;
    }

    private void OnDisable()
    {
        GameManager.spawnNewBlueFlag -= SpawnFlag;
    }

    private void Awake()
    {
        flagSpawnPositions = new List<Transform>();

        foreach (Transform child in transform)
        {
            flagSpawnPositions.Add(child);
        }
    }

    private void Start()
    {
        for (forCount = 0; forCount < 3; forCount++)
        {
            SpawnFlag();
        }
    }

    private void SpawnFlag()
    {
        int randomSpawn = Random.Range(0, flagSpawnPositions.Count);

        Collider[] spawnCheck = Physics.OverlapSphere(flagSpawnPositions[randomSpawn].position, 0.1f, flagMask);

        if (spawnCheck.Length <= 0)
        {
            Instantiate(flag, flagSpawnPositions[randomSpawn].position, Quaternion.identity);
        }
        else
        {
            SpawnFlag();
        }
    }
}
