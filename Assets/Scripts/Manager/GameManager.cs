using UnityEngine;
using System.Collections;
using AppSystem;
public class GameManager : MonoBehaviour {

	public static GameManager Instance;
	private bool gameReseted;
	void Awake()
	{
		Instance = this;
	}

	void Start ()
	{
		//SaveManager.DeleteAll();
		GamePlayController.Instance.SetTotalPickup(SaveManager.GetInt("pickup"));
	}


	public void Reset()
	{
		if (gameReseted)
		{
			return;
		}

		GamePlayController.Instance.SetTotalPickup(GamePlayController.Instance.pickUpCounter);
		GamePlayController.Instance.lastGameplayPickupCounter = GamePlayController.Instance.pickUpCounter;
		SaveManager.SetInt("lastGamePickup", GamePlayController.Instance.lastGameplayPickupCounter);
		SaveManager.SetInt("pickup", (int)GamePlayController.Instance.totalPickup);
		UnityEngine.SceneManagement.SceneManager.LoadScene("game");
		gameReseted = true;
	}

	public void RateApp()
	{
		// To be completed in v 1.0
		// after google play link generation
	}



}

public class SaveManager: MonoBehaviour
{

	public static bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public static void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	public static int GetInt(string value)
	{
		return PlayerPrefs.GetInt(value);
	}



	public static void SetFloat(string key, int value)
	{
		PlayerPrefs.SetFloat(key, value);
	}

	public static float GetFloat(string value)
	{
		return PlayerPrefs.GetFloat(value);
	}


	public static void SetBool(string key, bool value)
	{
		PlayerPrefs.SetString(key, value.ToString());
	}

	public static bool GetBool(string key)
	{
		if (!PlayerPrefs.HasKey(key))
		{
			return false;
		}

		return bool.Parse(PlayerPrefs.GetString(key));
	}

	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}


}
