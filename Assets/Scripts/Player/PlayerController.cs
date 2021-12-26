using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float maxAngleRotation = 0.4f;
    public float returnAngleSpeed = 0.05f; 
    public float rotationSpeed;
    public Vector2 speed = new Vector2(1, 1);

    [Header("Baundaries")]
    public float boundaryTop = 5.0f;
    public float boundaryBottom = -3.0f; 
    public float boundaryHorizontal = 9.0f;
    [Space(10)]


    private float horizontalInput;
    private float verticalInput;
    private Vector3 rotation;
    private float minRotationYToRestart = 0.001f; 

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        
        HandleRotation();
        HandleMovement(); 

    }
    private void HandleRotation()
    {
        float m_horizontalInput = horizontalInput; 
        if (horizontalInput != 0 || transform.rotation.y != 0)
        {
            if (horizontalInput == 0)
            {
                m_horizontalInput = transform.rotation.y > 0 ? returnAngleSpeed : -returnAngleSpeed;
            }
            if ((transform.rotation.y < maxAngleRotation && m_horizontalInput < 0) ||
                (transform.rotation.y > -maxAngleRotation && m_horizontalInput > 0))
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * -m_horizontalInput);
                if (transform.rotation.y <= minRotationYToRestart &&
                    transform.rotation.y >= -minRotationYToRestart)
                {
                    transform.rotation = Quaternion.identity;
                }
            }
        }
    }
    public void Hit(int damage)
    {
        //Debug.Log("Player hit by " + bullet.gameObject.name); 
    }
    private void HandleMovement()
    {
        
        if(horizontalInput != 0 || verticalInput != 0) 
        {
            transform.Translate(
                Time.deltaTime * speed.x * horizontalInput, 
                Time.deltaTime * speed.y * verticalInput, 
                0,Space.World);
        }
        transform.position = new Vector3(
            Mathf.Max(-boundaryHorizontal, Mathf.Min(boundaryHorizontal,transform.position.x)),
            Mathf.Max(boundaryBottom, Mathf.Min(boundaryTop, transform.position.y)),
            transform.position.z);
    }
}
