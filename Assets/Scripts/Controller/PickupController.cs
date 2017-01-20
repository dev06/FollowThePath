using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour {

	SpriteRenderer spriteRenderer;
	float primary_alpha;
	float secondary_alpha;

	void OnEnable()
	{
		EventManager.OnStateActive += OnStateActive;
	}


	void OnDisable()
	{
		EventManager.OnStateActive -= OnStateActive;
	}

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		primary_alpha = .7f;
		secondary_alpha = .2f;
		spriteRenderer.color = new Color(spriteRenderer.color.r,
		                                 spriteRenderer.color.g,
		                                 spriteRenderer.color.b,
		                                 secondary_alpha);
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
