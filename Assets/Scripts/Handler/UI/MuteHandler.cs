using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AppSystem;
public class MuteHandler : MonoBehaviour {


	private Image sprite;
	private Animation animation;


	void Start ()
	{

		UpdatePauseMute(AudioController.Instance.mute);
	}

	void Update ()
	{

	}

	public void UpdatePauseMute(bool b)
	{
		if (sprite == null)
		{
			sprite = GetComponent<Image>();
		}


		sprite.sprite = b ? AppResources.headphone_mute : AppResources.headphone;

	}

	public void OnChange()
	{
		if (animation == null)
		{
			animation = GetComponent<Animation>();

		}

		animation.Stop();
		animation.Play(animation.clip.name);
	}

}
