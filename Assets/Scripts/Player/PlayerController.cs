using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float maxAngleRotation = 0.4f;
    public float returnAngleSpeed = 0.05f; 
    public float rotationSpeed;
    public Vector2 speed = new Vector2(1, 1);

    [Serializable]
    public struct WeaponData
    {
        public string name;
        public GameObject weaponGameObject;
        public int damage;
        public int maxElements;
        public float delay;
        public float startDelay;
        public float speed; 
    }
    [Serializable]
    public struct WeaponLevel
    {
       
        public List<WeaponData> weapons;
    }

    [Header("Baundaries")]
    public float boundaryTop = 5.0f;
    public float boundaryBottom = -3.0f; 
    public float boundaryHorizontal = 9.0f;
    [Space(10)]



    [Header("Weapon Leveling")]
    public int currentWeaponLevel = 0; 
    public List<WeaponLevel> weaponLevels = new List<WeaponLevel>(); 
    [Space(10)]



    private float horizontalInput;
    private float verticalInput;
    private Vector3 rotation;
    private float minRotationYToRestart = 0.001f;

    private void Start()
    {
        LoadWeaponLevel(currentWeaponLevel); 
    }
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
        GetComponent<CameraShake>().enabled = true;  
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
    private void LoadWeaponData(WeaponData weaponData)
    {
        Weapon weapon = weaponData.weaponGameObject.GetComponent<Weapon>();
        weapon.damage = weaponData.damage;
        weapon.delay = weaponData.delay;
        weapon.speed = weaponData.speed;
        weapon.startDelay = weaponData.startDelay;
        weapon.maxElements = weaponData.maxElements; 
        weaponData.weaponGameObject.gameObject.SetActive(true);
    }
    private void LoadWeaponLevel(int weaponLevelNumber)
    {
        // Disable all enabled Weapons of Player
        Weapon[] weapons = GetComponentsInChildren<Weapon>(true);
        foreach(Weapon weapon in weapons) { weapon.gameObject.SetActive(false); }

        // Enable weapon level weapons
        foreach(WeaponData weaponData in weaponLevels[weaponLevelNumber].weapons) { LoadWeaponData(weaponData); }


        Debug.Log("Loading Weapon Level: " + currentWeaponLevel + " weapons.Length: " + weapons.Length);

    }
    public void LoadNextWeaponLevel()
    {
        Debug.Log("Loading next weapon Level"); 
        currentWeaponLevel++;
        LoadWeaponLevel(currentWeaponLevel); 
    }
}
