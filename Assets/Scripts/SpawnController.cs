using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;
using PathCreation.Examples;

public class SpawnController : MonoBehaviour
 {
    public float boundaryHorizontal = 9.0f;
    public GameObject enemiesContainer;
    public float spawnPositionY = 8.0f;
    [SerializeField] private GameObject weaponRewardPrefab; 

    public Wave[] waves;
    private int currentWave = 0;
    

    [Serializable]

    public struct Wave
    {
        public enum MovementType { infiniteDown, followPath }
        public enum RewardType { none, random, weaponReward }
        public int id { get; set; }
        public string name;
        public bool enabled;               
        
        public Enemy[] enemies;
        public bool randomEnemies;
        public int currentEnemy { get; set; }        
        public MovementType movementType;
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
        //initTime = Time.time;
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
    void   LaunchWave(Wave wave)
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
         
        // Skip Wave if
        //  quantity = 0  or not enemies added or disabled
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

                Enemy enemy = Instantiate(wave.enemies[enemyIndex], enemiesContainer.transform);

                enemy.gameObject.name = "Enemy " + i + " from wave " + wave.id + " (" + enemy.gameObject.name + ")";
                enemy.transform.position = new Vector3(
                    UnityEngine.Random.Range(-boundaryHorizontal, boundaryHorizontal),
                    spawnPositionY, 0);
                enemy.transform.rotation = wave.enemies[enemyIndex].transform.rotation;
                enemy.fromWaveNumber = wave.id; 
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
                
            }
                yield return new WaitForSeconds(wave.rate);
            }
        } else
        {
            Debug.Log("Wave " + wave.id + " skipped."); 
        }
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

            CreateReward(waves[waveNumber].reward, lastObject); 
            if(waves[waveNumber].methodAtAllDestroyed.GetPersistentEventCount() > 0 )
            {
                waves[waveNumber].methodAtAllDestroyed.Invoke();
            }
        }
    }
    public void CreateReward(Wave.RewardType m_reward, GameObject m_lastObject)
    {
        GameObject prefab = null; 
        switch(m_reward)
        {
            case Wave.RewardType.weaponReward:
                prefab = weaponRewardPrefab; 
                break; 
        }
        if (prefab)
        {
            GameObject rewardInstance = Instantiate(weaponRewardPrefab);
            rewardInstance.transform.position = m_lastObject.transform.position; 
        }
        

    }
}
    
