using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPos : MonoBehaviour
{

    public Vector3 newPos;
    private Vector3 initPosition; 
    public float totalTime;
    private float initTime;
    private float currentTime;
    public bool enableAtStart = true;
    public bool disableAtFinish = false; 
    public bool returnAtFinish = false; 


    private bool isMoving = false; 
    public enum EassingEffects
    {
        none,
        cubicEaseInOut,
        cubicEaseIn,
        cubicEaseOut
    }
    public EassingEffects eassingEffect; 
   
    void Update()
    {
        if (isMoving)
        {
            currentTime += Time.deltaTime / totalTime;
            float factor = currentTime;  
            switch(eassingEffect)
            {
                case EassingEffects.none:
                    factor = currentTime;
                    break;
                case EassingEffects.cubicEaseIn:
                    factor = (float)Cubic.EaseIn(currentTime, 0, 1, 1);
                    break;
                case EassingEffects.cubicEaseOut:
                    factor = (float)Cubic.EaseOut(currentTime, 0, 1, 1);
                    break;
                case EassingEffects.cubicEaseInOut:
                    factor = (float)Cubic.EaseInOut(currentTime, 0, 1, 1);
                    break; 

            }
            //float factor = (float)Cubic.EaseInOut(currentTime, 0, 1, 1);


            if (currentTime >= 1)
            {
                isMoving = false;
                if(disableAtFinish)
                {
                    gameObject.SetActive(false); 
                }
                if(returnAtFinish)
                {
                    transform.position = initPosition;
                    //Debug.Log("Returning to " + initPosition); 
                } else
                {
                    transform.position = newPos;
                } 
                
            } else
            {
                transform.position = Vector3.Lerp(initPosition, newPos, factor);
            }
            
        }
    }
    public void StartMoving(Vector3 newPosition, float time)
    {
        if(enableAtStart)
        {
            gameObject.SetActive(true); 
        }
        currentTime = 0;         
        newPos = newPosition;
        initTime = Time.time; 
        totalTime = time; 
        initPosition = transform.position;
        isMoving = true;
        //Debug.Log("Moving " + transform.name + " from " + initPosition + " to " + newPosition + " in " + time + " seconds."); 

    }
    public void StartMoving(Vector3 newPosition, float time, EassingEffects _eassingEffect)
    {
        eassingEffect = _eassingEffect;
        StartMoving(newPosition, time);
    }

}
