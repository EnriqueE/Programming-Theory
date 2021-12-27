using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowTarget : MonoBehaviour
{

    public GameObject target;
    public bool followEnabled = true;
    public float rotationSpeed;
    public float delayTime;
    private float initTime; 
    private float minToFixTarget = 0.5f;
    private float step = 0.3f; 
    private float rotationSpeedMultiplier = 100;
    public UnityEvent lostTargetEvent; 


    void Update()
    {
        if (followEnabled)
        {
            if (!target)
            {
                followEnabled = false;
                if (lostTargetEvent.GetPersistentEventCount() > 0)
                {
                    lostTargetEvent.Invoke();
                }
            } else
            {
                if (Time.time - initTime > delayTime)
                {
                    Vector3 targetPosition = (target.transform.position - transform.position).normalized;
                    float targetAngle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg - 90;
                    targetAngle = targetAngle < 0 ? targetAngle + 360 : targetAngle;
                    float distance = targetAngle - transform.eulerAngles.z;

                    if (targetAngle != transform.eulerAngles.z)
                    {

                        if (Mathf.Abs(distance) < minToFixTarget)
                        {
                            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, targetAngle);
                        }
                        else
                        {
                            if (distance > 180) distance -= 360;
                            if (distance < -180) distance += 360;
                            float newAngle =
                                distance < 0 ?
                                transform.eulerAngles.z - step * Time.deltaTime * rotationSpeed * rotationSpeedMultiplier :
                                transform.eulerAngles.z + step * Time.deltaTime * rotationSpeed * rotationSpeedMultiplier;
                            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newAngle);


                        }
                    }
                   
                }
            }
           
        }
    }
    public void StartFollow(GameObject m_target)
    {
        initTime = Time.time;
        followEnabled = true;
        target = m_target;
       
    }

    public void StartFollow(GameObject m_target, float m_rotationSpeed)
    {
        rotationSpeed = m_rotationSpeed;
        StartFollow(m_target); 
    }
    public void StartFollow(GameObject m_target, float m_rotationSpeed, float m_delayTime)
    {
        delayTime = m_delayTime;
        StartFollow(m_target, m_rotationSpeed);
    }
}
