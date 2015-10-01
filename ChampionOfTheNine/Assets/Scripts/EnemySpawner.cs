using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the enemy spawner
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    #region Fields

    [SerializeField]GameObject enemyRanger;

    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 1, 5);
    }

    /// <summary>
    /// Spawns an enemy
    /// </summary>
    private void SpawnEnemy()
    {
        Instantiate(enemyRanger, transform.position, transform.rotation);
    }

    #endregion
}
