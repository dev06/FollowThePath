using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
public class CameraController : MonoBehaviour {


	Transform player;
	float targetBloomThreshold;
	float targetBloomIntensity;
	float jitterPosVelX;
	float jitterPosVelY;
	float bloomVelThreshold;
	float bloomVelIntensity;

	Vector3 jitterVel;
	Vector3 jitterPos;
	Vector3 camInitLocalPos;
	GamePlayController gpc;
	BloomOptimized bloom;


	void Start ()
	{
		bloom = GetComponent<BloomOptimized>();
		targetBloomThreshold = bloom.threshold;
		targetBloomIntensity = bloom.intensity;
		gpc = GamePlayController.Instance;
		player = GameObject.FindWithTag("Entity/Player").transform;
		transform.SetParent(player);
		camInitLocalPos = transform.localPosition;
		bloom.threshold = 0;
		bloom.intensity = 2.5f;
		StartCoroutine("IntroFade");
	}

	void Update()
	{
		//Jitter(!gpc.isGameOver ? .015f : 0);
	}


	void Jitter(float intensity)
	{
		jitterVel.x = Random.Range(-intensity, intensity);
		jitterVel.y = Random.Range(-intensity, intensity);
		jitterPos.x = Mathf.SmoothDamp(jitterPos.x, jitterVel.x, ref jitterPosVelX, .01f);
		jitterPos.y = Mathf.SmoothDamp(jitterPos.y, jitterVel.y, ref jitterPosVelY, .01f);
		jitterPos.z = 0;

		transform.localPosition = camInitLocalPos + jitterPos;
	}


	IEnumerator IntroFade()
	{
		while (Mathf.Abs(targetBloomThreshold - bloom.threshold) > .01f ||
		        Mathf.Abs(targetBloomIntensity - bloom.intensity) > .01f)
		{
			bloom.threshold = Mathf.SmoothDamp(bloom.threshold, targetBloomThreshold, ref bloomVelThreshold, 0.7f);
			bloom.intensity = Mathf.SmoothDamp(bloom.intensity, targetBloomIntensity, ref bloomVelIntensity, 0.7f);

			yield return new WaitForSeconds(Time.deltaTime);
		}
	}


}
