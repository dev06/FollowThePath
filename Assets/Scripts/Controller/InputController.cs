using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	private Vector2 pointerDown;
	private Vector2 pointerUp;
	private float swipeTimer;
	private bool swiped = true;
	private bool canSwipe;
	private float swipeThreshold = 40.0f;
	private float swipeDelay = .55f;


	void Start ()
	{
		pointerDown = Vector2.zero;
		pointerUp = Vector2.zero;
	}

	void Update ()
	{


		if (StateManager.Instance.gameState == State.GAME)
		{
			if (swiped)
			{
				swipeTimer += Time.deltaTime;
				if (swipeTimer > swipeDelay)
				{
					canSwipe = true;
					swiped = false;
				} else
				{
					canSwipe = false;
				}
			}


			if (canSwipe && !GamePlayController.Instance.isGameOver)
			{
				DetectSwipe();
			}
		} else
		{
			pointerUp = Vector2.zero;
			pointerDown = Vector2.zero;
			swiped = true;
			canSwipe = false;
			swipeTimer = swipeDelay - .1f;
		}

	}

	/// <summary>
	/// Detects when there is a swipe
	/// </summary>
	private void DetectSwipe()
	{
		if (Input.GetMouseButtonDown(0))
		{
			pointerDown = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0))
		{
			pointerUp = Input.mousePosition;
			if (StateManager.Instance.gameState == State.GAME)
			{
				RegisterSwipe();
			}
		}
	}

	/// <summary>
	/// Registers which swipe is it
	/// </summary>
	private void RegisterSwipe()
	{


		float diff_swipe_x = pointerUp.x - pointerDown.x;
		float diff_swipe_y = pointerUp.y - pointerDown.y;
		if (Mathf.Abs(diff_swipe_x) > swipeThreshold)
		{

			if (Mathf.Abs(diff_swipe_x) > Mathf.Abs(diff_swipe_y))
			{

				if (EventManager.OnSwipe != null)
				{
					EventManager.OnSwipe(diff_swipe_x);
				}

				swiped = true;
				canSwipe = false;
				swipeTimer = 0;
			}
		} else
		{

		}
	}
}
