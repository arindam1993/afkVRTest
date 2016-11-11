using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoatStateManager : MonoBehaviour {

    public int Index;

    public PirateStateManager[] Pirates;

    public int AvailablePirates
    {
        get
        {
            return _inactivePirates.Count;
        }
    }

    private HashSet<PirateStateManager> _activePirates;
    private HashSet<PirateStateManager> _inactivePirates;


    void Awake()
    {
        _activePirates = new HashSet<PirateStateManager>();
        _inactivePirates = new HashSet<PirateStateManager>();
    }

    void Start()
    {
        foreach(PirateStateManager p in Pirates)
        {
            _inactivePirates.Add(p);
        }
    }

    public void ActivateRandomPirate()
    {
        int numInactive = _inactivePirates.Count ;
        if (numInactive > 0)
        {
            int rand = Random.Range(0, numInactive - 1);
            int p_i = 0;
            PirateStateManager randP = null;
            foreach (PirateStateManager p in _inactivePirates)
            {
                if ( p_i == rand)
                {
                    randP = p;
                    break;
                }
                p_i++;
            }

            _inactivePirates.Remove(randP);
            _activePirates.Add(randP);

            randP.Activate();
        }
        
    }

    public void DeactivateAll()
    {
        foreach (PirateStateManager p in _activePirates)
        {
            p.DeactivateNoCb();
        }
        _activePirates.Clear();
        foreach (PirateStateManager p in Pirates)
        {
            _inactivePirates.Add(p);
        }

    }


    public void PirateDeactivated(PirateStateManager pirate)
    {
        if (_activePirates.Remove(pirate))
        {
            _inactivePirates.Add(pirate);
        }else
        {
            Debug.LogWarning("Deactivating pirate not in active set");
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivateRandomPirate();
        }
    }

}
