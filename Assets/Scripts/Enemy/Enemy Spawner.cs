/*****************************************************************************
// File Name :         Enemy Spawner.cs
// Author :            Amber C. Cardamone
// Creation Date :     August 21st, 2024
//
// Brief Description : Spawns 3 enemy types at its position at random intervals on the 5 lanes.
*****************************************************************************/
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private bool _canSpawn;
    private int enemySpawn;
    private float enemySpawnCooldown;
    [SerializeField] private Transform _self;
    [SerializeField] private Transform[] _enemySpawnPoints;
    [SerializeField] private GameObject[] _enemyTypes;
    private Transform targetSpawnPoint;
    private GameObject targetEnemy;
    private int randomTarget;

    /// <summary>
    /// Spawns random enemies at 5 positions on the map.
    /// </summary>
    void Update()
    {
        // Checks if the spawning is on cooldown
        if (_canSpawn == true)
        {
            // Stops the spawning from spamming
            _canSpawn = false;
            StartCoroutine(SpawnCooldown());
            // Makes the spawn point of the enemy go between 5 random objects
            randomTarget = Random.Range(0, 5);
            // makes the spawn point selected by the randomizer be used as the Transform for the spawned enemy
            targetSpawnPoint = _enemySpawnPoints[randomTarget];
            // Makes the enemy spawned be between 3 types
            enemySpawn = Random.Range(0, 3);
            // Makes the spawned enemy be the one selected by the randomizer
            targetEnemy = _enemyTypes[enemySpawn];
            // Spawns the chosen enemy at the chosen point
            GameObject enemySpawning = Instantiate(targetEnemy, targetSpawnPoint.position, _self.rotation);
        }
    }

    /// <summary>
    /// Resets the ability to spawn enemies after a random short duration.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnCooldown()
    {
        // Chooses a random time for enemy spawns 
        enemySpawnCooldown = Random.Range(1f, 3f);
        // Makes the corouting pause for the chosen time in the randomizer
        yield return new WaitForSeconds(enemySpawnCooldown);
        // Allows another enemy to spawn
        _canSpawn = true;
    }
}
