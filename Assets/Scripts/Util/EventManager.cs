using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class EventManager : MonoBehaviour {

	public delegate void  Swipe(float i);
	public static Swipe OnSwipe;
	public delegate void  Input();

	public static Input OnTap;

	public delegate void StateActive(State s);
	public static StateActive OnStateActive;

	public delegate void Gameplay();
	public static Gameplay OnGameOver;

	public delegate void Skin(int i);
	public static Skin OnSkinChange;

}
