using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MenuPickupHandler : MonoBehaviour {

	private Text pickUpText;
	void Start ()
	{
		pickUpText = transform.GetChild(1).GetComponent<Text>();
		UpdatePickUpText();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void UpdatePickUpText()
	{
		//pickUpText.text = "x " + SaveManager.GetInt("pickup");
	}
}
