using System.Collections.Generic;
using UnityEngine;

public class PowerUpsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerupSpawn;
    [SerializeField] private int[] spawnCount;
    [SerializeField] private LayerMask powerupMask;
    private List<Transform> powerupSpawnPositions;

    private void Awake()
    {
        powerupSpawnPositions = new List<Transform>();

        foreach (Transform child in transform)
        {
            powerupSpawnPositions.Add(child);
        }
    }

    private void Start()
    {
        for (int i = 0; i < powerupSpawn.Length; i++)
        {
            for (int c = 0; c < spawnCount[i]; c++)
            {
                int randomSpawn = Random.Range(0, powerupSpawnPositions.Count);

                Collider[] powerupCheck = Physics.OverlapSphere(powerupSpawnPositions[randomSpawn].position, 0.1f, powerupMask);

                if (powerupCheck.Length == 0)
                {
                    Instantiate(powerupSpawn[i], powerupSpawnPositions[randomSpawn].position, Quaternion.identity);
                }
                else
                {
                    c--;
                }
            }
        }
    }
}
