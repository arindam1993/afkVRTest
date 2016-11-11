using UnityEngine;
using System.Collections;


public class VRPlayer : MonoBehaviour {

    public static VRPlayer Instance;
    public GameObject BulletPrefab;
    public Transform bulletSpawnPoint;

    public int Health { get; private set; }

    public int Score { get; private set; }

    public int InitialHealth;

    public AudioClip gunshotEffect;
    AudioSource audSrc;
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
    void Awake()
    {
        Instance = this;
    }

    public void Reset()
    {
        Health = InitialHealth;
        Score = 0;

    }

	// Use this for initialization
	void Start () {
        SimplePool.Preload(BulletPrefab, 20);
        audSrc = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (OnVRClickDown)
        {
            Ray gazeRay = new Ray(transform.position, transform.forward);
            Debug.DrawRay(gazeRay.origin, gazeRay.direction * 1000);
            Vector3 TargetPoint = transform.position + transform.forward * 50;
            RaycastHit rayHit;
            bool isRayHit = Physics.Raycast(gazeRay, out rayHit, 1000,~(LayerMask.NameToLayer("Shootable")));

            if(isRayHit)
            {
                IShootable shotObject = rayHit.transform.GetComponent<IShootable>();
                shotObject.OnShot();
                TargetPoint = rayHit.point;
                Score += shotObject.GetScore();
                Debug.Log("Score: " + Score);

                GameStateManager.Instance.CurrScoreUI.text = Score + "";
            }

            BulletTracer b = SimplePool.Spawn(BulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<BulletTracer>();
            b.TargetPoint.Set(TargetPoint.x, TargetPoint.y, TargetPoint.z);
            b.OnSpawned();

            audSrc.PlayOneShot(gunshotEffect);
        }



    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Fireball"))
        {
           
            Health--;
            GameStateManager.Instance.HealthUI.text = Health + "";
            Debug.Log("Player Hit "+Health);
            if (Health < 0)
            {
                GameStateManager.Instance.GameEnd();
            }
            col.gameObject.GetComponent<Fireball>().Explode();
            
        }
    }
}
