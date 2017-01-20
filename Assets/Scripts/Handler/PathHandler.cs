using UnityEngine;
using System.Collections;

public class PathHandler : MonoBehaviour
{

	SpriteRenderer spriteRenderer;
	GamePlayController gpc;
	StateManager sm;
	bool swiped;
	Color color;
	float timer;
	float velocity;
	float alpha;
	float alpha_vel;
	float primary_alpha;
	float secondary_alpha;
	float velocity_ref;


	void OnEnable()
	{
		EventManager.OnStateActive += OnStateActive;
		EventManager.OnSwipe += OnSwipe;
	}

	void OnDisable()
	{
		EventManager.OnStateActive -= OnStateActive;
		EventManager.OnSwipe -= OnSwipe;
	}

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		primary_alpha = .7f;
		secondary_alpha = .2f;
		spriteRenderer.color = new Color(spriteRenderer.color.r,
		                                 spriteRenderer.color.g,
		                                 spriteRenderer.color.b,
		                                 secondary_alpha);
		color = spriteRenderer.color;
		alpha = secondary_alpha;
		gpc = GamePlayController.Instance;
		sm = StateManager.Instance;
		velocity = 1.0f;
	}


	void Update()
	{


		if (gpc.isGameOver)
		{
			velocity = Mathf.SmoothDamp(velocity, 0, ref velocity_ref, .3f);
			if (velocity <= .001f)
			{
				GameManager.Instance.Reset();
			}
		}

		if (sm.gameState == State.GAME || sm.gameState == State.MENU)
		{
			if (swiped)
			{
				timer += Time.deltaTime;
				if (timer > .35f)
				{
					timer = 0;
					swiped = false;
				}
			} else
			{
				MovePath(Time.deltaTime * gpc.difficulty * velocity);
			}
		}


	}

	void MovePath(float speed)
	{
		if (gpc.direction == GamePlayController.Direction.NORTH)
		{
			UpdateVelocity(0,  -speed);
		} else if (gpc.direction == GamePlayController.Direction.EAST)
		{
			UpdateVelocity(-speed,  0);
		}
		else if (gpc.direction == GamePlayController.Direction.WEST)
		{
			UpdateVelocity(speed,  0);
		}
		else if (gpc.direction == GamePlayController.Direction.SOUTH)
		{
			UpdateVelocity(0,  speed);
		}
	}

	void OnSwipe(float i)
	{
		swiped = true;
		timer = 0;
	}


	void UpdateVelocity(float x, float y)
	{
		transform.Translate(x, y, 0, Space.World);
	}


	void OnStateActive(State s)
	{
		if (spriteRenderer == null)
		{
			return;
		}

		if (s == State.GAME)
		{
			spriteRenderer.color = new Color(spriteRenderer.color.r,
			                                 spriteRenderer.color.g,
			                                 spriteRenderer.color.b,
			                                 primary_alpha);
		} else
		{
			spriteRenderer.color = new Color(spriteRenderer.color.r,
			                                 spriteRenderer.color.g,
			                                 spriteRenderer.color.b,
			                                 secondary_alpha);
		}
	}

}
