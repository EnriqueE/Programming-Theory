using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    public ParticleSystem ps;
    public ParticleSystem trailPS; 
    public int emissionMainRate = 40;
    public int emissionTrailRate = 3; 
    public Light light;
    

    private float inputX;
    private float inputY;

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical"); 

        if(inputY > 0 )
        {   
            light.gameObject.SetActive(true);
            ps.emissionRate = emissionMainRate;
            trailPS.emissionRate = emissionTrailRate; 
            
            
        } else 
        {
            if (light.gameObject.activeSelf)
            {
                light.gameObject.SetActive(false);
                ps.emissionRate = 0;
                trailPS.emissionRate = 0; 
            }
                
        }

    }
}
