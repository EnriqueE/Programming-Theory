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
    [System.Serializable]
    public struct EnemyData
    {
        public string name;
        public Enemy prefab;
        public int health;
        [Header("Weapons")]
        public bool updateWeaponData;
        public WeaponData weaponData;

    }
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
        GameController.instance.AddScore(1); 
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
            GameObject.Find("SpawnController").GetComponent<SpawnController>().DeathOnWaveNumber(fromWaveNumber, gameObject); 
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
        GameController.instance.AddScore(health);
        GameController.instance.Log("Enemy Down: " + name + "(" + health + ")");
        // Audio Explision
        if (explosionClip) { 
            AudioController.instance.Play(explosionClip);
        }

        // Explosion Particle system
        
        if (explosionPS && explosionPS.GetComponent<ParticleSystem>())
        {
            explosionPS.transform.parent = pool.transform;
            explosionPS.SetActive(true);
            InfiniteDown infiniteDown = explosionPS.AddComponent<InfiniteDown>();
            infiniteDown.speed = 0.5f; 
            
        }        
        SilentDeath();
    }
    public void UpdateWeaponData(WeaponData weaponData)
    {     
        foreach (Weapon weapon in GetComponentsInChildren<Weapon>(true))
            weapon.UpdateWeaponData(weaponData); 
        
    }

  
}
