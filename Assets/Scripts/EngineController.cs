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
    public KeyTriggerType mainPSTrigger = KeyTriggerType.forward;
    [Space(3)]

    [Header("Trail Particle System")]
    public bool enabledTrailPS = false; 
    public ParticleSystem trailPS;
    public Material trailPSMaterial;
    public int trailPSEmissionRate = 3;
    public float trailPSParticleSize = 0.005f;
    public float trailPSParticleSpeed = 5.0f;
    public float trailPSParticleLifeTime = 0.22f;
    public KeyTriggerType trailPSTrigger = KeyTriggerType.forward;


    [Header("Light")]
    public bool enabledLight = false; 
    public Light bulletLight;
    public Color lightColor;
    public KeyTriggerType lightTrigger;
    [Space(3)]

    public KeyTriggerType initMainKeyTriggerType; 
    private float inputX;
    private float inputY;
    private bool val;
    public enum KeyTriggerType { 
        allwaysOn,
        forward, 
        backward, 
        left, 
        right };

    private void Start()
    {
        initMainKeyTriggerType = mainPSTrigger; 

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
                case KeyTriggerType.allwaysOn: val = true; break; 
                case KeyTriggerType.forward: val = inputY > 0 ? true : false; break;
                case KeyTriggerType.backward: val = inputY < 0 ? true : false; break;
                case KeyTriggerType.right: val = inputX > 0 ? true : false; break;
                case KeyTriggerType.left: val = inputX > 0 ? true : false; break;
            }
            MainPSEnabled(val);
        }
        
        if (enabledTrailPS)
        {
            switch (trailPSTrigger)
            {
                case KeyTriggerType.allwaysOn: val = true; break;
                case KeyTriggerType.forward: val = inputY > 0 ? true : false; break;
                case KeyTriggerType.backward: val = inputY < 0 ? true : false; break;
                case KeyTriggerType.right: val = inputX > 0 ? true : false; break;
                case KeyTriggerType.left: val = inputX > 0 ? true : false; break;
            }
            TrailPSEnabled(val);
        }

        if (enabledLight)
        {
            switch (lightTrigger)
            {
                case KeyTriggerType.allwaysOn: val = true; break;
                case KeyTriggerType.forward: val = inputY > 0 ? true : false; break;
                case KeyTriggerType.backward: val = inputY < 0 ? true : false; break;
                case KeyTriggerType.right: val = inputX > 0 ? true : false; break;
                case KeyTriggerType.left: val = inputX > 0 ? true : false; break;
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
