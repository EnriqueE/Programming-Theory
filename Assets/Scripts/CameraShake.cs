using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	
	// How long the object should shake for.
	
	public float shakeDuration = 0f;
	private float currentShakeDuration; 
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	
	Vector3 originalPos;
	private float initTime; 
	
	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
    public void Start()
    {
		originalPos = camTransform.localPosition;

	}
    public void OnEnable()
	{
		initTime = Time.time;
		currentShakeDuration = shakeDuration;
		
	}

	void Update()
	{
		if (currentShakeDuration > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			currentShakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			currentShakeDuration = 0f;
			camTransform.localPosition = originalPos;
			this.enabled = false; 
		}
	}
}