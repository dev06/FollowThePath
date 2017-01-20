using UnityEngine;
using System.Collections;
using AppSystem;
public class PlayerController : MonoBehaviour {

	float targetRotation;
	float deathTimer;
	float velocity;
	float velocity_ref;
	float primary_alpha;
	float secondary_alpha;
	int sibilingIndex = 0;
	int hitCount;
	bool isDead;
	bool executed ;
	SpriteRenderer spriteRenderer;
	GameObject currentPath = null;
	GameObject obj;
	GamePlayController gpc;
	Skin playerSkin;
	StateManager sm;

	void OnEnable()
	{
		EventManager.OnSwipe += OnSwipe;
		EventManager.OnStateActive += OnStateActive;
		EventManager.OnSkinChange += OnSkinChange;
	}
	void OnDisable()
	{
		EventManager.OnSwipe -= OnSwipe;
		EventManager.OnStateActive -= OnStateActive;
		EventManager.OnSkinChange -= OnSkinChange;

	}

	void Start ()
	{
		gpc = GamePlayController.Instance;
		sm = StateManager.Instance;
		spriteRenderer = GetComponent<SpriteRenderer>();
		primary_alpha = 1.0f;
		secondary_alpha = .8f;
		spriteRenderer.color = new Color(spriteRenderer.color.r,
		                                 spriteRenderer.color.g,
		                                 spriteRenderer.color.b,
		                                 secondary_alpha);

		obj = GameObject.Find("Path");

	}
	float pickedTimer;

	float pitch = 1.0f;
	void Update ()
	{
		UpdatePlayer();

		if (picked)
		{
			pickedTimer += Time.deltaTime;
		}

		if (pickedTimer > .2f)
		{
			picked = false;
			pitch = 1.0f;
		}
	}

	void UpdatePlayer()
	{

		if (hitCount <= 0)
		{
			deathTimer += Time.deltaTime;

			if (deathTimer > .1f)
			{
				isDead = true;
				gpc.isGameOver = true;
			}
		}


		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, targetRotation)), 15 * Time.deltaTime);

		if (gpc.autoPlay)
		{
			sibilingIndex = currentPath.transform.GetSiblingIndex();
			GameObject nextPath = obj.transform.GetChild(sibilingIndex + 1).gameObject;
			if (Vector2.Distance(transform.position, nextPath.transform.position) < 1.0f)
			{
				if (!executed)
				{
					float diff = nextPath.transform.eulerAngles.z - transform.eulerAngles.z;
					targetRotation = targetRotation + diff;
					executed = true;
				}
			} else {
				executed = false;
			}
		}
	}

	void OnSwipe(float i)
	{
		targetRotation += i > 0 ? -90 : 90;
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




	void OnTriggerExit2D(Collider2D col)
	{



		switch (col.gameObject.tag)
		{
			case "Entity/Path":
			{
				if (col.gameObject.transform.GetSiblingIndex() > 1)
				{
					SpawnController.Instance.PoolPath();
				}
				if (!isDead)
				{
					hitCount--;
					deathTimer = 0;
				}
				break;
			}

		}
	}
	bool picked;
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Entity/Path")
		{
			if (!isDead)
			{
				hitCount++;
				deathTimer = 0;
				currentPath = col.gameObject;
			}
		}


		if (sm != null && sm.gameState == State.GAME)
		{
			if (col.gameObject.tag == "Entity/Pickup")
			{
				bool active = col.gameObject.GetComponent<SpriteRenderer>().enabled ;
				if (active)
				{
					gpc.UpdatePickupCounter();

					pitch += .2f;
					picked = true;
					pickedTimer = 0;


					AudioController.Instance.Play(AppResources.pickup_sfx, pitch);
				}
				active = false;
				col.gameObject.GetComponent<SpriteRenderer>().enabled = active;
			}
		}
	}


	public void OnSkinChange(int i )
	{
		if (spriteRenderer == null)
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		SetSkin(SkinController.Instance.skins[i]);
	}

	public Skin GetPlayerSkin()
	{
		return playerSkin;
	}

	public void SetSkin(Skin s)
	{
		playerSkin = s;
		spriteRenderer.sprite = playerSkin.sprite;
	}



}
