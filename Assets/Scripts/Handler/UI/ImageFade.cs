using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class ImageFade : MonoBehaviour {


	private Image image;
	private float alpha;
	private Color color;


	void Start ()
	{
		// image = GetComponent<Image>();
		// color = image.color;
		// alpha = 1.0f;
		//StartCoroutine("Fade");
	}

	void Update ()
	{

	}

	IEnumerator Fade()
	{
		while (alpha > 0)
		{
			alpha -= Time.deltaTime;
			color.a = alpha;
			image.color = color;
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}
}
