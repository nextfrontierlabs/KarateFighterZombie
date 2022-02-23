using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class UnityAds : MonoBehaviour, IUnityAdsListener  {

	public string gameId = "4579829";
	public bool testMode = false;
	public bool isLoaded = false;
	public string placementID = "Interstitial_Android";

	#region rewarded Video Data
	public string myRewardedPlacementId = "Rewarded_Android";
	public bool isRewardedLoaded = false;


	public int gems;
	public int gold;
	public bool twiceTheReward;

	#endregion

	void Awake()
	{
		#if UNITY_IOS
		gameId = "3384380";
		#elif UNITY_ANDROID
		gameId = "4579829";
		#endif
		Advertisement.AddListener (this);
		Advertisement.Initialize (gameId, testMode);
	}
	void Start()
	{
		isLoaded = IsLoaded ();
	}

	#region interstitial Implementation
	public bool IsLoaded()
	{
		return isLoaded = Advertisement.IsReady (placementID);
	}
	public void ShowInterstitial()
	{
		Debug.Log("Show Rewarded Video Ad");
		Advertisement.Show (placementID);
	}

	#endregion

	#region Rewarded Video Impletementation
	// Implement IUnityAdsListener interface methods:
	public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {

		// Define conditional logic for each ad completion status:
		if (showResult == ShowResult.Finished) {
			SetReward ();
			IsLoaded ();
			// Reward the user for watching the ad to completion.
		} else if (showResult == ShowResult.Skipped) {

			// Do not reward the user for skipping the ad.
			IsLoaded();
		} else if (showResult == ShowResult.Failed) {

			IsLoaded ();
			Debug.LogWarning ("The ad did not finish due to an error.");
		}
	}

	public bool IsRewardedLoaded()
	{
		return isRewardedLoaded;
	}
	public void OnUnityAdsReady (string placementId) {
		// If the ready Placement is rewarded, show the ad:
		if (placementId == myRewardedPlacementId) {
			isRewardedLoaded = true;

		}
	}
	public void ShowRewardedVideo(int gold,int gems, bool twiceIt)
	{
		
		this.gold = gold;
		this.gems = gems;
		this.twiceTheReward = twiceIt;
		Debug.Log("Show it to request an ad.");
		if (isRewardedLoaded) {
			Advertisement.Show (myRewardedPlacementId);
		}
		else
		{
			Debug.Log("Ad not loaded. Load it to request an ad.");
		}
	}
	public void ShowRewardedAd()
	{
		Debug.Log("Show Rewarded Video Ad");
		Advertisement.Show (myRewardedPlacementId);
	}
	public void SetReward()
	{
		Debug.Log(" gold"+gold.ToString()+" gems"+gems.ToString());
		if (gold != 0 && gems == 0) {

			int goldsLocal = EconomyController.GetInstance ().GetGold () + gold;
			EconomyController.GetInstance ().SetGold (goldsLocal);
			CurrencyPopUpManager.GetInstance ().ShowPop (0, gold);

		} else if (gold == 0 && gems != 0) {

			int gemLocal = EconomyController.GetInstance ().GetGems () + gems;
			EconomyController.GetInstance ().SetGems (gemLocal);
			CurrencyPopUpManager.GetInstance ().ShowPop (gems, 0);

		} else if (gold != 0 && gems != 0) {
			int multiplier = 1;
			if (twiceTheReward)
				multiplier = 2;

			int goldsLocal = EconomyController.GetInstance ().GetGold () + (gold);
			EconomyController.GetInstance ().SetGold (goldsLocal);

			int gemLocal = EconomyController.GetInstance ().GetGems () + (gems );
			EconomyController.GetInstance ().SetGems (gemLocal);
			CurrencyPopUpManager.GetInstance ().ShowPop (gems * multiplier, gold * multiplier);
		}

		if (DefaultMainMenuScreen.GetInstance ().comingFromAdToBuyStarterPack) {
			PlayerPrefs.SetInt("StarterPackBoughtFromAd",1);
			DefaultMainMenuScreen.GetInstance().starterPackBtnBoughtByAd.SetActive(false);
			DefaultMainMenuScreen.GetInstance().starterPackInAppBtn.SetActive(true);
			DefaultMainMenuScreen.GetInstance ().LoaderPanel.SetActive (false);
		}
	}
	public void OnUnityAdsDidError (string message) {
		// Log the error.
	}

	public void OnUnityAdsDidStart (string placementId) {
		// Optional actions to take when the end-users triggers an ad.
	} 

	#endregion

}
