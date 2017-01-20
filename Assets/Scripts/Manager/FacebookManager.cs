using UnityEngine;
using System.Collections;
using Facebook.Unity;
using UnityEngine.UI;
using System.Collections.Generic;
public class FacebookManager : MonoBehaviour
{
	public static FacebookManager Instance;
	void Awake()
	{

		Instance = this;
		if (!FB.IsInitialized)
		{
			FB.Init();
		} else
		{
			FB.ActivateApp();
		}
	}

	public void Share()
	{
		int lastScore = SaveManager.GetInt("lastGamePickup");
		string message = "I just collected " + lastScore + " coins in my last try. Can you do any better?";
		FB.ShareLink(contentTitle: "Follow The Path", contentURL: new System.Uri("http://www.multidevan13.weebly.com"), contentDescription: message , callback: OnShare);
	}

	private void OnShare(IShareResult result)
	{
		if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
		{
			Debug.Log("ShareLink error: " + result.Error);
		} else if (!string.IsNullOrEmpty(result.PostId))
		{
			Debug.Log(result.PostId);
		} else
		{
			Debug.Log("Succeed");
		}
	}

}
