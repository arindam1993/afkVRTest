using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

    public float LifeTime;
    float spawnTime;

    public bool IsSpawned { get; private set; }

    void Awake()
    {
        IsSpawned = false;
    }

    public void OnSpawned()
    {
        spawnTime = Time.time;
        IsSpawned = true;
    }

    void FixedUpdate()
    {
        if ( Time.time - spawnTime > LifeTime)
        {
            SimplePool.Despawn(this.gameObject);
            IsSpawned = false;
        }
    }	

}
