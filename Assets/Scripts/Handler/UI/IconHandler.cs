using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AppSystem;
public class IconHandler : MonoBehaviour {

	private GameObject[] icons;
	private bool iconsActive = true;

	void Start ()
	{
		Initialize();
	}

	private void Initialize()
	{
		icons = new GameObject[transform.childCount];

		for (int i = 0; i < transform.childCount; i++)
		{
			icons[i] = transform.GetChild(i).gameObject;
		}

		ToggleIcons();
	}



	void Update () {

	}

	public void ToggleIcons()
	{
		iconsActive = !iconsActive;
		for (int i = 0; i < icons.Length; i++)
		{
			if (i == 0)
			{
				icons[i].GetComponent<Animation>().Play("pickup_collect");
				icons[i].GetComponent<Image>().sprite = !iconsActive ? AppResources.gear : AppResources.back;
				continue;
			}
			icons[i].GetComponent<Icon>().interactable = iconsActive;
			icons[i].GetComponent<Icon>().Animate(iconsActive ? "setting_icon" : "setting_icon_close", (float)i / (float)(icons.Length * 7.0f));
		}


	}


}
