using UnityEngine;
using System.Collections;

public class CanvasController : MonoBehaviour {

	GameObject Menu;
	GameObject Pause;

	void OnEnable()
	{
		EventManager.OnStateActive += OnStateActive;
	}

	void OnDisable()
	{
		EventManager.OnStateActive -= OnStateActive;
	}

	void Start ()
	{
		Menu = transform.FindChild("MENU").gameObject;
		Pause = transform.FindChild("PAUSE").gameObject;


	}




	void OnStateActive(State s)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}

		if (transform.FindChild(s + "") != null)
		{
			transform.FindChild(s + "").gameObject.SetActive(true);
		}
	}
}
