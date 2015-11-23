using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the enemy spawner
/// </summary>
public class EnemySpawner : PauseableObjectScript
{
    #region Fields

    [SerializeField]GameObject enemyRanger;
    [SerializeField]GameObject enemyMage;
    [SerializeField]GameObject enemyWarrior;
    [SerializeField]Transform spawnLocation;
    Timer spawnTimer;

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected void Start()
    {
        Initialize();
        spawnTimer = new Timer(Constants.AI_MIN_SPAWN_TIME);
        spawnTimer.Register(SpawnEnemy);
        spawnTimer.Start();
    }

    /// <summary>
    /// Updates the object when it isn't paused
    /// </summary>
    protected override void NotPausedUpdate()
    {
        spawnTimer.Update();
    }

    /// <summary>
    /// Spawns an enemy
    /// </summary>
    protected void SpawnEnemy()
    {
        GameObject spawn = enemyRanger;
        int rand = Random.Range(0, 3);
        if (rand == 1)
        { spawn = enemyMage; }
        else if (rand == 2)
        { spawn = enemyWarrior; }
        Instantiate(spawn, spawnLocation.position, transform.rotation);

        // Resets the spawn timer
        spawnTimer.TotalSeconds = Random.Range(Constants.AI_MIN_SPAWN_TIME, Constants.AI_MAX_SPAWN_TIME);
        spawnTimer.Start();
    }

    #endregion
}
