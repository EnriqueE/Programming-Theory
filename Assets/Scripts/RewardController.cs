using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RewardController : MonoBehaviour
{
    public bool isEnabled = true;
    public Vector2 speed = new Vector2(1f, 0.1f);
    public float horiztonalBoundary = 9.0f;
    public float bottomBoundary = -5.0f;
    public float timeAlive = 10.0f;
    public ParticleSystem collectParticleSystem;
    public float delayToDisapearWhenCollect = 0.4f;
    public float disapearingTimeWhenCollect = 0.2f;
    private bool disapearing = false;
    private float disapearingStartTime;
    private bool collected = false;
    public float appearingTime = 0.5f;
    private float rate;
    public UnityEvent methodCollectReward; 


    private float initTime;
    private Vector3 vectorMovement;



    void Start()
    {

        vectorMovement = Random.value > 0.5f ? Vector3.right : Vector3.left;
        initTime = Time.time;

    }
    private void OnTriggerEnter(Collider other)
    {
        string name = other.transform.root.name;
        if (name == "Player" && !collected)
        {
            Collect();
        }
    }
    public void Collect()
    {
        if (methodCollectReward.GetPersistentEventCount() > 0)
        {
            methodCollectReward.Invoke();
        }            
        collected = true;
        collectParticleSystem.Play();
        GetComponent<AudioSource>().Play();
        Death();

    }

    private void Death()
    {

        disapearing = true;
        disapearingStartTime = Time.time;

    }
    void Update()
    {

        if (disapearing)
        {

            if (Time.time > disapearingStartTime + delayToDisapearWhenCollect)
            {
                rate = (float)Elastic.EaseIn((Time.time - disapearingStartTime - delayToDisapearWhenCollect) / (disapearingTimeWhenCollect), 0, 1, 1);

                if (rate <= 1)
                {
                    transform.localScale = new Vector3(1 - rate, 1 - rate, 1 - rate);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (Time.time - initTime > timeAlive)
            {
                Death();
            }
            if (isEnabled)
            {
                if (Time.time < initTime + appearingTime)
                {

                    rate = (float)Cubic.EaseOut((Time.time - initTime) / appearingTime, 0, 1, 1);
                    transform.localScale = new Vector3(rate, rate, rate);
                }



                transform.Translate(vectorMovement * speed.x * Time.deltaTime);
                transform.Translate(Vector3.down * speed.y * Time.deltaTime);
                if (transform.position.x > horiztonalBoundary)
                {
                    vectorMovement = Vector3.left;
                    transform.position = new Vector3(horiztonalBoundary, transform.position.y, 0);
                }
                else if (transform.position.x < -horiztonalBoundary)
                {
                    vectorMovement = Vector3.right;
                    transform.position = new Vector3(-horiztonalBoundary, transform.position.y, 0);
                }
            }
        }
    }
    public void LoadNextPlayerWeaponLevel()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().LoadNextWeaponLevel(); 
    }
}
