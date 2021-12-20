using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PathCreation.Examples
{
    public class SpawnController : MonoBehaviour
    {
        //public Enemy rock1;
        public float boundaryHorizontal = 9.0f;
        public GameObject enemiesContainer; 
        // public float rock1Rate = 3.0f;
        // public int quantity = 1; 
        // private float lastRockTime = 0.0f;
        public float spawnPositionY = 8.0f;
        //  private int quantitySpawned = 0;
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
        }



        private void Start()
        {
            initTime = Time.time;
            LaunchWave(waves[0]); 
        }
        private void Update()
        {





            /*  if(GameTime() - lastRockTime >= rock1Rate && quantitySpawned < quantity && rock1)
            {
                quantitySpawned++; 
                Enemy rockInstance = Instantiate(rock1);
                rockInstance.transform.position = new Vector3(
                    Random.Range(-boundaryHorizontal, boundaryHorizontal),spawnPositionY,0);
                lastRockTime = GameTime();
            }*/
        }

        void   LaunchWave(Wave wave)
        {
            IEnumerator coroutine;
            coroutine = LaunchWaveCoroutine(wave);
            StartCoroutine(coroutine);  

            
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
                        spawnPositionY, 0) ; 
                    if(wave.movementType == Wave.MovementType.followPath)
                    {
                        PathFollower pathFollower = enemy.gameObject.AddComponent<PathFollower>();
                        pathFollower.applyRotation = false;
                        pathFollower.pathCreator = wave.path;
                        pathFollower.speed = wave.speed; 
                        pathFollower.endOfPathInstruction = wave.endOfPathInstruction;
                        

                    }
                    yield return new WaitForSeconds(wave.rate);
                }
            }
           
            yield return null; 
        }
    }
}