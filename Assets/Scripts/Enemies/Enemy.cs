using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public GameObject enemyGameObject; 
    public GameObject explosionPS;
    public AudioClip explosionClip;
    private GameObject pool;

    private AudioSource audioSource;
    public int fromWaveNumber { get; set; }
    private bool isDead = false;

    private void Awake()
    {
        fromWaveNumber = -1;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pool = GameObject.Find("Pool"); 
    }
    public void Hit(int damage)
    {
        // Reduce health of Enemy and Destroy it, when health <= 0
        health -= damage;
        if (health <= 0)
        {
            if(!isDead) Death(); 
        }
    }
    public virtual void SilentDeath()
    {
        isDead = true;
        if (fromWaveNumber >= 0)
        {
            GameObject.Find("SpawnController").GetComponent<SpawnController>().DeathOnWaveNumber(fromWaveNumber); 
        }
        DestroyPools(); 
        Destroy(gameObject);
        
    }
    private void DestroyPools()
    {
        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            if (weapon.gameObject.GetComponent<PoolController>())
            {
                weapon.gameObject.GetComponent<PoolController>().MarkToDestroy();
            }
        }

    }
    public virtual void Death()
    {
        // Audio Explision
        if (explosionClip) { 
            AudioController.instance.Play(explosionClip);
        }

        // Explosion Particle system
        
        if (explosionPS && explosionPS.GetComponent<ParticleSystem>())
        {
            explosionPS.transform.parent = pool.transform;
            explosionPS.SetActive(true);
        }        
        SilentDeath();
    }

  
}
