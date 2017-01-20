using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AppSystem;
/// <summary>
/// Class dedicated to spawn and manage in-game entities and objects
/// </summary>
public class SpawnController : MonoBehaviour {


	public static SpawnController Instance;

	private List<GameObject> ref_path;
	private GameObject ref_player;
	private GameObject obj_spawn;
	private Transform hie_path;
	private int previousValue = 0;
	private GamePlayController gpc;

	void Awake()
	{
		Instance = this;
	}

	void Start ()
	{
		Initialize();
	}

	private void Initialize()
	{
		gpc = GamePlayController.Instance;
		obj_spawn = GameObject.FindWithTag("Objects");
		hie_path = obj_spawn.transform.FindChild("Path");
		SpawnPlayer();
		SpawnPath();
	}

	/// <summary>
	/// Spawns The Player
	/// </summary>
	private void SpawnPlayer()
	{
		ref_player = (GameObject)Instantiate(AppResources.Player, -Vector3.up * 3, Quaternion.identity);
		ref_player.transform.SetParent(obj_spawn.transform.FindChild("Entity"));
	}

	/// <summary>
	/// Instantiates the path
	/// </summary>
	private void SpawnPath()
	{
		ref_path = new List<GameObject>();
		for (int i = 0; i < gpc.PathCount; i++)
		{
			GameObject path = (GameObject)Instantiate(AppResources.Path, -Vector3.up *  10, Quaternion.identity);
			path.transform.SetParent(hie_path);
			path.transform.localScale = new Vector3(gpc.PathWidth, gpc.InitPathLength, 1f);
			path.name = "path_" + i;
			SpawnPickup(path.transform);
			ref_path.Add(path);
		}

		RepositionPath();
	}

	/// <summary>
	/// Spawns the Pickup
	/// </summary>
	/// <param name="parent"></param>
	private void SpawnPickup(Transform parent)
	{
		int pickup_count = Random.Range(gpc.PickUpSpawn[0], gpc.PickUpSpawn[1]);
		for (int i = 0; i < pickup_count; i++)
		{
			GameObject pickup = (GameObject)Instantiate(AppResources.Pickup, Vector3.zero, Quaternion.identity);
			pickup.transform.SetParent(parent.GetChild(1));
			pickup.transform.localScale = new Vector3(parent.localScale.y, parent.localScale.x, 1);
			pickup.transform.localPosition = new Vector3( 0, i / (float)pickup_count, 0);
			pickup.name = "pickup_" + i;
			RepositionPickup(parent,  10);
		}
	}

	/// <summary>
	/// Repositions the path into a line
	/// </summary>
	private void RepositionPath()
	{
		for (int i = 0; i < ref_path.Count; i++)
		{
			Transform rt_previousPath = null;
			Transform rt_path = ref_path[i].GetComponent<Transform>(); // returns the current path
			if (i >= 1)
			{
				rt_previousPath = ref_path[i - 1].GetComponent<Transform>(); // return previous path
			}

			if (rt_previousPath != null)
			{

				Vector3 position = new Vector3(rt_path.position.x, (i * rt_previousPath.localScale.y) - 10 , 0);
				rt_path.position = position;
			}
		}
	}

	/// <summary>
	/// Pools the Path
	/// </summary>
	public void PoolPath()
	{
		Transform currentPath = hie_path.GetChild(0);
		Transform lastPath = hie_path.GetChild(hie_path.childCount - 1).GetChild(0);
		Vector3 newPosition = lastPath.position;
		currentPath.position = newPosition;
		if (StateManager.Instance.gameState == State.GAME)
		{

			RotatePath(currentPath, lastPath);

			ChangePathScale(currentPath);
			RepositionPickup(currentPath, gpc.PickupSpawnProb);

		}
		currentPath.SetSiblingIndex(ref_path.Count - 1);
	}


