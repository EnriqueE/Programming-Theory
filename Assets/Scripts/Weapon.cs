using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float damage;
    public float speed;
    public float bulletDelay = 0.1f;
    public float inheritWeaponRotation; 

    public List<GameObject> pooledBullets;
    public int amounToPool;
    private GameObject pool; 

    private float lastBulletTime;

    private void Start()
    {
        pool = GameObject.Find("Pool"); 
        pooledBullets = new List<GameObject>();
        GameObject bulletInstance;
        for (int i = 0; i < amounToPool; i++)
        {         
            bulletInstance = Instantiate(bulletPrefab, pool.transform );           
            bulletInstance.name = "Bullet from " + gameObject.name + " of " + gameObject.transform.root.name; 
            bulletInstance.SetActive(false);
            bulletInstance.transform.Translate(gameObject.transform.position);
            pooledBullets.Add(bulletInstance);
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
                    Debug.Log("Taking rotation from " + transform.name + " of " + transform.parent.name);
                    bullet.transform.rotation = gameObject.transform.rotation;
                    Debug.Log("rotation: " + bullet.transform.rotation);
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
