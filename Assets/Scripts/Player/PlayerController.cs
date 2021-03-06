using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public int health = 100;
    public int currentHealth; 
    public float maxAngleRotation = 0.4f;
    public float returnAngleSpeed = 0.05f;
    public float rotationSpeed;
    public Vector2 speed = new Vector2(1, 1);
    private AudioSource audioSource;
    public AudioClip hitClip;
    public AudioClip explosionClip; 
    private UIGameController uIGameController;
    public PathCreator pathCreator;
    private PathFollower pathFollower;



    public GameObject bodyGroup;
    public GameObject weaponsGroup;
    public GameObject enginesGroup;
    public GameObject accesoriesGroup;
    public GameObject explosionsGroup;
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
    public bool startWithMaxWeaponLevel = false;
    public List<WeaponLevel> weaponLevels = new List<WeaponLevel>();
    [Space(10)]



    public float horizontalInput;
    public float verticalInput;
    private Vector3 rotation;
    private float minRotationYToRestart = 0.02f;

    private void Start()
    {
        currentHealth = health; 
        GameController.instance.SetGameState(GameController.GameState.play);
        if (startWithMaxWeaponLevel)
            currentWeaponLevel = weaponLevels.Count - 1;
       audioSource = GetComponent<AudioSource>();
        LoadWeaponLevel(currentWeaponLevel);
     

    }

    private void FixedUpdate()
    {
          horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (GameController.instance.GetGameSate() == GameController.GameState.play)
        {
            HandleRotation();
            HandleMovement();
        }


    }
    
    public void LevelUp()
    {
        MoveToPos moveToPos = gameObject.AddComponent<MoveToPos>();
        moveToPos.StartMoving(new Vector3(0, 0.88f, -8.5f), 3f, MoveToPos.EassingEffects.cubicEaseInOut);
        moveToPos.destroyComponentAtFinish = true;
        moveToPos.enableAtStart = true;

        ForceEnginesOn(EngineController.KeyTriggerType.forward);

    }
    public void ForceEnginesOn(EngineController.KeyTriggerType trigger)
    {
        EngineController[] engines = GetComponentsInChildren<EngineController>();
        foreach (EngineController engine in engines)
        {
            if (engine.mainPSTrigger == trigger)
            {
                engine.mainPSTrigger = EngineController.KeyTriggerType.allwaysOn;
            }
        }
    }
    public void ForceEnginesOff()
    {
        EngineController[] engines = GetComponentsInChildren<EngineController>();
        foreach (EngineController engine in engines)
        {
            engine.mainPSTrigger = engine.initMainKeyTriggerType;
        }
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
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y, 0);
    }
    public void Hit(int damage)
    {
        if (hitClip)
        {
            audioSource.PlayOneShot(hitClip);
        }
        GetComponent<CameraShake>().enabled = true;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
        if (!uIGameController && GameObject.Find("UI"))
        {
            uIGameController = GameObject.Find("UI").GetComponent<UIGameController>();
        }
        if (uIGameController) uIGameController.UpdateUIInfo();

    }
    private void Death()
    {
        GameController.instance.Log("?Player Down!");
        GameController.instance.SetGameState(GameController.GameState.gameOver);
        bodyGroup.SetActive(false);
        weaponsGroup.SetActive(false);
        enginesGroup.SetActive(false);
        accesoriesGroup.SetActive(false);
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particleSystems)
        {
            particle.Play();
        }
        audioSource.PlayOneShot(explosionClip); 

    }
    private void HandleMovement()
    {

       
        if (horizontalInput != 0 || verticalInput != 0)
        {
            transform.position = new Vector3(
                transform.position.x + Time.deltaTime * speed.x * horizontalInput,
                transform.position.y + Time.deltaTime * speed.y * verticalInput,
                0);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        transform.position = new Vector3(
            Mathf.Max(-boundaryHorizontal, Mathf.Min(boundaryHorizontal, transform.position.x)),
            Mathf.Max(boundaryBottom, Mathf.Min(boundaryTop, transform.position.y)),
            0);
        

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
        foreach (Weapon weapon in weapons) { weapon.gameObject.SetActive(false); }

        // Enable weapon level weapons
        foreach (WeaponData weaponData in weaponLevels[weaponLevelNumber].weapons) { LoadWeaponData(weaponData); }
        if(!uIGameController && GameObject.Find("UI"))
        {
            uIGameController = GameObject.Find("UI").GetComponent<UIGameController>();
        }
        if(uIGameController) uIGameController.UpdateUIInfo();
    }
    public void LoadNextWeaponLevel()
    {
        if (currentWeaponLevel < weaponLevels.Count - 1)
        {
            currentWeaponLevel++;
            LoadWeaponLevel(currentWeaponLevel);
        }

    }
    public void IncreaseHealth()
    {
        health++;
        currentHealth = health;
        if (!uIGameController && GameObject.Find("UI"))
        {
            uIGameController = GameObject.Find("UI").GetComponent<UIGameController>();
        }
        if (uIGameController) uIGameController.UpdateUIInfo(); 
    }

}