	/// <summary>
	/// Rotates the Path
	/// </summary>
	/// <param name="targetPath"></param>
	/// <param name="previousPath"></param>
	private void RotatePath(Transform targetPath, Transform previousPath)
	{
		int value = Random.Range(-1, 2);
		int counter = 0;

		while (value == previousValue)
		{
			value = Random.Range(-1, 2);
		}

		float targetRotation = (int)previousPath.eulerAngles.z  + value * 90;
		Vector3 rotation = targetPath.eulerAngles;
		rotation = new Vector3(0, 0, targetRotation);
		targetPath.eulerAngles = rotation;

		if (previousPath.eulerAngles.z != targetRotation)
		{
			targetPath.transform.position += GetPathOffset((int)targetRotation, (int)previousPath.eulerAngles.z, targetPath.localScale.x);
		}

		previousValue = value;
	}

	/// <summary>
	/// Reposition the pickup after path being pulled
	/// </summary>
	/// <param name="parent"></param>
	/// <param name="chance"></param>
	private void RepositionPickup(Transform parent, int chance)
	{
		if (Random.Range(0, 11) > chance)
		{
			SetPickupVisibility(parent, false);
			return;
		}
		SetPickupVisibility(parent, true);
		float diff = parent.localScale.y / 10.0f;
		float xOffset = 1.5f;
		float pickupSize = 1;
		float crowdness = 2.0f;
		float aspect = parent.localScale.y / parent.localScale.x;
		float value = pickupSize / aspect;

		for (int i = 0; i < parent.GetChild(1).childCount; i++)
		{
			Transform pickup = parent.GetChild(1).GetChild(i);
			pickup.localScale = new Vector3(pickupSize, value);
			pickup.localPosition = new Vector3((i % 2 == 0) ? xOffset : -xOffset, ((i / (float)parent.GetChild(1).childCount / crowdness) + .5f) - diff / (crowdness * 2.0f), 0);
		}
	}

	/// <summary>
	/// Changes the path scale
	/// </summary>
	/// <param name="currentPath"></param>
	private void ChangePathScale(Transform currentPath)
	{
		currentPath.localScale = new Vector3(currentPath.localScale.x, Random.Range(gpc.PathLength[0], gpc.PathLength[1]), currentPath.localScale.z);
	}

	/// <summary>
	/// Adds the path offset
	/// </summary>

	private Vector3 GetPathOffset(int targetRotation, int previousPathRotation, float size)
	{
		Vector3 ret = Vector3.zero;
		float factor = size / 2.0f;
		switch (targetRotation)
		{
			case 360: 	{	ret.x =  previousPathRotation == 270 ? factor : previousPathRotation == 90 ? -factor : 0;			ret.y =  previousPathRotation == 0 ? 0 : -factor;			ret.z = 0;		break;}
			case 270: 	{	ret.x = -factor;																					ret.y = previousPathRotation == 180 ? -factor : 0;			ret.z = 0;		break;}
			case 180: 	{	ret.x = previousPathRotation == 90 ? -factor : previousPathRotation == 270 ? factor : 0;			ret.y = factor;	 											ret.z = 0; 		break;}
			case -90:	{	ret.x = -factor; 																					ret.y = previousPathRotation == 0 ? factor : -factor; 		ret.z = 0; 		break;}
			case 90: 	{	ret.x = factor;																						ret.y = previousPathRotation == 0 ? factor : -factor;		ret.z = 0;		break;}
			case 0:		{	ret.x = previousPathRotation == 270 ? factor : previousPathRotation == 90 ? -factor : 0;			ret.y = previousPathRotation == 0 ? 0 : -factor;			ret.z = 0;		break;}
		}

		return ret;
	}

	/// <summary>
	/// Sets the visibility of the pickups of the path
	/// </summary>
	/// <param name="target"></param>
	/// <param name="value"></param>
	private void SetPickupVisibility(Transform target, bool value)
	{
		for (int i = 0; i < target.GetChild(1).childCount; i++)
		{
			SpriteRenderer sr = target.GetChild(1).GetChild(i).GetComponent<SpriteRenderer>();
			sr.color = GetPickupColor(gpc.difficultyIndex + 1);
			if (sr.enabled != value)
			{
				sr.enabled = value;
			}
		}
	}

	private Color GetPickupColor(int difficultyIndex)
	{
		switch (difficultyIndex)
		{
			case 1: return GamePlayController.pickup_color_one;
			case 2 : return GamePlayController.pickup_color_two;
			case 3 : return GamePlayController.pickup_color_three;
			default : return GamePlayController.pickup_color_three;
		}
	}
}
