using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class Rocket : Bullet
{
    public float lifeTime = 5.0f;
    public float explosionRadius = 1.0f;
    public bool destroyOnBoundaries = false;
    public GameObject rocketPrefab; 
    public ParticleSystem fireParticleSystem;
    public ParticleSystem smokeParticleSystem; 
    private GameObject target;
    private float initSpeed;

    new void Start()
    {
        transform.rotation = Quaternion.identity;
        initSpeed = speed; 
        base.Start();
       
    }
    private void OnEnable()
    {
        FindTargetAndFollow();
    }
    public GameObject FindTarget()
    {
        GameObject m_target = null;
        GameObject parent = GameObject.Find("Enemies");
        List<GameObject> enemiesToTarget = new List<GameObject>(); 
        if (!parent) return null; 
        

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            if (child.CompareTag("Enemy") && child.activeSelf)
            {           
                enemiesToTarget.Add(child.gameObject); 
            }           
        }
        m_target = enemiesToTarget.Count > 0 ? enemiesToTarget[Random.Range(0, enemiesToTarget.Count)] : null;
        return m_target;
        
    }
    public void FindTargetAndFollow()
    {
        target = FindTarget();
        
        StartFollowing(); 
    }
    public void StartFollowing()
    {
        if (target) GetComponent<FollowTarget>().StartFollow(target);
    }
    // POLYMORPHISM
    public override void DestroyBullet()
    {
        speed = 0;
        rocketPrefab.SetActive(false);
        fireParticleSystem.Stop();
        smokeParticleSystem.Stop(); 
        StartCoroutine("DestroyBulletAfterSeconds"); 
        

    }
    IEnumerator  DestroyBulletAfterSeconds()
    {
        yield return new WaitForSeconds(1);
        speed = initSpeed;
        rocketPrefab.SetActive(true);
        gameObject.SetActive(false); 
        TryToDestroyPool();
        yield return null; 
    }


}
