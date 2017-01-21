using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AppSystem;
public class GamePlayController : MonoBehaviour {


	public static GamePlayController Instance;

	public static Color diff_stage_one = 			new Color(54.0f / 255.0f, 102.0f / 255.0f, 144.0f / 255.0f, 1);
	public static Color diff_stage_two = 			new Color(65.0f / 255.0f, 68.0f / 255.0f, 131.0f / 255.0f, 1);
	public static Color diff_stage_three = 		new Color(55.0f / 255.0f, 139.0f / 255.0f, 63.0f / 255.0f, 1);
	public static Color diff_stage_four = 		new Color(150.0f / 255.0f, 150.0f / 255.0f, 77.0f / 255.0f, 1);
	public static Color diff_stage_five = 			new Color(174.0f / 255.0f, 113.0f / 255.0f, 113.0f / 255.0f, 1);


	public static Color pickup_color_one = new Color(1, 1, 1, 1);
	public static Color pickup_color_two = new Color(127.5f / 255f, 255f / 255f, 127.5f / 255f, 1);
	public static Color pickup_color_three = new Color(255f / 255f, 255f / 255f, 127.5f / 255f, 1);

	public static int CoinsToBlow = 500 ;

	public enum Direction
	{
		NORTH,
		EAST,
		SOUTH,
		WEST
	}

	public Direction[] FacingDirection = { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST};
	public long totalPickup;
	public int facingDirectionIndex;
	public int pickUpCounter;
	public int lastGameplayPickupCounter;
	public Direction direction;
	public float difficulty;
	public bool isGameOver;
	public bool autoPlay;
	public bool updatePlayer;
	private Camera camera;
	private float startDifficulty = 10.0f;
	private float difficultyProgression = .14f;
	private float difficultyStageProgression = 1.1f;
	private float difficultyChange = 45.0f;
	private float difficultyTimer;
	private float difficultyVel;
	private Color[] difficultyStage =  { diff_stage_one, diff_stage_two, diff_stage_three, diff_stage_four, diff_stage_five};
	private Text pickUpText;
	private Text totalPickupText;
	private Animation pickUpAnimation;
	private GameObject blowCoin;
	public int difficultyIndex;
	public int pickupConsecutive;
	public float nextTier;

	public float[] PathLength = {6, 10};
	public int [] PickUpSpawn = {10, 20};
	public float InitPathLength = 15;
	public int PathCount = 5;
	public float PathWidth = .25f;
	public int PickupSpawnProb = 4;

	void Awake()
	{
		Instance = this;
	}

	void OnEnable()
	{
		EventManager.OnSwipe += OnSwipe;
	}

	void OnDisable()
	{
		EventManager.OnSwipe -= OnSwipe;
	}





	void Start ()
	{
		difficulty = startDifficulty;
		nextTier = difficultyChange;
		camera = Camera.main;


	}

	void Update ()
	{
		if (StateManager.Instance.gameState == State.GAME)
		{
			difficultyTimer += Time.deltaTime;
		}

		if (difficultyTimer > nextTier)
		{
			nextTier = difficultyTimer + difficultyChange;
			if (difficultyIndex < difficultyStage.Length - 1)
			{
				difficultyIndex++;
			}

		}
		difficulty = Mathf.SmoothDamp(difficulty, difficultyIndex * 3 + startDifficulty, ref difficultyVel, 2.0f);
		camera.backgroundColor = Color.Lerp(camera.backgroundColor, difficultyStage[difficultyIndex], Time.deltaTime * 2.0f);
	}

	void OnSwipe(float i)
	{
		UpdateDirection(i);
	}

	public void UpdateDirection(float i)
	{
		if (i > 0)
		{
			if (facingDirectionIndex < FacingDirection.Length - 1)
			{
				facingDirectionIndex++;
			} else {
				facingDirectionIndex = 0;
			}
		} else {
			if (facingDirectionIndex > 0)
			{
				facingDirectionIndex--;
			} else {
				facingDirectionIndex = FacingDirection.Length - 1 ;
			}
		}

		direction = FacingDirection[facingDirectionIndex];
	}

	public void UpdatePickupCounter()
	{
		pickUpCounter += (difficultyIndex > 2 ? 3 : difficultyIndex) + 1;

		if (pickUpText == null || pickUpAnimation == null)
		{
			pickUpText = GameObject.FindWithTag("Entity/PickCounter").GetComponent<Text>();
			pickUpAnimation = pickUpText.gameObject.GetComponent<Animation>();
		}
		pickUpText.text = "x" + pickUpCounter;

		pickUpAnimation.Stop();
		pickUpAnimation.Play("pickup_collect");

	}

	public void SetTotalPickup(int value)
	{
		totalPickup += value;
		if (totalPickupText == null)
		{

			totalPickupText = GameObject.FindWithTag("UI/totalPickupText").GetComponent<Text>();

		}
		totalPickupText.text = "x " + totalPickup;
		CheckForBlowCoin();
	}

	public void BlowCoins()
	{
		GameObject go = (GameObject)Instantiate(AppResources.BlowCoin, Vector3.zero, Quaternion.identity);
		ParticleSystem ps = go.GetComponent<ParticleSystem>();
		ParticleSystem.EmissionModule em = ps.emission;


		if (totalPickup >= CoinsToBlow)
		{
			em.rate = CoinsToBlow * 2;
			SetTotalPickup(-CoinsToBlow);
		} else
		{
			em.rate = totalPickup * 2;
			SetTotalPickup((int) - totalPickup);
		}
		SaveManager.SetInt("pickup", (int)GamePlayController.Instance.totalPickup);
		SkinController.Instance.UpdateSkin(0);
		CheckForBlowCoin();

	}

	public void CheckForBlowCoin()
	{
		if (blowCoin == null)
		{
			blowCoin = GameObject.FindWithTag("UI/Blowcoin").gameObject;

		}
		blowCoin.GetComponentInChildren<Text>().text = "Blow Coins! " + "( Max: " + CoinsToBlow + ")";
		blowCoin.SetActive(totalPickup > 0);

	}
}
