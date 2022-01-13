using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;
using PathCreation.Examples;

public class SpawnController : MonoBehaviour
 {
    [SerializeField] private float boundaryHorizontal = 9.0f;
    [SerializeField] private GameObject enemiesContainer;
    [SerializeField] private float spawnPositionY = 8.0f;
    private PlayerController playerController;
    [Header("Rewards")]
    [Space(10)]
    [Header("Waves")]
    public Wave[] waves;
    private int currentWave = 0;


   
    [Serializable]
    public struct Wave
    {
        public enum MovementType { infiniteDown, followPath, fromPositionToPosition }
        public enum RewardType { none, random, weaponReward, healthReward }
        public int id { get; set; }
        public string name;
        public bool enabled;               

        [Tooltip("All Enemies in the wave, in order")]
        public Enemy.EnemyData[] enemies;
        [Tooltip("Randomize between all elements of Enemies")]
        public bool randomEnemies;
        public int currentEnemy { get; set; }        
        public MovementType movementType;
        public Vector3 fromPosition;
        public Vector3 toPosition;
        public float fromPosToPosTime; 
        public float infiniteDownSpeed;
        public PathCreator path;
        public EndOfPathInstruction endOfPathInstruction;
        public RewardType reward; 
        public int quantity;
        public float initDelay;
        public float rate;
        public float speed;
        public float totalTime { get { return initDelay + (rate * quantity); } }            
        public UnityEvent methodAtTotalLaunched;
        public UnityEvent methodAtAllDestroyed;
        
        public int deaths { get; set; }
    }

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>(); 
        LaunchNextWave();
        InitializeWaves();
    }
        
    public void InitializeWaves()
    {
        // set id 
        for(int i = 0; i < waves.Length; i++)
        {
            waves[i].id = i;
            waves[i].deaths = 0; 
        }
            
    }
    void LaunchWave(Wave wave)
    {

        IEnumerator coroutine;
        coroutine = LaunchWaveCoroutine(wave);
        StartCoroutine(coroutine);             
    }
    public void LaunchNextWave()
    {
        if(currentWave < waves.Length)
        {
            LaunchWave(waves[currentWave++]);
        } else
        {
            Debug.Log("Completed all Waves"); 
        }
    }
    private IEnumerator LaunchWaveCoroutine(Wave wave)
    {
        // Skip Wave if quantity = 0  or not enemies added or disabled
        if(wave.quantity > 0 && wave.enabled && wave.enemies.Length > 0)
        {
            if (wave.initDelay > 0)
            {
                yield return new WaitForSeconds(wave.initDelay);
            }
            for (int i = 0; i < wave.quantity; i++)
            {
                int enemyIndex;
                if (wave.randomEnemies)
                {
                    enemyIndex = UnityEngine.Random.Range(0, wave.enemies.Length);
                }
                else
                {
                    enemyIndex = wave.currentEnemy;
                    wave.currentEnemy++;
                    if(wave.currentEnemy >= wave.enemies.Length)
                    {
                        wave.currentEnemy = 0; 
                    }

                }
                
                Enemy enemy = Instantiate(wave.enemies[enemyIndex].prefab, enemiesContainer.transform);

                enemy.gameObject.name = "Enemy " + i + " from wave " + wave.id + " (" + enemy.gameObject.name + ")";
                enemy.transform.position = new Vector3(
                    UnityEngine.Random.Range(-boundaryHorizontal, boundaryHorizontal),
                    spawnPositionY, 0);
                enemy.transform.rotation = wave.enemies[enemyIndex].prefab.transform.rotation;
                enemy.fromWaveNumber = wave.id;
                enemy.health = wave.enemies[enemyIndex].health; 
                if(wave.enemies[enemyIndex].updateWeaponData)
                {
                    enemy.UpdateWeaponData(wave.enemies[enemyIndex].weaponData); 
                }



                if(wave.movementType == Wave.MovementType.followPath)
                {
                    PathFollower pathFollower = enemy.gameObject.AddComponent<PathFollower>();
                    pathFollower.applyRotation = false;
                    pathFollower.pathCreator = wave.path;
                    pathFollower.speed = wave.speed; 
                    pathFollower.endOfPathInstruction = wave.endOfPathInstruction;
                } else if (wave.movementType == Wave.MovementType.infiniteDown)
                {
                    InfiniteDown infiniteDown = enemy.gameObject.AddComponent<InfiniteDown>();
                    infiniteDown.speed = wave.speed;
                
                } else if (wave.movementType == Wave.MovementType.fromPositionToPosition)
                {
                    MoveToPos moveToPos = enemy.gameObject.AddComponent<MoveToPos>();
                    enemy.transform.position = wave.fromPosition; 
                    moveToPos.StartMoving(wave.toPosition, wave.fromPosToPosTime, MoveToPos.EassingEffects.cubicEaseOut);
                }
                yield return new WaitForSeconds(wave.rate);
            }
        } else
        {
            Debug.Log("Wave " + wave.id + " skipped."); 
        }
        GameController.instance.Log("Wave " + wave.name + " completly launched."); 
        if(wave.methodAtTotalLaunched.GetPersistentEventCount()>0)
        {
            wave.methodAtTotalLaunched.Invoke(); 
        } 
        yield return null; 
    }
    public void DeathOnWaveNumber(int waveNumber, GameObject lastObject)
    {
        waves[waveNumber].deaths++;
        if(waves[waveNumber].deaths == waves[waveNumber].quantity && waves[waveNumber].quantity > 0)
        {

            RewardController.CreateReward(waves[waveNumber].reward, lastObject.transform);
            GameController.instance.Log("Killed wave " + waves[waveNumber].name); 
            if(waves[waveNumber].methodAtAllDestroyed.GetPersistentEventCount() > 0 )
            {
                waves[waveNumber].methodAtAllDestroyed.Invoke();
            }
        }
    }
    
}
    
