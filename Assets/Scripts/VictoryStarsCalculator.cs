using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryStarsCalculator : MonoBehaviour {
	public GameObject VictoryStardsHolder;
	public bool storyMode = false;
	public Text gemsTxt;
	public Text goldTxt;
	public GameObject next;
	public GameObject home;
	int currentGold=0;
	int currentGem=0;
	void OnEnable()
	{
		
		//UFE.config.aiOptions.selectedDifficulty;
		if (StoryModeProgression.level <= 0.333f) {
			VictoryStardsHolder.transform.GetChild (0).gameObject.SetActive (true);
			if (storyMode) {
				int gold = EconomyController.GetInstance ().GetGold () + EconomyController.oneStarWinGoldForStoryMode ;  
				EconomyController.GetInstance ().SetGold (gold);  
				int gems = EconomyController.GetInstance ().GetGems () + EconomyController.oneStarWinGemForStoryMode ;  
				EconomyController.GetInstance ().SetGems (gems);  
				gemsTxt.text = EconomyController.oneStarWinGemForStoryMode.ToString ()+ " Gems";
				goldTxt.text = EconomyController.oneStarWinGoldForStoryMode.ToString ()+ " Gold" ;

				currentGold = EconomyController.oneStarWinGoldForStoryMode;
				currentGem =  EconomyController.oneStarWinGemForStoryMode;
			}
			else {
				int gold = EconomyController.GetInstance ().GetGold () + EconomyController.oneStarWinGoldForChallengeMode ;  
				EconomyController.GetInstance ().SetGold (gold);  
				int gems = EconomyController.GetInstance ().GetGems () + EconomyController.oneStarWinGemForChallengeMode ;  
				EconomyController.GetInstance ().SetGems (gems); 
				gemsTxt.text = EconomyController.oneStarWinGemForChallengeMode.ToString ()+ " Gems";
				goldTxt.text = EconomyController.oneStarWinGoldForChallengeMode.ToString ()+ " Gold" ;

				currentGold = EconomyController.oneStarWinGoldForChallengeMode;
				currentGem =  EconomyController.oneStarWinGemForChallengeMode;
			}
		} else if (StoryModeProgression.level > 0.333f && StoryModeProgression.level <= 0.888f) {
			
			VictoryStardsHolder.transform.GetChild (1).gameObject.SetActive (true);
			if (storyMode) {
				int gold = EconomyController.GetInstance ().GetGold () + EconomyController.twoStarWinGoldForStoryMode ;  
				EconomyController.GetInstance ().SetGold (gold);  
				int gems = EconomyController.GetInstance ().GetGems () + EconomyController.twoStarWinGemForStoryMode ;  
				EconomyController.GetInstance ().SetGems (gems);  
				gemsTxt.text = EconomyController.twoStarWinGemForStoryMode.ToString ()+ " Gems";
				goldTxt.text = EconomyController.twoStarWinGoldForStoryMode.ToString ()+ " Gold" ;

				currentGold = EconomyController.twoStarWinGoldForStoryMode;
				currentGem =  EconomyController.twoStarWinGemForStoryMode;

			}
			else {
				int gold = EconomyController.GetInstance ().GetGold () + EconomyController.twoStarWinGoldForChallengeMode ;  
				EconomyController.GetInstance ().SetGold (gold);  
				int gems = EconomyController.GetInstance ().GetGems () + EconomyController.twoStarWinGemForChallengeMode ;  
				EconomyController.GetInstance ().SetGems (gems);
				gemsTxt.text = EconomyController.twoStarWinGemForChallengeMode.ToString ()+ " Gems";
				goldTxt.text = EconomyController.twoStarWinGoldForChallengeMode.ToString ()+ " Gold" ;

				currentGold = EconomyController.twoStarWinGoldForChallengeMode;
				currentGem =  EconomyController.twoStarWinGemForChallengeMode;
			}
		} else if (StoryModeProgression.level > 0.888f) {
			
			VictoryStardsHolder.transform.GetChild (2).gameObject.SetActive (true);
			if (storyMode) {
				int gold = EconomyController.GetInstance ().GetGold () + EconomyController.threeStarWinGoldForStoryMode ;  
				EconomyController.GetInstance ().SetGold (gold);  
				int gems = EconomyController.GetInstance ().GetGems () + EconomyController.threeStarWinGemForStoryMode ;  
				EconomyController.GetInstance ().SetGems (gems);  
				gemsTxt.text = EconomyController.threeStarWinGemForStoryMode.ToString ()+ " Gems";
				goldTxt.text = EconomyController.threeStarWinGoldForStoryMode.ToString ()+ " Gold" ;

				currentGold = EconomyController.threeStarWinGoldForStoryMode;
				currentGem =  EconomyController.threeStarWinGemForStoryMode;
			}
			else {
				int gold = EconomyController.GetInstance ().GetGold () + EconomyController.threeStarWinGoldForChallengeMode ;  
				EconomyController.GetInstance ().SetGold (gold);  
				int gems = EconomyController.GetInstance ().GetGems () + EconomyController.threeStarWinGemForChallengeMode ;  
				EconomyController.GetInstance ().SetGems (gems);  
				gemsTxt.text = EconomyController.threeStarWinGemForChallengeMode.ToString ()+ " Gems";
				goldTxt.text = EconomyController.threeStarWinGoldForChallengeMode.ToString ()+ " Gold" ;

				currentGold = EconomyController.threeStarWinGoldForChallengeMode;
				currentGem =  EconomyController.threeStarWinGemForChallengeMode;
			}
		}
		if (storyMode) {
			EconomyController.GetInstance ().SetStoryModeWins (1);
		}
		else{
			if (DefaultCharacterSelectionScreen.isMultiplayerSelected)
			{
				EconomyController.GetInstance().SetMultiplayerModeWins(1);
			}
			else
				EconomyController.GetInstance().SetChallengeModeWins(1);

		}
		if (StoryModeProgression.level > 0.99f) {
			EconomyController.GetInstance ().SetPerfectWins (1);
		}
	

		if (storyMode) {
			if (PlayerPrefs.GetInt ("levels") == 14) {
				next.SetActive (false);
				home.SetActive (true);
			}
		}
	}
	public void WatchAdForTwiceTheRewards(bool twiceIt = true)
	{
		Debug.Log (currentGold+" currentGold");
		Debug.Log (currentGem+" currentGem");

#if UNITY_IOS || UNITY_ANDROID
    
         
        if (AdsManager.GetInstance ().unityRewardedAd.isLoaded) {
				AdsManager.GetInstance ().unityRewardedAd.ShowRewardedVideo (currentGold, currentGem, twiceIt);
				//AdsManager.GetInstance ().facebookRewardedAd.LoadRewardedVideo ();
			}
		else if (AdsManager.GetInstance ().admobRewardedAd.isRewardedAdLoaded)
			{
				AdsManager.GetInstance ().admobRewardedAd.ShowRewardedVideo (currentGold, currentGem, twiceIt);
				//AdsManager.GetInstance ().facebookRewardedAd.LoadRewardedVideo ();
			}
	     


		#endif
	}
}
