using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float damage;
    public float speed;
    public float bulletDelay = 0.1f; 

    public List<GameObject> pooledBullets;
    public int amounToPool;
    private GameObject pool; 

    private float lastBulletTime;

    private void Start()
    {
        pool = GameObject.Find("Pool"); 
        pooledBullets = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amounToPool; i++)
        {
         
            tmp = Instantiate(bulletPrefab, pool.transform );
         
            tmp.SetActive(false);
            tmp.transform.Translate(gameObject.transform.position);
            pooledBullets.Add(tmp);
        }

        
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time - lastBulletTime > bulletDelay)
            {
                GameObject bullet = GetPooledBullet();
                if (bullet != null)
                {
                    lastBulletTime = Time.time;
                    bullet.SetActive(true);
                    bullet.transform.position = transform.position;
                }
            }
        }
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
