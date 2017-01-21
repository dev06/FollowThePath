using UnityEngine;
using System.Collections;

public class Icon : MonoBehaviour {

	public bool interactable;
	private Animation animation;

	void Start () {

	}


	void Update () {

	}

	public void Animate(string animation, float delay)
	{
		StartCoroutine("PlayAnimation", new string[2] {animation, delay.ToString()});
	}

	IEnumerator PlayAnimation(string[] values)
	{
		yield return new WaitForSeconds(float.Parse(values[1]));
		if (animation == null)
		{
			animation = GetComponent<Animation>();
		}


		animation.Stop();
		animation.Play(values[0]);

	}
}
