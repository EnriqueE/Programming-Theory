using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponTriggerType weaponTriggerType;
    public GameObject bulletPrefab;
    public int damage;
    public float bulletSpeed;
    public float bulletDelay = 0.1f;
    // When weaponTriggerType is set to always maxBullets act as a max amount
    public int maxBullets = 0;
    private int bulletsFired = 0; 
    public float startDelay = 0.0f; 

    // Dynamic Pool of objects, starts at amounToPool but increases if necessary
    private List<GameObject> pooledBullets;
    public int amounToPool;
    private GameObject pool;

    private float lastBulletTime;
    private bool fromPlayer = false;
    private float initTime = 0.0f; 

    public enum WeaponTriggerType
    {       
        onFireKey,
        always,
        manual
    }

    private void Start()
    {

        initTime = Time.time; 
        // Check if weapon is attachet to the player
        if (transform.root.GetComponent<PlayerController>())
        {
            fromPlayer = true; 
        }

        // Create pool of bullets
        
        CreateBulletPool();
        
    }
    public virtual void CreateBulletPool()
    {
        pool = GameObject.Find("Pool");
        pooledBullets = new List<GameObject>();

        for (int i = 0; i < amounToPool; i++)
        {
            AddBulletToPool();
        }
    }
    private void AddBulletToPool()
    {
        GameObject bulletInstance;
        bulletInstance = Instantiate(bulletPrefab, pool.transform);
        bulletInstance.name = "Bullet from " + gameObject.name + " of " + gameObject.transform.root.name;
        bulletInstance.GetComponent<Bullet>().fromPlayer = fromPlayer;
        bulletInstance.GetComponent<Bullet>().SetBulletSpeed(bulletSpeed);
        bulletInstance.GetComponent<Bullet>().SetBulletDamage(damage);
        bulletInstance.SetActive(false);
        bulletInstance.transform.Translate(gameObject.transform.position);
        pooledBullets.Add(bulletInstance);
    }
    private void Update()
    {
        if (Time.time - lastBulletTime > bulletDelay)
        {
            if ( (Input.GetKey(KeyCode.Space) && weaponTriggerType == WeaponTriggerType.onFireKey) ||
                (weaponTriggerType == WeaponTriggerType.always &&
                Time.time - initTime > startDelay &&
                (bulletsFired < maxBullets || maxBullets==0)))
            {            
                FireBullet();
            }
        }
    }
    public void FireBullet()
    {
        
        GameObject bullet = GetPooledBullet();
        if (bullet == null)
        {
            AddBulletToPool();
            amounToPool++;
            bullet = GetPooledBullet();
        }
        lastBulletTime = Time.time;
        bullet.SetActive(true);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = gameObject.transform.rotation;
        bulletsFired++; 
        
        
    }
    public GameObject GetPooledBullet()
    {
        for(int i =0; i < amounToPool; i++)
        {
            if(!pooledBullets[i].activeInHierarchy)
            {
                return pooledBullets[i];    
            }
        }
        return null; 
    }
}
