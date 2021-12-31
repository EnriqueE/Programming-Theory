using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public int health = 100;
    public float maxAngleRotation = 0.4f;
    public float returnAngleSpeed = 0.05f; 
    public float rotationSpeed;
    public Vector2 speed = new Vector2(1, 1);
    private AudioSource audioSource; 
    public AudioClip hitClip;
    private UIGameController uIGameController; 

   
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
        uIGameController = GameObject.Find("UI").GetComponent<UIGameController>(); 
        audioSource = GetComponent<AudioSource>(); 
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
        if(hitClip)
        {
            audioSource.PlayOneShot(hitClip); 
        }
        GetComponent<CameraShake>().enabled = true;
        health -= damage;
        uIGameController.UpdateUIInfo();

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
        weapon.UpdatePoolProperties(weapon.damage, weapon.speed); 
        weaponData.weaponGameObject.gameObject.SetActive(true);
    }
    private void LoadWeaponLevel(int weaponLevelNumber)
    {
        // Disable all enabled Weapons of Player
        Weapon[] weapons = GetComponentsInChildren<Weapon>(true);
        foreach(Weapon weapon in weapons) { weapon.gameObject.SetActive(false); }

        // Enable weapon level weapons
        foreach(WeaponData weaponData in weaponLevels[weaponLevelNumber].weapons) { LoadWeaponData(weaponData); }
        uIGameController.UpdateUIInfo(); 
    }
    public void LoadNextWeaponLevel()
    {
        if(currentWeaponLevel < weaponLevels.Count - 1)
        {
            currentWeaponLevel++;
            LoadWeaponLevel(currentWeaponLevel);
        }
        
    }
}
