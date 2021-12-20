using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public GameObject enemyGameObject; 
    public GameObject explosionPS;
    public AudioClip explosionClip;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  
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
        // Audio Explision
        AudioController.instance.Play(explosionClip); 

        // Explosion Particle system
        explosionPS.transform.parent = null; 
        if (explosionPS && explosionPS.GetComponent<ParticleSystem>())
        {
            explosionPS.SetActive(true);
        }        
        Destroy(gameObject);
    }
}
