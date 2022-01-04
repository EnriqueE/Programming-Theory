using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;

        [Header("Speed")]
        public float speed = 5;
        [SerializeField] private int speedMultiplier = 100;
        public bool enableSpeedCurve = false;        
        public AnimationCurve speedCurve;
        
        [Space(10)]
        public float time; 
        public bool applyRotation = false;
        public Vector3 offsetRotation;
        
        float distanceTravelled;
        float currentTime = 0;
        float initTime;
        

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }
        void onEnable()
        {
            initTime = Time.time; 

        }

        void Update()
        {
            if (pathCreator != null)
            {
                if (speedCurve != null && enableSpeedCurve)
                {
                    float currentTime = Time.time - initTime;
                    float factor = currentTime / time > 1 ? 1 : currentTime / time;
                    float m_speed = speedCurve.Evaluate(factor);
                    distanceTravelled += m_speed * speed / speedMultiplier;

                }
                else
                {
                    // float factor = 0.3f; 
                    // speed = AnimationCurve.Linear(speedCurve.Evaluate(factor),);
                    //Debug.Log("Disatnce traveled: " + distanceTravelled); 
                    distanceTravelled += speed * Time.deltaTime;
                    //BezierPath beizerPath = new BezierPath(); 
                }
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                if (applyRotation) 
                {
                    transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                    transform.Rotate(offsetRotation);
                }
            }
        }
        void Update_old()
        {
            if (pathCreator != null)
            {
                if (speedCurve != null && enableSpeedCurve)
                {                
                    float currentTime = Time.time - initTime;
                    float factor = currentTime / time > 1 ? 1 : currentTime / time;
                    float m_speed = speedCurve.Evaluate(factor);
                    distanceTravelled += m_speed * speed;
                  //  Debug.Log(gameObject.name + " -> f: " + factor + " m_speed: " + m_speed + " d: " + distanceTravelled);
                }
                else
                {
                    bool speedCurveBool = speedCurve != null ? true : false;
                    Debug.Log("speedCurve:!=null: " + speedCurveBool.ToString() + " enableSpeedCurve: " + enableSpeedCurve); 
                    distanceTravelled += speed  * Time.deltaTime; 
                }
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                if (applyRotation)
                {
                    transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                    transform.Rotate(offsetRotation);
                }
            }            
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}