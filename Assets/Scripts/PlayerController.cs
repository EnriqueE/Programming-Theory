using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;

    public float maxAngleRotation = 0.4f;
    public float returnAngleSpeed = 0.05f; 
    public float rotationSpeed;
    private Vector3 rotation;
    private float minRotationYToRestart = 0.0001f; 

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        

        if (horizontalInput != 0 || transform.rotation.y != 0)
        {
            if(horizontalInput == 0)
            {
                horizontalInput = transform.rotation.y > 0 ? returnAngleSpeed : -returnAngleSpeed;               
            }
            if ((transform.rotation.y < maxAngleRotation && horizontalInput < 0) ||
                (transform.rotation.y > -maxAngleRotation && horizontalInput > 0))
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * -horizontalInput);
                if(transform.rotation.y <= minRotationYToRestart && 
                    transform.rotation.y >= -minRotationYToRestart)
                {
                    transform.rotation = Quaternion.identity;
                }
               


            }

        }       
     
        

    }
}
