using UnityEngine;
using System.Collections;

public class BulletTracer : MonoBehaviour {

    public Vector3 TargetPoint;
    public float BulletSpeed;
    Vector3 startPos;
    bool isSpawned = false;
    public void OnSpawned()
    {
        startPos = transform.position;
        isSpawned = true;
        GetComponent<TrailRenderer>().enabled = true;
        Debug.Log("Bullet Target:" + TargetPoint);
    }

	
	// Update is called once per frame
	void Update () {
        if(isSpawned)
        {
            Vector3 moveVec = Vector3.Normalize(TargetPoint - startPos)* BulletSpeed * Time.deltaTime;
            transform.position += moveVec;
            Debug.Log(Vector3.Distance(startPos, transform.position));
            if (Vector3.Distance(startPos, transform.position) > Vector3.Distance(startPos, TargetPoint))
            {
                Debug.Log("Despaewn "+ startPos);
                GetComponent<TrailRenderer>().enabled = false;
                SimplePool.Despawn(this.gameObject);
                isSpawned = false;
            }
        }
        
	}
}
