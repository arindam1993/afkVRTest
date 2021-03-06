﻿using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    // Function defined between [0,1]
    public AnimationCurve spawnIntervalFunc;
    public float minSpawnInterval;
    public float maxSpawnInterval;
    public float maxRunTime;

    float runTime;
    float lastSpawnTime;
    public bool IsSpawning { get; private set; }

    public BoatStateManager[] boats;

    void Awake()
    {
        IsSpawning = false;
    }

    void Start()
    {
    }

    public void StartSpawning()
    {
        runTime = 6;
        lastSpawnTime = 0;
        IsSpawning = true;
    }
    public void DeactivateAll()
    {
        foreach(BoatStateManager boat in boats)
        {
            boat.DeactivateAll();
        }
    }

    public void StopSpawning()
    {
        IsSpawning = false;
    }

    float getCurrSpawnInterval()
    {
        return maxSpawnInterval + (minSpawnInterval - maxSpawnInterval) * spawnIntervalFunc.Evaluate(runTime/maxRunTime);
    }

    void FixedUpdate()
    {
        if (IsSpawning)
        {
            spawnUpdate();
        }
    }

    void spawnUpdate()
    {
        float spawnInterval = getCurrSpawnInterval();
        //Debug.Log("SpawnInterval " + spawnInterval);
        if(runTime - lastSpawnTime > spawnInterval)
        {
            int boatIndex = Random.Range(0, boats.Length);
            boats[boatIndex].ActivateRandomPirate();
            lastSpawnTime = runTime;
        }

        runTime += Time.fixedDeltaTime;
    }
}
