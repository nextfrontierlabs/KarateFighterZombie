using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GPGManager : MonoBehaviour {


	private static GPGManager gPGManager;

	public static GPGManager GetInstance()
	{
		return gPGManager;
	}

	public bool loggedIn;
	void Awake()
	{
		gPGManager = this;
		 

		PlayGamesPlatform.Activate ();

	}

	void Start()
	{
		Login ();
	}
	public void Login()
	{
		if (Social.localUser.authenticated) {
		   
			return;
		}
		Social.localUser.Authenticate ((bool succes) =>
			{
				if(succes)
				{
					Debug.Log("Logged In");
					loggedIn = true;

				}
				else
				{
					Debug.Log("Not Logged In");
					loggedIn = false;

				}
			});
	}
	public void ShowLeaderboard()
	{
		Social.ShowLeaderboardUI ();
	}
	#region leaderboard
	public void ReportScoreChallengeModeWins()
	{
		Social.ReportScore (EconomyController.GetInstance().GetChallengeModeWins(),GPGSIds.leaderboard_challenge_mode_wins,(bool success)=>{
			if(success)
			{
				Debug.Log("Scores Updated");
			}
			else
			{
				Debug.Log("Scores unable to Updated");
			}
		});
	}
	public void ReportScoreStoryModeWins()
	{
		Social.ReportScore (EconomyController.GetInstance().GetStoryModeWins(),GPGSIds.leaderboard_story_mode_wins,(bool success)=>{
			if(success)
			{
				Debug.Log("Scores Updated");
			}
			else
			{
				Debug.Log("Scores unable to Updated");
			}
		});
	}
	public void ReportScorePerfectWins()
	{
		Social.ReportScore (EconomyController.GetInstance().GetPerfectWins(),GPGSIds.leaderboard_perfect_wins,(bool success)=>{
			if(success)
			{
				Debug.Log("Scores Updated");
			}
			else
			{
				Debug.Log("Scores unable to Updated");
			}
		});
	}
	public void ReportScoreGold()
	{
		Social.ReportScore (EconomyController.GetInstance().GetGold(),GPGSIds.leaderboard_gold,(bool success)=>{
			if(success)
			{
				Debug.Log("Scores Updated");
			}
			else
			{
				Debug.Log("Scores unable to Updated");
			}
		});
	}
	public void ReportScoreGem()
	{
		Social.ReportScore (EconomyController.GetInstance().GetGems(),GPGSIds.leaderboard_gems,(bool success)=>{
			if(success)
			{
				Debug.Log("Scores Updated");
			}
			else
			{
				Debug.Log("Scores unable to Updated");
			}
		});
	}

	public void ReportScoreMultiplayerModeWins()
	{
		Social.ReportScore(EconomyController.GetInstance().GetMultiplayerModeWins(), GPGSIds.leaderboard_multiplayer_mode_wins, (bool success) => {
			if (success)
			{
				Debug.Log("Scores Updated");
			}
			else
			{
				Debug.Log("Scores unable to Updated");
			}
		});
	}
	#endregion
	#region Achievements
	public void ShowAchievements()
	{
		Social.ShowAchievementsUI ();
	}
	public void UnlockAchievementGotANewFighter()
	{
		Social.ReportProgress (GPGSIds.achievement_new_fighter,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_new_fighter)==0){
				int gem = EconomyController.GetInstance().GetGems() +  10;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(10,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_new_fighter,1);
				}
			}	
			else
			{
				
			}
		});
	}
	public void UnlockAchievementPlayGamesFor10Minutes()
	{
		Social.ReportProgress (GPGSIds.achievement_10_mins_today,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_10_mins_today)==0){
				int gem = EconomyController.GetInstance().GetGems() +  10;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(10,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_10_mins_today,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementPlayGamesFor30Minutes()
	{
		Social.ReportProgress (GPGSIds.achievement_30_min_today,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_30_min_today)==0){
				int gem = EconomyController.GetInstance().GetGems() +  20;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(20,0);
				PlayerPrefs.SetInt(GPGSIds.achievement_30_min_today,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementPlayGamesFor60Minutes()
	{
		Social.ReportProgress (GPGSIds.achievement_60_min_today,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_60_min_today)==0){
				int gem = EconomyController.GetInstance().GetGems() +  30;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(30,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_60_min_today,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementWin5000GoldInChallengeMode()
	{
		Social.ReportProgress (GPGSIds.achievement_wealth,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_wealth)==0){
				int gem = EconomyController.GetInstance().GetGems() +  15;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(15,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_wealth,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementWinMillionGoldInChallengeMode()
	{
		Social.ReportProgress (GPGSIds.achievement_millionaire,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_millionaire)==0){
				int gem = EconomyController.GetInstance().GetGems() +  30;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(30,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_millionaire,1);
				}

			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementWin1FightInChallengeMode()
	{
		
		Social.ReportProgress (GPGSIds.achievement_doing_well,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_doing_well)==0){
				int gem = EconomyController.GetInstance().GetGems() +  10;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(10,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_doing_well,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementWin5FightInChallengeMode()
	{
		Social.ReportProgress (GPGSIds.achievement_excellent,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_excellent)==0){
				int gem = EconomyController.GetInstance().GetGems() +  15;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(15,0);
				PlayerPrefs.SetInt(GPGSIds.achievement_excellent,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementWin15FightInChallengeMode()
	{
		Social.ReportProgress (GPGSIds.achievement_marvellous,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_marvellous)==0){
				int gem = EconomyController.GetInstance().GetGems() +  20;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(20,0);
				PlayerPrefs.SetInt(GPGSIds.achievement_marvellous,1);
				}
			}	
			else
			{

			}
		});
	}

	public void UnlockAchievementWin1FightInStoryMode()
	{
		Social.ReportProgress (GPGSIds.achievement_newbie_of_fight,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_newbie_of_fight)==0){
				int gem = EconomyController.GetInstance().GetGems() +  10;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(10,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_newbie_of_fight,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementWin10FightInStoryMode()
	{
		Social.ReportProgress (GPGSIds.achievement_fans_of_fight,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_fans_of_fight)==0){
				int gem = EconomyController.GetInstance().GetGems() +  20;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(20,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_fans_of_fight,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementWinAllFightInStoryMode()
	{
		Social.ReportProgress (GPGSIds.achievement_legend_of_fight,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_legend_of_fight)==0){
				int gem = EconomyController.GetInstance().GetGems() +  30;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(30,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_legend_of_fight,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGot2NewFighters()
	{
		Social.ReportProgress (GPGSIds.achievement_strong,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_strong)==0){
				int gem = EconomyController.GetInstance().GetGems() +  20;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(20,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_strong,1);

				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGot3NewFighters()
	{
		Social.ReportProgress (GPGSIds.achievement_fighter_lover,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_fighter_lover)==0){
				int gem = EconomyController.GetInstance().GetGems() +  30;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(30,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_fighter_lover,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGot5NewFighters()
	{
		Social.ReportProgress (GPGSIds.achievement_aspirant,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_aspirant)==0){
				int gem = EconomyController.GetInstance().GetGems() +  50;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(50,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_aspirant,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGot7NewFighters()
	{
		Social.ReportProgress (GPGSIds.achievement_legendary,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_legendary)==0){
				int gem = EconomyController.GetInstance().GetGems() +  70;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(70,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_legendary,1);

				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGot10NewFighters()
	{
		Social.ReportProgress (GPGSIds.achievement_true_fans,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_true_fans)==0){
				int gem = EconomyController.GetInstance().GetGems() +  100;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(100,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_true_fans,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGotAllFighters()
	{
		Social.ReportProgress (GPGSIds.achievement_great_collector,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_great_collector)==0){
				int gem = EconomyController.GetInstance().GetGems() +  120;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(120,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_great_collector,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementFailed1Time()
	{
		Social.ReportProgress (GPGSIds.achievement_r_u_ok,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_r_u_ok)==0){
				int gem = EconomyController.GetInstance().GetGems() +  5;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(5,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_r_u_ok,1);

				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementFailed5Time()
	{
		Social.ReportProgress (GPGSIds.achievement_r_u_disabled,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_r_u_disabled)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  10;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(10,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_r_u_disabled,1);
			}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementFailed15Time()
	{
		Social.ReportProgress (GPGSIds.achievement_r_u_dead,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_r_u_dead)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  15;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(15,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_r_u_dead,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGot1SpecialMove()
	{
		Social.ReportProgress (GPGSIds.achievement_good_start,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_good_start)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  5;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(5,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_good_start,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGot2SpecialMove()
	{
		Social.ReportProgress (GPGSIds.achievement_good_game,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_good_game)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  10;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(10,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_good_game,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGot3SpecialMove()
	{
		Social.ReportProgress (GPGSIds.achievement_gain,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_gain)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  15;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(15,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_gain,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGot4SpecialMove()
	{
		Social.ReportProgress (GPGSIds.achievement_surprise,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_surprise)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  20;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(20,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_surprise,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGot5SpecialMove()
	{
		Social.ReportProgress (GPGSIds.achievement_harvest,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_harvest)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  25;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(25,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_harvest,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGot6SpecialMove()
	{
		Social.ReportProgress (GPGSIds.achievement_shining,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_shining)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  30;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(30,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_shining,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGotAPowerShot()
	{
		Social.ReportProgress (GPGSIds.achievement_richer,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_richer)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  20;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(20,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_richer,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementGot2PowerShots()
	{
		Social.ReportProgress (GPGSIds.achievement_no_desire,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_no_desire)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  40;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(40,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_no_desire,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementSpent30Gold()
	{
		Social.ReportProgress (GPGSIds.achievement_alms,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_alms)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  5;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(5,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_alms,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementSpent100Gold()
	{
		Social.ReportProgress (GPGSIds.achievement_on_own_feet,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_on_own_feet)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  10;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(10,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_on_own_feet,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementSpent3000Gold()
	{
		
		Social.ReportProgress (GPGSIds.achievement_follow_heart,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_follow_heart)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  20;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(20,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_follow_heart,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementWin1FightPerfect()
	{
		Social.ReportProgress (GPGSIds.achievement_invisable,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_invisable)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  10;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(10,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_invisable,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementWin5FightPerfect()
	{
		Social.ReportProgress (GPGSIds.achievement_transparent,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_transparent)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  50;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(50,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_transparent,1);
				}
			}	
			else
			{

			}
		});
	}
	public void UnlockAchievementWin15FightPerfect()
	{
		Social.ReportProgress (GPGSIds.achievement_no_one,100, (bool success) => {
			if(success)
			{
				if(PlayerPrefs.GetInt(GPGSIds.achievement_no_one)==0)
				{
				int gem = EconomyController.GetInstance().GetGems() +  100;
				EconomyController.GetInstance().SetGems(gem);
				CurrencyPopUpManager.GetInstance().ShowPop(100,0);
					PlayerPrefs.SetInt(GPGSIds.achievement_no_one,1);
				}
			}	
			else
			{

			}
		});
	}

	#endregion



}
