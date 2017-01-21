using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using AppSystem;
public enum ButtonID
{
	NONE,
	PAUSE,
	PAUSEPANEL,
	MENUPANEL,
	RETRY,
	MUTE,
	LEFTARROW,
	RIGHTARROW,
	PURCHASE,
	BLOWCOIN,
	FACEBOOK,
	RATE,
	SETTING,
}

public class ButtonEventHandler : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler {

	public ButtonID buttonID;

	private Vector2 pointerDown;
	private Vector2 pointerUp;


	public virtual void OnPointerClick(PointerEventData data)
	{


	}

	public virtual void OnPointerUp(PointerEventData data)
	{
		pointerUp = data.position;
		if (Mathf.Abs(pointerUp.x - pointerDown.x) < 50)
		{
			switch (buttonID)
			{
				case ButtonID.PAUSE:
				{
					if (!GamePlayController.Instance.isGameOver)
					{
						StateManager.Instance.SetState(State.PAUSE);
					}
					break;

				}

				case ButtonID.MENUPANEL:
				{
					StateManager.Instance.SetState(State.GAME);
					break;
				}
				case ButtonID.PAUSEPANEL:
				{
					StateManager.Instance.SetState(State.GAME);
					break;
				}

				case ButtonID.MUTE:
				{

					AudioController.Instance.SetMute(!AudioController.Instance.mute);
					GetComponent<MuteHandler>().UpdatePauseMute(AudioController.Instance.mute);
					GetComponent<MuteHandler>().OnChange();
					break;
				}
				case ButtonID.RETRY:
				{
					GameManager.Instance.Reset();
					break;
				}

				case ButtonID.LEFTARROW:
				{
					SkinController.Instance.UpdateSkin(-1);
					break;
				}

				case ButtonID.RIGHTARROW:
				{
					SkinController.Instance.UpdateSkin(1);
					break;
				}

				case ButtonID.PURCHASE:
				{
					SkinController.Instance.PurchaseSkin();
					break;
				}

				case ButtonID.BLOWCOIN:
				{
					GamePlayController.Instance.BlowCoins();
					break;
				}
				case ButtonID.FACEBOOK:
				{
					if (transform.GetComponent<Icon>().interactable)
					{
						FacebookManager.Instance.Share();
					}
					break;
				}
				case ButtonID.RATE:
				{
					if (transform.GetComponent<Icon>().interactable)
					{
						GameManager.Instance.RateApp();
					}
					break;
				}
				case ButtonID.SETTING:
				{
					transform.GetComponentInParent<IconHandler>().ToggleIcons();
					break;
				}
			}
		}
	}
	public virtual void OnPointerDown(PointerEventData data)
	{
		pointerDown = data.position;
	}



}
