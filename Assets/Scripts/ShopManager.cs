using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopManager : MonoBehaviour {

	public static event Action OnRemoveAdsBought;
	public static void DoOnRemoveAdsBought() => OnRemoveAdsBought?.Invoke();

	[SerializeField]
	public enum ItemToBuy
	{
		character,
		powershots,
		specialMoves
	}

	public ItemToBuy itemToBuy;
	public Button[] characterBtns;
	public Button[] powerBtns;
	public Button[] specialMovesBtns;
	public GameObject gemsButton;
	public GameObject goldButton;
	public Text       gemAmountText;
	public Text       goldAmountText;
	public Text       characterName;

	public string[] characterNames;
	public int[] goldPricePerCharacter;
	public int[] gemPricePerCharacter;



	public int[] goldPricePerPower;
	public int[] gemPricePerPower;


	public int[] goldPricePerSpecialMoves;
	public int[] gemPricePerSpecialMoves;
	/// <summary>
	/// for confirmation panel
	/// </summary>
	public Image currencyImage;
	public Sprite goldCoin;
	public Sprite gems;
	public GameObject confirmationPanel;
	int currentPriceInGems;
	int currentPriceInGold;
	int currentCharacterIndex;
	int currentPowerIndex;
	public Text amountTextInConfirmationPanel;
	bool boughtFromGems = false;
	public GameObject notEnoughCurrencyPanel;



	public GameObject[] shopPanels;
	public GameObject[] sidePanelsObjects;

	private static ShopManager instance;
	public static ShopManager GetInstance()
	{
		return instance;
	}

    private void ShopManager_OnRemoveAdsBought()
    {
		OpenThisShopPanel(0);

	}

    public void OpenThisShopPanel(int index)
	{
		if(PlayerPrefs.GetInt("RemoveAds") ==1)
        {
			sidePanelsObjects[sidePanelsObjects.Length - 1].SetActive(false);

		}
		for (int i = 0; i < sidePanelsObjects.Length; i++) {
			if (i == index) 
			{
				shopPanels [index].SetActive (true);
				sidePanelsObjects [index].GetComponent<Image> ().color = new Color (255f,255f,255f,255f); 
			} 
			else 
			{
				shopPanels [i].SetActive (false);
				sidePanelsObjects [i].GetComponent<Image> ().color = new Color (255f,255f,255f,0); 
			}
	
		}
		switch (index) {
		case 0:
			ShowCharacterToBuy(currentCharacterIndex,characterNames[currentCharacterIndex] ,goldPricePerCharacter[currentCharacterIndex],gemPricePerCharacter[currentCharacterIndex]);
			break;
		case 1:
			characterSHows.inst.showCharInShop (currentCharacterIndex);
			ShowPowersToBuy (0, goldPricePerPower [0], gemPricePerPower [0]);
			break;
		case 2:
			characterSHows.inst.showCharInShop (currentCharacterIndex);
			ShowSpecialMovesToBuy (0, goldPricePerSpecialMoves [0], gemPricePerSpecialMoves [0]);
			break;
		case 3:
		case 4:
		case 5:
		case 6:
				characterSHows.inst.hideModelsParent ();
			gemsButton.SetActive (false);
			goldButton.SetActive (false);
		
			break;

		}
	}
	public void WatchAdForGold(int gold)
	{

#if UNITY_IOS || UNITY_ANDROID

  
     if (AdsManager.GetInstance ().unityRewardedAd.isLoaded)
			{
				AdsManager.GetInstance ().unityRewardedAd.ShowRewardedVideo (gold, 0, false);
				//AdsManager.GetInstance ().facebookRewardedAd.LoadRewardedVideo ();
			} 
		else if (AdsManager.GetInstance ().admobRewardedAd.isRewardedAdLoaded)
			{
				AdsManager.GetInstance ().admobRewardedAd.ShowRewardedVideo (gold, 0, false);
				//AdsManager.GetInstance ().facebookRewardedAd.LoadRewardedVideo ();

			}

		#endif

	}
	public void WatchAdForGems(int gems)
	{
#if UNITY_IOS || UNITY_ANDROID
    
       	if (AdsManager.GetInstance ().unityRewardedAd.isLoaded)
		{
			AdsManager.GetInstance ().unityRewardedAd.ShowRewardedVideo (0, gems, false);
			//AdsManager.GetInstance ().facebookRewardedAd.LoadRewardedVideo ();
		} 
		else if (AdsManager.GetInstance ().admobRewardedAd.isRewardedAdLoaded)
		{
			AdsManager.GetInstance ().admobRewardedAd.ShowRewardedVideo (0, gems, false);
			//AdsManager.GetInstance ().facebookRewardedAd.LoadRewardedVideo ();

		}
	
		#endif
	}
	void Awake()
	{
		instance = this;
		
		int golds = EconomyController.GetInstance ().GetGold ();
		int gems = EconomyController.GetInstance ().GetGems ();

	
	}
	void AttachFunctionsToButtons()
	{
		for (int i = 0; i < characterBtns.Length; i++) {
			int closureIndex = i ; // Prevents the closure problem
			characterBtns[closureIndex].onClick.AddListener(() => ShowCharacterToBuy(closureIndex,characterNames[closureIndex] ,goldPricePerCharacter[closureIndex],gemPricePerCharacter[closureIndex]));
		}
		for (int i = 0; i < powerBtns.Length; i++) {
			int closureIndex = i ; // Prevents the closure problem
			powerBtns[closureIndex].onClick.AddListener(() => ShowPowersToBuy(closureIndex,goldPricePerPower[closureIndex],gemPricePerPower[closureIndex]));
		}
		for (int i = 0; i < specialMovesBtns.Length; i++) {
			int closureIndex = i ; // Prevents the closure problem
			specialMovesBtns[closureIndex].onClick.AddListener(() => ShowSpecialMovesToBuy(closureIndex,goldPricePerSpecialMoves[closureIndex],gemPricePerSpecialMoves[closureIndex]));
		}
	}
	void Start()
	{

		ShowCharacterToBuy(0,"Logan Cupin",12300,115);
	
	}
	void OnEnable()
	{
		ShopManager.OnRemoveAdsBought += ShopManager_OnRemoveAdsBought;
		OpenThisShopPanel (0);
		EconomyController.GetInstance ().Start ();
		AttachFunctionsToButtons ();

	}
	public void ShowPowersToBuy(int index,int goldPrice, int gemPrice)
	{
		ShowPower (index+1);
		itemToBuy = ItemToBuy.powershots;
		currentPriceInGems = gemPrice;
		currentPriceInGold = goldPrice;
		foreach (Button btns in powerBtns) {
			btns.transform.GetChild (0).gameObject.SetActive (false);
		}
	
		powerBtns[index].transform.GetChild (0).gameObject.SetActive (true);
		if (PlayerPrefs.GetInt ("Power Move" + (index + 1))==0) {
			gemAmountText.text = gemPrice.ToString ();
			goldAmountText.text = goldPrice.ToString ();
			gemsButton.SetActive (true);
			goldButton.SetActive (true);
		} else {
			gemsButton.SetActive (false);
			goldButton.SetActive (false);
		}
		currentPowerIndex = index;
		int count = 0;
		for (int i = 1; i <= 2; i++) {
			if (PlayerPrefs.GetInt ("Power Move" + (i)) == 1) {
				count++;
			}
		}
		switch (count) {
		case 1:
			GPGManager.GetInstance ().UnlockAchievementGotAPowerShot ();
			break;
		case 2:
			GPGManager.GetInstance ().UnlockAchievementGot2PowerShots ();
			break;

		}
	}
	public void ShowSpecialMovesToBuy(int index,int goldPrice, int gemPrice)
	{
		Debug.Log (index);
		ShowMove (index+1);
		itemToBuy = ItemToBuy.specialMoves;
		currentPriceInGems = gemPrice;
		currentPriceInGold = goldPrice;
		foreach (Button btns in specialMovesBtns) {
			btns.transform.GetChild (0).gameObject.SetActive (false);
		}

		specialMovesBtns[index].transform.GetChild (0).gameObject.SetActive (true);
		if (PlayerPrefs.GetInt ("Special Move Bought" + (index + 1), 0)==0) {
			gemAmountText.text = gemPrice.ToString ();
			goldAmountText.text = goldPrice.ToString ();
			gemsButton.SetActive (true);
			goldButton.SetActive (true);
		} else {
			gemsButton.SetActive (false);
			goldButton.SetActive (false);
		}
		currentPowerIndex = index;
		int count = 0;
		for (int i = 1; i <= 6; i++) {
			if (PlayerPrefs.GetInt ("Special Move Bought" + (i)) == 1) {
				count++;
			}
		}
		switch (count) {
		case 1:
			GPGManager.GetInstance ().UnlockAchievementGot1SpecialMove ();
			break;
		case 2:
			GPGManager.GetInstance ().UnlockAchievementGot2SpecialMove ();
			break;
		case 3:
			GPGManager.GetInstance ().UnlockAchievementGot3SpecialMove ();
			break;
		case 4:
			GPGManager.GetInstance ().UnlockAchievementGot4SpecialMove ();
			break;
		case 5:
			GPGManager.GetInstance ().UnlockAchievementGot5SpecialMove ();
			break;
		case 6:
			GPGManager.GetInstance ().UnlockAchievementGot6SpecialMove ();
			break;
		}
	}
	public void ShowCharacterToBuy(int index,string name ,int goldPrice, int gemPrice)
	{
		itemToBuy = ItemToBuy.character;
		characterSHows.inst.showCharInShop (index);
		foreach (Button btns in characterBtns) {
     		btns.transform.GetChild (0).gameObject.SetActive (false);
		}
		currentPriceInGems = gemPrice;
		currentPriceInGold = goldPrice;
		characterBtns[index].transform.GetChild (0).gameObject.SetActive (true);
		if (PlayerPrefs.GetInt ("Characters Bought" + (index + 1)) == 0) {
			gemAmountText.text = gemPrice.ToString ();
			goldAmountText.text = goldPrice.ToString ();
			gemsButton.SetActive (true);
			goldButton.SetActive (true);
		} else {
			gemsButton.SetActive (false);
			goldButton.SetActive (false);
		}
		characterName.text = characterNames [index];
		currentCharacterIndex = index;

		int count = 0;
		for (int i = 1; i <= 15; i++) {
			if (PlayerPrefs.GetInt ("Characters Bought" + (i)) == 1) {
				count++;
			}
		}
		switch (count) {
		case 2:
			GPGManager.GetInstance ().UnlockAchievementGotANewFighter ();
			break;
		case 3:
			GPGManager.GetInstance ().UnlockAchievementGot2NewFighters ();
			break;
		case 4:
			GPGManager.GetInstance ().UnlockAchievementGot3NewFighters ();
			break;
		case 6:
			GPGManager.GetInstance ().UnlockAchievementGot5NewFighters ();
			break;
		case 8:
			GPGManager.GetInstance ().UnlockAchievementGot7NewFighters ();
			break;
		case 11:
			GPGManager.GetInstance ().UnlockAchievementGot10NewFighters ();
			break;
		case 15:
			GPGManager.GetInstance ().UnlockAchievementGotAllFighters ();
			break;
		}


	}
	public void BuyFromCoins()
	{
		currencyImage.sprite = goldCoin;

		if (EconomyController.GetInstance().GetGold() < currentPriceInGold) {
			notEnoughCurrencyPanel.SetActive (true);
			confirmationPanel.SetActive (false);
		} else {
			confirmationPanel.SetActive (true);
			notEnoughCurrencyPanel.SetActive (false);
		}
		amountTextInConfirmationPanel.text = currentPriceInGold.ToString();
		boughtFromGems = false;
	}
	public void BuyFromGems()
	{
		currencyImage.sprite = gems;
	
		if (EconomyController.GetInstance().GetGems() < currentPriceInGems) {
			notEnoughCurrencyPanel.SetActive (true);
			confirmationPanel.SetActive (false);
		} else {
			confirmationPanel.SetActive (true);
			notEnoughCurrencyPanel.SetActive (false);
		}
		amountTextInConfirmationPanel.text = currentPriceInGems.ToString();
		boughtFromGems = true;
	}
	public void CloseConfirmationPanel()
	{
		confirmationPanel.SetActive (false);
		notEnoughCurrencyPanel.SetActive (false);
	}

	public void BuyMoreGold()
	{
		CloseConfirmationPanel ();
		OpenThisShopPanel (4);
	}
	public void BuyMoreGems()
	{
		CloseConfirmationPanel ();
		OpenThisShopPanel (3);
	}
	public void YesBuyThisCharacter()
	{
		// Deducting currency
		if (boughtFromGems) {
			int result = EconomyController.GetInstance().GetGems() - currentPriceInGems;
			EconomyController.GetInstance ().SetGems (result);
		} else {
			int result = EconomyController.GetInstance().GetGold() - currentPriceInGold;
			EconomyController.GetInstance ().SetGold (result);

			PlayerPrefs.SetInt ("SpentGold",PlayerPrefs.GetInt("SpentGold")+currentPriceInGold);
			Debug.Log ("SpentGold " + PlayerPrefs.GetInt ("SpentGold"));
			if (PlayerPrefs.GetInt ("SpentGold") >= 30 ) {
				GPGManager.GetInstance ().UnlockAchievementSpent30Gold ();
			}
			 if (PlayerPrefs.GetInt ("SpentGold") >= 100) {
				GPGManager.GetInstance ().UnlockAchievementSpent100Gold ();
			}
			 if (PlayerPrefs.GetInt ("SpentGold") >= 3000) {
				GPGManager.GetInstance ().UnlockAchievementSpent3000Gold ();
				Debug.Log ("SpentGold 3000" + PlayerPrefs.GetInt ("SpentGold"));
			}

		}



		if (itemToBuy == ItemToBuy.character) {
			
			PlayerPrefs.SetInt ("Characters Bought" + (currentCharacterIndex + 1), 1);
			// Updating character status 
			ShowCharacterToBuy (currentCharacterIndex, characterNames [currentCharacterIndex], 0, 0);
		
			confirmationPanel.SetActive (false);
		
		} else if (itemToBuy == ItemToBuy.powershots) {
			PlayerPrefs.SetInt ("Power Move" + (currentPowerIndex + 1), 1);
			// Updating power shot status 
			ShowPowersToBuy (currentPowerIndex, 0, 0);

			confirmationPanel.SetActive (false);
		}
		else if (itemToBuy == ItemToBuy.specialMoves) {
			PlayerPrefs.SetInt ("Special Move Bought" + (currentPowerIndex + 1), 1);

			// Setting the Special Moves for player right after its purchase
			int specialMoveCounter = 0;
			for (int i = 0; i < specialMovesBtns.Length; i++) {
				if(PlayerPrefs.GetInt ("Special Move" + (i + 1)) !=0)
					{
					   specialMoveCounter++;
					}
			}
			// the Powers bought will not set if they are more than 3 and player have to manually select or deselect from selection panel window  
			if (specialMoveCounter < 3) {
				PlayerPrefs.SetInt ("Special Move" + (currentPowerIndex + 1), (currentPowerIndex + 1));
				PlayerPrefs.Save ();
			}

			// Updating special move status 
			ShowSpecialMovesToBuy(currentPowerIndex, 0, 0);

			confirmationPanel.SetActive (false);
		}
		// Updating all top bars with updated currency 

		currentPriceInGems = 0;
		currentPriceInGold = 0;
	}

	public void ShowMove(int indexOfMove)
	{
		switch(indexOfMove)
		{
		case 1:
			characterSHows.inst.ShowMove ("Special Move 1");
			break;
		case 2:
			characterSHows.inst.ShowMove ("Special Move 2");
			break;
		case 3:
			characterSHows.inst.ShowMove ("Special Move 3");
			break;
		case 4:
			characterSHows.inst.ShowMove ("Special Move 4");
			break;
		case 5:
			characterSHows.inst.ShowMove ("Special Move 5");
			break;
		case 6:
			characterSHows.inst.ShowMove ("Special Move 6");
			break;

		}
	}
	public void ShowPower(int indexOfMove)
	{
		switch(indexOfMove)
		{
		case 1:
			characterSHows.inst.ShowMove ("PowerShot 1");
			break;
		case 2:
			characterSHows.inst.ShowMove ("PowerShot 2");
			break;


		}
	}
	private void OnDisable()
    {
		ShopManager.OnRemoveAdsBought -= ShopManager_OnRemoveAdsBought;
	}
}
