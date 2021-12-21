using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{


    [Header("Main Particle Syste")]
    public bool enabledMainPS; 
    public ParticleSystem mainPS;
    public Material mainPSMaterial;
    public int mainPSEmissionRate = 40;
    public float mainPSParticleSize = 0.005f;
    public float mainPSParticleSpeed = 5.0f;
    public float mainPSParticleLifeTime = 0.22f; 
    public keyTriggerType mainPSTrigger = keyTriggerType.forward;
    [Space(3)]

    [Header("Trail Particle System")]
    public bool enabledTrailPS = false; 
    public ParticleSystem trailPS;
    public Material trailPSMaterial;
    public int trailPSEmissionRate = 3;
    public float trailPSParticleSize = 0.005f;
    public float trailPSParticleSpeed = 5.0f;
    public float trailPSParticleLifeTime = 0.22f;
    public keyTriggerType trailPSTrigger = keyTriggerType.forward;


    [Header("Light")]
    public bool enabledLight = false; 
    public Light bulletLight;
    public Color lightColor;
    public keyTriggerType lightTrigger; 
    [Space(3)]
    

    private float inputX;
    private float inputY;
    private bool val;
    public enum keyTriggerType { 
        allwaysOn,
        forward, 
        backward, 
        left, 
        right };

    private void Start()
    {
        mainPS.startLifetime = mainPSParticleLifeTime;
        mainPS.startSpeed = mainPSParticleSpeed;
        mainPS.startSize = mainPSParticleSize;
        mainPS.GetComponent<ParticleSystemRenderer>().material = mainPSMaterial; 

        trailPS.startLifetime = trailPSParticleLifeTime;        
        trailPS.startSpeed = trailPSParticleSpeed;
        trailPS.startSize = mainPSParticleSize;
        trailPS.GetComponent<ParticleSystemRenderer>().material = trailPSMaterial;

        bulletLight.color = lightColor; 
        
        mainPS.gameObject.SetActive(enabledMainPS);
        trailPS.gameObject.SetActive(enabledTrailPS);   
        bulletLight.gameObject.SetActive(enabledLight);  
      
    }
    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

                       
        if (enabledMainPS)
        {
            switch (mainPSTrigger)
            {
                case keyTriggerType.allwaysOn: val = true; break; 
                case keyTriggerType.forward: val = inputY > 0 ? true : false; break;
                case keyTriggerType.backward: val = inputY < 0 ? true : false; break;
                case keyTriggerType.right: val = inputX > 0 ? true : false; break;
                case keyTriggerType.left: val = inputX > 0 ? true : false; break;
            }
            MainPSEnabled(val);
        }
        
        if (enabledTrailPS)
        {
            switch (trailPSTrigger)
            {
                case keyTriggerType.allwaysOn: val = true; break;
                case keyTriggerType.forward: val = inputY > 0 ? true : false; break;
                case keyTriggerType.backward: val = inputY < 0 ? true : false; break;
                case keyTriggerType.right: val = inputX > 0 ? true : false; break;
                case keyTriggerType.left: val = inputX > 0 ? true : false; break;
            }
            TrailPSEnabled(val);
        }

        if (enabledLight)
        {
            switch (lightTrigger)
            {
                case keyTriggerType.allwaysOn: val = true; break;
                case keyTriggerType.forward: val = inputY > 0 ? true : false; break;
                case keyTriggerType.backward: val = inputY < 0 ? true : false; break;
                case keyTriggerType.right: val = inputX > 0 ? true : false; break;
                case keyTriggerType.left: val = inputX > 0 ? true : false; break;
            }
            bulletLight.gameObject.SetActive(val);
        }
    }

    
    public void MainPSEnabled(bool enabled)
    {
        mainPS.emissionRate = enabled ? mainPSEmissionRate : 0; 
    }
    public void TrailPSEnabled(bool enabled)
    {
        trailPS.emissionRate = enabled ? trailPSEmissionRate : 0;        

    }
    
}
