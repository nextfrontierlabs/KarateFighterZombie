using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityRewardedAd : MonoBehaviour, IUnityAdsListener {


	public string gameId ;
	public string myRewardedPlacementId = "rewardedVideo";
	bool testMode = true;
	public bool isRewardedLoaded = false;


	public int gems;
	public int gold;
	public bool twiceTheReward;

	// Initialize the Ads listener and service:
	void Awake () {
		Advertisement.AddListener (this);
		Advertisement.Initialize (gameId, testMode);

	}

	// Implement IUnityAdsListener interface methods:
	public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
		
		// Define conditional logic for each ad completion status:
		if (showResult == ShowResult.Finished) {
			SetReward ();
			// Reward the user for watching the ad to completion.
		} else if (showResult == ShowResult.Skipped) {
			
			// Do not reward the user for skipping the ad.
		} else if (showResult == ShowResult.Failed) {
			
			Debug.LogWarning ("The ad did not finish due to an error.");
		}
	}

	public bool IsLoaded()
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
			CurrencyPopUpManager.GetInstance ().ShowPop (0, gold * multiplier);

			int gemLocal = EconomyController.GetInstance ().GetGems () + (gems );
			EconomyController.GetInstance ().SetGems (gemLocal);
			CurrencyPopUpManager.GetInstance ().ShowPop (gems * multiplier, 0);
		}


	}
	public void OnUnityAdsDidError (string message) {
		// Log the error.
	}

	public void OnUnityAdsDidStart (string placementId) {
		// Optional actions to take when the end-users triggers an ad.
	} 
}
