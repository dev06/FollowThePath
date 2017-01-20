using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AppSystem;
using UnityEngine.UI;
public class SkinController : MonoBehaviour {


	public static SkinController Instance;

	public int skinIndex;

	public static Skin skin_1;
	public static Skin skin_2;
	public static Skin skin_3;
	public static Skin skin_4;
	public static Skin skin_5;
	public static Skin skin_6;
	public static Skin skin_7;
	public static Skin skin_8;
	public static Skin skin_9;
	public static Skin skin_10;
	public static Skin skin_11;
	public static Skin skin_12;
	public static Skin skin_13;
	public static Skin skin_14;
	public static Skin skin_15;
	public static Skin skin_16;
	public static Skin skin_17;
	public static Skin skin_18;
	public static Skin skin_19;
	public static Skin skin_20;
	public static Skin skin_21;
	public static Skin skin_22;
	public static Skin skin_23;
	public static Skin skin_24;
	public static Skin skin_25;
	public static Skin skin_26;
	public static Skin skin_27;





	public  Skin[] skins;
	public List<Skin> purchasedSkins;

	private PlayerController pc;
	private Text skinCostText;
	private Text purchasedText;
	private Image purchaseButton;

	void OnEnable()
	{
		EventManager.OnStateActive += OnStateActive;
	}

	void OnDisable()
	{
		EventManager.OnStateActive -= OnStateActive;
	}


	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		Initialize();
	}

	private void Initialize()
	{
		skin_1 = new Skin(0, AppResources.skin_1, 10, true);
		skin_2 = new Skin(1, AppResources.skin_2, 500);
		skin_3 = new Skin(2, AppResources.skin_3, 500);
		skin_4 = new Skin(3, AppResources.skin_4, 500);
		skin_5 = new Skin(4, AppResources.skin_5, 500);
		skin_6 = new Skin(5, AppResources.skin_6, 500);
		skin_7 = new Skin(6, AppResources.skin_7, 500);
		skin_8 = new Skin(7, AppResources.skin_8, 500);
		skin_9 = new Skin(8, AppResources.skin_9, 500);
		skin_10 = new Skin(9, AppResources.skin_10, 500);
		skin_11 = new Skin(10, AppResources.skin_11, 3000);
		skin_12 = new Skin(11, AppResources.skin_12, 3000);
		skin_13 = new Skin(12, AppResources.skin_13, 3000);
		skin_14 = new Skin(13, AppResources.skin_14, 3000);
		skin_15 = new Skin(14, AppResources.skin_15, 3000);
		skin_16 = new Skin(15, AppResources.skin_16, 3000);
		skin_17 = new Skin(16, AppResources.skin_17, 3000);
		skin_18 = new Skin(17, AppResources.skin_18, 3000);
		skin_19 = new Skin(18, AppResources.skin_19, 5000);
		skin_20 = new Skin(19, AppResources.skin_20, 5000);
		skin_21 = new Skin(20, AppResources.skin_21, 5000);
		skin_22 = new Skin(21, AppResources.skin_22, 10000);
		skin_23 = new Skin(22, AppResources.skin_23, 15000);
		skin_24 = new Skin(23, AppResources.skin_24, 20000);
		skin_25 = new Skin(24, AppResources.skin_25, 5000);
		skin_26 = new Skin(25, AppResources.skin_26, 5000);
		skin_27 = new Skin(26, AppResources.skin_27, 7000);



		skins = new Skin[27] {skin_1, skin_2, skin_3, skin_4, skin_5, skin_6,
		                      skin_7, skin_8, skin_9, skin_10, skin_11, skin_12,
		                      skin_13, skin_14, skin_15, skin_16, skin_17, skin_18,
		                      skin_19, skin_20, skin_21, skin_22, skin_23, skin_24,
		                      skin_25, skin_26, skin_27,

		                     };


		skinIndex = SaveManager.GetInt("skinIndex");
		purchasedSkins = new List<Skin>();
		RepopulatePurchasedSkins();
		StartCoroutine("Wait");

	}

	private void RepopulatePurchasedSkins()
	{
		for (int i = 0; i < skins.Length; i++)
		{
			if (SaveManager.HasKey("skinId_" + i))
			{
				if (SaveManager.GetBool("skinId_" + i))
				{
					purchasedSkins.Add(skins[i]);
				}
			}
		}
		for (int i = 0; i < purchasedSkins.Count; i++)
		{
			purchasedSkins[i].SetPurchased(true);
		}
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(.4f);
		UpdateSkin(0);
	}

	public void UpdateSkin(int i)
	{

		skinIndex += i;

		if (skinIndex < 0)
		{
			skinIndex = skins.Length - 1;
		}

		if (skinIndex > skins.Length - 1)
		{
			skinIndex = 0;
		}

		if (pc == null)
		{
			pc = GameObject.FindWithTag("Entity/Player").GetComponent<PlayerController>();
		}

		if (skinCostText == null)
		{
			skinCostText = GameObject.FindWithTag("Skin/costText").GetComponent<Text>();
		}

		if (purchaseButton == null)
		{
			purchaseButton = GameObject.FindWithTag("Skin/purchaseButton").GetComponent<Image>();
			purchasedText = purchaseButton.transform.GetChild(0).GetComponent<Text>();

		}

		pc.OnSkinChange(skinIndex);
		skinCostText.text = !skins[skinIndex].purchased ? skins[skinIndex].cost.ToString() : "";
		purchaseButton.gameObject.SetActive(!skins[skinIndex].purchased);
		purchasedText.text = CanPurcahseSkin(skins[skinIndex]) ? "Purchase" : "Not enough coins";
		SaveManager.SetInt("skinIndex", skinIndex);
	}

	public void PurchaseSkin()
	{
		Skin targetSkin = skins[skinIndex];

		if (CanPurcahseSkin(targetSkin))
		{
			if (!purchasedSkins.Contains(targetSkin))
			{
				purchasedSkins.Add(targetSkin);
				targetSkin.SetPurchased(true);
				SaveManager.SetBool("skinId_" + targetSkin.id, targetSkin.purchased);
				UpdateSkin(0);
				GamePlayController.Instance.SetTotalPickup((int)(-targetSkin.cost));
			}
		}

	}

	public bool CanPurcahseSkin(Skin skin)
	{
		return GamePlayController.Instance.totalPickup >= skin.cost;
	}

	void OnStateActive(State s)
	{
		if (s == State.GAME)
		{
			if (!pc.GetPlayerSkin().purchased)
			{
				pc.SetSkin(skins[0]);
			}
		}
	}


}


public class Skin
{
	public Sprite sprite;
	public int cost;
	public bool purchased;
	public int id;

	public Skin(int id, Sprite sprite, int cost, bool purchased = false)
	{
		this.id = id;
		this.sprite = sprite;
		this.cost = cost;
		this.purchased = purchased;
	}


	public void SetSprite(Sprite sprite)
	{
		this.sprite = sprite;
	}

	public void SetCost(int cost)
	{
		this.cost = cost;
	}

	public void SetPurchased(bool purchased)
	{
		this.purchased = purchased;
	}
}
