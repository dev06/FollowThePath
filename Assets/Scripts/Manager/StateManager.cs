using UnityEngine;
using System.Collections;


public enum State
{
	NONE,
	GAME,
	MENU,
	PAUSE,
}


public class StateManager : MonoBehaviour {




	public static StateManager Instance;
	public State gameState;

	void Awake()
	{
		Instance = this;

	}

	void Start ()
	{
		SetState(State.MENU);
	}


	public void SetState(State s)
	{
		gameState = s;
		if (EventManager.OnStateActive != null)
		{
			EventManager.OnStateActive(s);
		}
	}
}
