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
        private float initTime;
        private float gameTime
        {
            get
            {
                return Time.time - initTime;
            }
        }

        public Wave[] waves;
        private int currentWave = 0;


        [Serializable]

        public struct Wave
        {
            public int id { get; set; }
            public string name; 
            public enum MovementType { infiniteDown, followPath }
            public Enemy enemy;
            public PathCreator path;            
            public MovementType movementType;
            public EndOfPathInstruction endOfPathInstruction;
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
            initTime = Time.time;
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
            }
        }
        private IEnumerator LaunchWaveCoroutine(Wave wave)
        {
            // init Delay
            if (wave.initDelay > 0)
            {
                yield return new WaitForSeconds(wave.initDelay);
            }
           
            // Spawn Enemies
            if(wave.quantity > 0)
            {
                for(int i = 0; i < wave.quantity; i++)
                {
                    Enemy enemy = Instantiate(wave.enemy, enemiesContainer.transform);
                    enemy.transform.position = new Vector3(
                        UnityEngine.Random.Range(-boundaryHorizontal, boundaryHorizontal),
                        spawnPositionY, 0);
                    enemy.transform.rotation = wave.enemy.transform.rotation;
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
                    MoveToPos moveToPos = enemy.gameObject.AddComponent<MoveToPos>();
                    moveToPos.enableAtStart = true;
                    moveToPos.newPos.y = -6.0f;
                    moveToPos.totalTime = 10.0f;
                    moveToPos.StartMoving(new Vector3(enemy.transform.position.x, -6, enemy.transform.position.z), 10.0f); 
                }
                    yield return new WaitForSeconds(wave.rate);
                }
            }
            if(wave.methodAtTotalLaunched.GetPersistentEventCount()>0)
            {
                wave.methodAtTotalLaunched.Invoke(); 
            } 
            yield return null; 
        }
        public void DeathOnWaveNumber(int waveNumber)
        {
            waves[waveNumber].deaths++;
            if(waves[waveNumber].deaths == waves[waveNumber].quantity && waves[waveNumber].quantity > 0)
            {
                if (waves[waveNumber].methodAtAllDestroyed.GetPersistentEventCount() > 0)
                {
                    waves[waveNumber].methodAtAllDestroyed.Invoke();
                }
            }
        }
    }
    
