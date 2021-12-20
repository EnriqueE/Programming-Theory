using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public GameObject explosionPrefab;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Hit(Bullet bullet)
    {

        
       // Reduce health of Enemy and Destroy it, when health <= 0
        health -= bullet.damage;
        if (health <= 0)
        {
            Death(); 
        }
    }
    public virtual void SilentDeath()
    {
        Destroy(gameObject);
    }
    public virtual void Death()
    {
        explosionPrefab.transform.parent = null; 
        if (explosionPrefab && explosionPrefab.GetComponent<ParticleSystem>())
        {
            explosionPrefab.GetComponent<ParticleSystem>().Play();            
            foreach (Transform child in gameObject.transform)
            {
                if (child.GetComponent<ParticleSystem>())
                {
                    child.GetComponent<ParticleSystem>().Play();
                }
            }
        }

        if (gameObject.GetComponent<MeshRenderer>())
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        foreach (Transform child in gameObject.transform)
        {
            if (child.GetComponent<MeshRenderer>())
            {
                child.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        if (gameObject.GetComponent<CapsuleCollider>())
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        if(gameObject.GetComponent<MeshCollider>())
        {
            gameObject.GetComponent<MeshCollider>().enabled = false;
        }
        Destroy(gameObject);
    }
}
