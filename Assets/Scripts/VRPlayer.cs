using UnityEngine;
using System.Collections;


public class VRPlayer : MonoBehaviour {

    public int Health { get; private set; }

    public int InitialHealth;

    public bool OnVRClickDown
    {
        get
        {
            bool res = false;
            #if UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)
                res |= GvrController.ClickButtonDown;
            #endif
            res |= Input.GetMouseButtonDown(0); //left click

            return res;
        }
    }

    public void Reset()
    {
        Health = InitialHealth;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (OnVRClickDown)
        {
            Debug.Log("Shoo Fired");
            Ray gazeRay = new Ray(transform.position, transform.forward);
            Debug.DrawRay(gazeRay.origin, gazeRay.direction * 1000);
            
            RaycastHit rayHit;
            bool isRayHit = Physics.Raycast(gazeRay, out rayHit, 1000,~(LayerMask.NameToLayer("Shootable")));

            if(isRayHit)
            {
                IShootable shotObject = rayHit.transform.GetComponent<IShootable>();
                shotObject.OnShot();
            }
            
        }



    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Fireball"))
        {
            Debug.Log("Player Hit");
            Health--;
            col.gameObject.GetComponent<Fireball>().Explode();
        }
    }
}
