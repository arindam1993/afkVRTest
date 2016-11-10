using UnityEngine;
using System.Collections;
using System;

public class PirateShotListener : MonoBehaviour, IShootable
{
    PirateStateManager psm;

    void Start()
    {
        psm = transform.parent.GetComponent<PirateStateManager>();
    }
    public void OnShot()
    {
        psm.OnShotByPlayer();
    }

    
}
