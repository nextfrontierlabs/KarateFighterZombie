using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyController : MonoBehaviour {



	private static EconomyController instance;

	public const int threeStarWinGoldForStoryMode = 1000;
	public  const int twoStarWinGoldForStoryMode = 750;
	public  const int oneStarWinGoldForStoryMode = 500;

	public  const int threeStarWinGoldForChallengeMode = 700;
	public  const int twoStarWinGoldForChallengeMode = 500;
	public  const int oneStarWinGoldForChallengeMode = 300;


	public const  int threeStarWinGemForStoryMode = 10;
	public const int twoStarWinGemForStoryMode = 7;
	public const int oneStarWinGemForStoryMode = 5;

	public const int threeStarWinGemForChallengeMode = 5;
	public const int twoStarWinGemForChallengeMode = 3;
	public const int oneStarWinGemForChallengeMode = 2;
	public CurrencyUpdater[] currencyUpdaters;

	float spentTime;
	void Awake()
	{
		instance = this;
		//Only Hunk is unlocked
		PlayerPrefs.SetInt ("Characters Bought3",1);//Only Hunk is unlocked
//		EconomyController.GetInstance().SetGold(100000);
//		EconomyController.GetInstance().SetGems(100000);
		spentTime = PlayerPrefs.GetFloat("TimeSpentOnGame");
	}

	public void Start()
	{
		currencyUpdaters = FindObjectsOfType <CurrencyUpdater>() as CurrencyUpdater[];
	}
	void Update()
	{
		spentTime = Time.deltaTime + spentTime;

		PlayerPrefs.SetFloat ("TimeSpentOnGame",spentTime);
		if (spentTime >= 600.0f) {
			if (PlayerPrefs.GetInt ("TimeSpentOnGame10Done") == 0) {
				GPGManager.GetInstance ().UnlockAchievementPlayGamesFor10Minutes ();
				PlayerPrefs.SetInt ("TimeSpentOnGame10Done", 1);
			}
		}
		else if (spentTime >= 1800.0f) {
			if (PlayerPrefs.GetInt ("TimeSpentOnGame30Done") == 0) {
				GPGManager.GetInstance ().UnlockAchievementPlayGamesFor30Minutes ();
				PlayerPrefs.SetInt ("TimeSpentOnGame30Done", 1);
			}
		}
		else if (spentTime >= 3600.0f) {
			if (PlayerPrefs.GetInt ("TimeSpentOnGame60Done") == 0) {
				GPGManager.GetInstance ().UnlockAchievementPlayGamesFor60Minutes ();
				PlayerPrefs.SetInt ("TimeSpentOnGame60Done", 1);
			}
		}

	}
	public static EconomyController GetInstance()
	{
		return instance;
	}

	public void SetGems(int gems)
	{
		PlayerPrefs.SetInt ("Gems",gems);
		PlayerPrefs.Save ();
		Start ();
		foreach (CurrencyUpdater updater in currencyUpdaters) {
			updater.SetCurrencyInTopBar ();
		}
		GPGManager.GetInstance ().ReportScoreGem ();
	}
	public int GetGems()
	{
		return PlayerPrefs.GetInt ("Gems");
	}

	public void SetGold(int gold)
	{
		PlayerPrefs.SetInt ("Gold",gold);
		PlayerPrefs.Save ();
		Start ();
		foreach (CurrencyUpdater updater in currencyUpdaters) {
			updater.SetCurrencyInTopBar ();
		}
		GPGManager.GetInstance ().ReportScoreGold ();
	}
	public int GetGold()
	{
		return PlayerPrefs.GetInt ("Gold");
	}

	public void SetChallengeModeWins(int challengeModeWins)
	{
		PlayerPrefs.SetInt ("ChallengeModeWins", PlayerPrefs.GetInt ("ChallengeModeWins")+challengeModeWins);
		PlayerPrefs.Save ();
		switch(PlayerPrefs.GetInt ("ChallengeModeWins"))
		{
		case 1:
			GPGManager.GetInstance ().UnlockAchievementWin1FightInChallengeMode ();
			break;
		case 5:
			GPGManager.GetInstance ().UnlockAchievementWin5FightInChallengeMode ();
			break;
		case 15:
			GPGManager.GetInstance ().UnlockAchievementWin15FightInChallengeMode ();
			break;
		}
		GPGManager.GetInstance ().ReportScoreChallengeModeWins();
	}
	public void SetMultiplayerModeWins(int challengeModeWins)
	{
		PlayerPrefs.SetInt("MultiplayerModeWins", PlayerPrefs.GetInt("MultiplayerModeWins") + challengeModeWins);
		PlayerPrefs.Save();
		GPGManager.GetInstance().ReportScoreMultiplayerModeWins();
	}
	public int GetChallengeModeWins()
	{
		return PlayerPrefs.GetInt ("ChallengeModeWins");
	}
	public int GetMultiplayerModeWins()
	{
		return PlayerPrefs.GetInt("MultiplayerModeWins");
	}

	public void SetStoryModeWins(int storyModeWins)
	{
		PlayerPrefs.SetInt ("StoryModeWins", PlayerPrefs.GetInt ("StoryModeWins")+storyModeWins);
		PlayerPrefs.Save ();
		switch(PlayerPrefs.GetInt ("StoryModeWins"))
		{
		case 1:
			GPGManager.GetInstance ().UnlockAchievementWin1FightInStoryMode ();
			break;
		case 10:
			GPGManager.GetInstance ().UnlockAchievementWin10FightInStoryMode ();
			break;
		case 15:
			GPGManager.GetInstance ().UnlockAchievementWinAllFightInStoryMode ();
			break;
		}
		GPGManager.GetInstance ().ReportScoreStoryModeWins();
	}
	public int GetStoryModeWins()
	{
		return PlayerPrefs.GetInt ("StoryModeWins");
	}
	public void SetPerfectWins(int perfectWins)
	{
		PlayerPrefs.SetInt ("PerfectWins", PlayerPrefs.GetInt ("PerfectWins")+perfectWins);
		PlayerPrefs.Save ();
		switch(PlayerPrefs.GetInt ("PerfectWins"))
		{
		case 1:
			GPGManager.GetInstance ().UnlockAchievementWin1FightPerfect ();
			break;
		case 5:
			GPGManager.GetInstance ().UnlockAchievementWin5FightPerfect ();
			break;
		case 15:
			GPGManager.GetInstance ().UnlockAchievementWin15FightPerfect ();
			break;
		}
		GPGManager.GetInstance ().ReportScorePerfectWins();
	}
	public int GetPerfectWins()
	{
		return	PlayerPrefs.GetInt ("PerfectWins");
	}
	public void LooseFight(int lost)
	{
		PlayerPrefs.SetInt ("Lost", PlayerPrefs.GetInt ("Lost") + lost);
		PlayerPrefs.Save ();

		switch(PlayerPrefs.GetInt ("Lost")){
		case 1:
		GPGManager.GetInstance().UnlockAchievementFailed1Time();
		break;
		case 5:
			GPGManager.GetInstance().UnlockAchievementFailed5Time();
			break;
		case 15:
			GPGManager.GetInstance().UnlockAchievementFailed15Time();
			break;

		}
	}
}
