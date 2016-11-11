using UnityEngine;
using System.Collections;

public class PirateStateManager : MonoBehaviour {



    public Transform ShootPoint { get; private set; }

    public Transform Target;

    public GameObject Fireball;

    public float ShootInterval;

    [Range(30,60)]
    public float ThrowAngle;

    public AudioClip throwEffect;
    public AudioClip onRiseEffect;
    public AudioClip onKillEffect;

    private AudioSource audSrc;

    private BoxCollider _collider;

    private Animator _anim;

    private GameObject _pirateGO;

    private BoatStateManager _boat;

    #region STATE_MACHINE_OBJECTS
    private SimpleStateMachine _sm;
    private SimpleState _inactive;
    private SimpleState _rising;
    private SimpleState _shooting;
    private SimpleState _dead;
    #endregion

    void Awake()
    {
        _sm = new SimpleStateMachine();
        _inactive = new SimpleState(_inactiveStart, _inactiveUpdate, _inactiveEnd, "[ PIRATE ] : Inactive");
        _rising = new SimpleState(_risingStart, _risingUpdate, _risingEnd, "[ PIRATE ] : Rising");
        _shooting = new SimpleState(_shootingStart, _shootingUpdate, _shootingEnd, "[ PIRATE ] : Shooting");
        _dead = new SimpleState(_deadStart, _deadUpdate, _deadEnd, "[ PIRATE ] : Dead");
    }


    // Use this for initialization
    void Start () {

        ShootPoint = transform.Find("ShootPoint");
        _pirateGO = transform.Find("Pirate_Geometry").gameObject;
        _anim = GetComponent<Animator>();
        _collider = _pirateGO.GetComponent<BoxCollider>();
        Target = GameObject.Find("Target").transform;
        _boat = transform.parent.parent.GetComponent<BoatStateManager>();
        _sm.SwitchStates(_inactive);
        audSrc = GetComponent<AudioSource>();
        SimplePool.Preload(Fireball, 20);
	}

    #region INACTIVE_STATE

    void _inactiveStart() {
        _collider.enabled = false;
        
        //_anim.SetTrigger("Deactivate");
    }

    void _inactiveUpdate() { }

    void _inactiveEnd() { }

    #endregion

    #region RISING_STATE

    void _risingStart() {
        _anim.SetTrigger("Activate");
        _collider.enabled = true;
        audSrc.PlayOneShot(onRiseEffect);
    }

    void _risingUpdate() { }

    void _risingEnd() { }

    #endregion

    #region SHOOTING_STATE

    float _intervalCtr;

    void _shootingStart() {
        _anim.SetTrigger("Shoot");
        _intervalCtr = 3;
    }

    void _shootingUpdate() {

        _intervalCtr += Time.deltaTime;

        if( _intervalCtr > ShootInterval)
        {
            _shoot();
            _intervalCtr = 0;
        }
    }

    void _shootingEnd() { }

    #endregion

    #region DEAD_STATE

    void _deadStart() {
        _anim.SetTrigger("Die");
        _collider.enabled = false;
        audSrc.PlayOneShot(onKillEffect);
    }

    void _deadUpdate() { }

    void _deadEnd() {

    }

    #endregion


    // Update is called once per frame
    void Update () {
        _sm.Execute(); 
    }

    //State machine controller methods
    public void Activate()
    {
        _sm.SwitchStates(_rising);
    }

    public void Deactivate()
    {
        _sm.SwitchStates(_inactive);
        _boat.PirateDeactivated(this);
    }

    public void DeactivateNoCb()
    {
        _sm.SwitchStates(_inactive);
        _anim.SetTrigger("Die");
    }

    public void StartShooting()
    {
        _sm.SwitchStates(_shooting);
    }

    //Math for velocity calculation for the projectile
    private void _shoot(){
        GameObject fireballGo = SimplePool.Spawn(Fireball, ShootPoint.position, ShootPoint.rotation);
        Fireball fireball = fireballGo.GetComponent<Fireball>();
        fireball.OnSpawned();

        ThrowAngle = (float)Random.Range(30, 60);

        float D = Vector2.Distance(new Vector2(ShootPoint.position.x, ShootPoint.position.z), new Vector2(Target.position.x, Target.position.z));
        float H = Target.position.y - ShootPoint.position.y;
        float th = Mathf.Deg2Rad*ThrowAngle;

        Vector3 toTarget = Vector3.Normalize(Target.position - ShootPoint.position);
        Quaternion rot = Quaternion.LookRotation(toTarget, Vector3.up) * Quaternion.AngleAxis(-ThrowAngle, Vector3.right);

        Vector3 throwVec =  rot * Vector3.forward ;
        float time = Mathf.Sqrt(2*(D*Mathf.Tan(th) - H)/10);
        float v = D / (time * Mathf.Cos(th));
        Debug.DrawLine(ShootPoint.position, ShootPoint.position + throwVec);

        fireballGo.GetComponent<Rigidbody>().velocity = throwVec * v; 
        
        Debug.Log("Shot bullet "+time +", "+ v);
        audSrc.PlayOneShot(throwEffect);
       // Debug.Break();
    }

    public void OnShotByPlayer()
    {
        _sm.SwitchStates(_dead);
    }
}
