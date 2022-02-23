using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InAppManager : MonoBehaviour {
	[System.Serializable]
	public class Package
	{
		public float price;
		public int goldReward;
		public int gemReward;
	}

	[SerializeField]
	public enum CurrencyToBuy
	{
		gems,
		gold,
		package
	}
	public CurrencyToBuy currencyItem;

	public float[] gemPrices;
	public int[] gemRewards;
	public Text[] gemPricesTxt;
	public Text[] gemRewardsTxt;



	public float[] goldPrices;
	public int[] goldRewards;
	public Text[] goldPricesTxt;
	public Text[] goldRewardsTxt;


	public Package[] packages;
	public Text[] packagePricesTxt;
	public Text[] packageRewardsTxt;

	public GameObject confirmationPanel;
	public Text currentRewardTxt;
	public Text currentPriceTxt;

	float currentGemPrice;
	float currentGoldPrice;
	public	int currentRewardGem;
	public int currentRewardGold;
	string currentID;

	public GameObject loading;
	public GameObject failedPanel;
	public Text failedMessage;
	private static InAppManager inAppManager;
	public static InAppManager GetInstance()
	{
		return inAppManager;
	}
	void Awake()
	{
		inAppManager = this;
	}

	void OnEnable()
	{
		for (int i = 0; i < gemPrices.Length; i++) {
			gemPricesTxt [i].text = IAP.Instance.gemsPrices [i];
		}
		for (int i = 0; i < gemRewards.Length; i++) {
			gemRewardsTxt[i].text = gemRewards[i].ToString()+ " Gems";
		}
		for (int i = 0; i < goldPrices.Length; i++) {
			goldPricesTxt[i].text = IAP.Instance.gemsPrices [i];
		}
		for (int i = 0; i < goldRewards.Length; i++) {
			goldRewardsTxt[i].text = goldRewards[i].ToString()+ " Gold";
		}

		for (int i = 0; i < packages.Length; i++) {
			packagePricesTxt[i].text = IAP.Instance.packs [i];
		}
		for (int i = 0; i < packages.Length; i++) {
			packageRewardsTxt[i].text = packages[i].goldReward.ToString() +" Gold,\n "+packages[i].gemReward.ToString()+" Gems" ;
		}
	}


	public void BuyTheseGems(int index)
	{
		currencyItem = CurrencyToBuy.gems;
		currentPriceTxt.text = gemPricesTxt [index].text+ " ";
		currentRewardTxt.text = gemRewards[index].ToString()+ " Gems";
		currentGemPrice = gemPrices [index];
		currentRewardGem = gemRewards [index];
		OpenConfirmationPanel ();
	}

	public void BuyTheseGold(int index)
	{
		currencyItem = CurrencyToBuy.gold;
		currentPriceTxt.text = goldPricesTxt[index].text+ " " ;
		currentRewardTxt.text = goldRewards[index].ToString()+" Gold";
		currentGoldPrice = goldPrices [index];
		currentRewardGold = goldRewards [index];
		OpenConfirmationPanel ();
	}
	public void BuyThesePackage(int index)
	{
		currencyItem = CurrencyToBuy.package;
		currentPriceTxt.text = packagePricesTxt[index].text;
		currentRewardTxt.text = packages[index].goldReward.ToString()+" Gold, " + packages[index].gemReward.ToString()+ " Gems";
		currentGoldPrice = packages [index].price;
		currentRewardGold = packages [index].goldReward;
		currentRewardGem = packages [index].gemReward;
		OpenConfirmationPanel ();
	}


	public void OpenConfirmationPanel()
	{
		confirmationPanel.SetActive (true);

	}
	public void CloseConfirmationPanel()
	{
		confirmationPanel.SetActive (false);
		failedPanel.SetActive (false);
	}

	public void SendThisProductID(string productID)
	{
		currentID = productID;
	}
	public void SendThisProductID_RemoveAds(string productID)
	{
		IAP.Instance.PurchaseItemButton(productID);
	}
	public void YesBuyThis()
	{
		IAP.Instance.PurchaseItemButton (currentID);

		CloseConfirmationPanel ();
	}


}
