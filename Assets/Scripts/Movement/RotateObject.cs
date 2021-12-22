using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 minRotationSpeed = new Vector3(0, 0, 0);
    public Vector3 maxRotationSpeed = new Vector3(0, 0, 0);
    private Vector3 rotationSpeed; 

    private void Start()
    {
        rotationSpeed.x = minRotationSpeed.x != maxRotationSpeed.x ? Random.Range(0.0f, maxRotationSpeed.x - minRotationSpeed.x) + minRotationSpeed.x : minRotationSpeed.x;
        rotationSpeed.y = minRotationSpeed.y != maxRotationSpeed.y ? Random.Range(0.0f, maxRotationSpeed.y - minRotationSpeed.y) + minRotationSpeed.y : minRotationSpeed.y;
        rotationSpeed.z = minRotationSpeed.z != maxRotationSpeed.z ? Random.Range(0.0f, maxRotationSpeed.z - minRotationSpeed.z) + minRotationSpeed.z : minRotationSpeed.z;
        
    }
    private void Update()
    {
        gameObject.transform.Rotate(rotationSpeed * Time.deltaTime);
         
    }
}
