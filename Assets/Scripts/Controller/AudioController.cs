using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AppSystem;
public class AudioController : MonoBehaviour {


	public static AudioController Instance;
	private AudioSource audioSource;
	public bool mute;
	private Image muteImage;
	private Animation muteAnimation;
	private MuteHandler muteHandler;

	void Awake()
	{
		Instance = this;
	}
	void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		mute = SaveManager.GetBool("mute");
		SetMute(mute);
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void Play(AudioClip clip, float pitch)
	{
		if (!mute)
		{
			audioSource.clip = clip;
			audioSource.pitch =  pitch;
			audioSource.Play();
		}
	}

	public void SetMute(bool b)
	{
		mute = b;

		SaveManager.SetBool("mute", mute);


	}

}
