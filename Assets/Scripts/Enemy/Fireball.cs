using UnityEngine;
using System.Collections;
using System;

public class Fireball : MonoBehaviour, IShootable {

    public float LifeTime;
    float spawnTime;
    Rigidbody rbd;
    MeshRenderer mr;
    ParticleSystem ps;
    public bool IsSpawned { get; private set; }

    AudioSource audSrc;
    public AudioClip onExplodedEffect;

    void Awake()
    {
        IsSpawned = false;
        rbd = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        ps = GetComponent<ParticleSystem>();
        audSrc = GetComponent<AudioSource>();
    }

    public void OnSpawned()
    {
        rbd.useGravity = true;
        mr.enabled = true;
        spawnTime = Time.time;
        IsSpawned = true;

        ps.loop = true;
        ps.startSpeed = -3.0f;
    }

    void FixedUpdate()
    {
        if ( Time.time - spawnTime > LifeTime)
        {
            despawn();
        }
    }	

    public void Explode()
    {
        rbd.velocity = Vector3.zero;
        rbd.useGravity = false;
        mr.enabled = false;
        ps.loop = false;
        ps.startSpeed = 20.0f;
        audSrc.PlayOneShot(onExplodedEffect);
        Invoke("despawn", 0.3f);
    }

    void despawn()
    {
        SimplePool.Despawn(this.gameObject);
        IsSpawned = false;
    }

    public void OnShot()
    {
        Explode();
    }

    public int GetScore()
    {
        return 5;
    }
}
