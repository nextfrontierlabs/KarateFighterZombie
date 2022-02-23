using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class DailyBonusManager : MonoBehaviour {

    DateTime previous;
    DateTime today;

	public GameObject DailyBonusMenu;

    public float showTimer = 2f;
    public float vanishTimer = 3;

    public Text GrenText, medalText, grenadeCount, armorCount;

    public List<DailyBonus> _dailyBonuses;

    public AudioClip ClaimClip;
	public Button claimButton;
    void Awake()
    {
        //ResetDailyBonus();
        //ResetPrevious();
        Invoke("DailyBonusStart", showTimer);
    }

    public int dummyValue = 1;
    void DailyBonusStart()
    {
		Debug.Log ("DailyBonusStart");
        today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        if (PlayerPrefs.GetInt("FirstPlay") < 1)//dont allow daily bonus to be poped up
        {
			PlayerPrefs.SetInt ("FirstPlay", 1);
        }
        else//If daily Daily bonus has/have been awarded in past
        {
            previous = new DateTime(PlayerPrefs.GetInt("BonusYear",2017), PlayerPrefs.GetInt("BonusMonth",1), PlayerPrefs.GetInt("BonusDay",1));
            TimeSpan diff = CompareDates(previous, today);
            if (diff.Days >= 1)
            {
                
				int dailyClaim = PlayerPrefs.GetInt("DailyClaim");

                if (diff.Days == 1) // 1st day and consecutive days.
                {
					
					if (dailyClaim >= 10)
					{
						ResetDailyBonus();
						dailyClaim = PlayerPrefs.GetInt("DailyClaim");
						//ArrangeDailyBonus(dailyClaim + 1);
					}

					else if (dailyClaim >= 2)
					{
						int offset = dailyClaim - 2;
						print("Move: " + 324 * offset);
			
					}

				

                    ArrangeDailyBonus(dailyClaim + 1);
                    //AwardDailyBonus(today, bonusNumber);
                    print("Difference: " + diff.Days);
                }
                else // missed consecutive days. Reset daily bonus from day one
                {
                    ResetDailyBonus();
					dailyClaim = PlayerPrefs.GetInt("DailyClaim");
                    ArrangeDailyBonus(dailyClaim + 1);
                }
            }
        }
    }
    int day;
    public void ArrangeDailyBonus(int _dailyClaim)
    {
		Camera.main.depth = 3;
        day = _dailyClaim;
        for (int i = 1; i < _dailyBonuses.Count + 1; i++)
        {
            if (i < _dailyClaim)
            {
                _dailyBonuses[i-1]._claimState = DailyBonus.ClaimState.claimed;
                _dailyBonuses[i-1].StateUpdate();
            }
            else if (i == _dailyClaim)
            {
                _dailyBonuses[i-1]._claimState = DailyBonus.ClaimState.readyToclaim;
                _dailyBonuses[i-1].StateUpdate();
            }
            else
            {
                _dailyBonuses[i-1]._claimState = DailyBonus.ClaimState.notReadyToClaim;
                _dailyBonuses[i-1].StateUpdate();
            }
        }
        DailyBonusMenu.SetActive(true);

	
    }

    public TimeSpan CompareDates(DateTime dateOld, DateTime dateNew)
    {
        print("Today: "+dateNew);
        print("Previous: "+dateOld);
        return dateNew - dateOld;
    }

    IEnumerator ShowPopUp(float seconds, int day)
    {
        yield return new WaitForSeconds(seconds);
      
        _dailyBonuses[day - 1].ShowPopUpView(true);
		Invoke ("CloseDailyBonus",3);
    }



    public void ClaimBonus()
    {
		
        print("Claimed Bonus for day: " + day);
        PlayerPrefs.SetInt("DailyClaim",day);
        StartCoroutine(ShowPopUp(0.8f, day));
        _dailyBonuses[day - 1]._claimState = DailyBonus.ClaimState.claimed;
        _dailyBonuses[day - 1].StateUpdate();
        AwardDailyBonus(today, 1);
		claimButton.interactable = false;
        switch (day)
        {
		case 1:
			AudioSource.PlayClipAtPoint (ClaimClip, transform.position);
			{
				int gold = EconomyController.GetInstance ().GetGold () + 500;
				EconomyController.GetInstance ().SetGold (gold);
				CurrencyPopUpManager.GetInstance ().ShowPop (0,500);
				PlayerPrefs.Save ();
			}
                break;
		case 2:
			AudioSource.PlayClipAtPoint (ClaimClip, transform.position);
			{
				int gems = EconomyController.GetInstance ().GetGems () + 10;
				EconomyController.GetInstance ().SetGems (gems);
				CurrencyPopUpManager.GetInstance ().ShowPop (10,0);
				PlayerPrefs.Save ();
			}
                break;
		case 3:
			AudioSource.PlayClipAtPoint (ClaimClip, transform.position);
			{
				int gold = EconomyController.GetInstance ().GetGold () + 1000;
				EconomyController.GetInstance ().SetGold (gold);
				CurrencyPopUpManager.GetInstance ().ShowPop (0,1000);
				PlayerPrefs.Save ();
			}
                break;
            case 4:
                AudioSource.PlayClipAtPoint(ClaimClip, transform.position);
			{
				int gems = EconomyController.GetInstance ().GetGems () + 15;
				EconomyController.GetInstance ().SetGems (gems);
				CurrencyPopUpManager.GetInstance ().ShowPop (15,0);
				PlayerPrefs.Save ();
			}
           
               
                break;
            case 5:
                AudioSource.PlayClipAtPoint(ClaimClip, transform.position);
			{
				int gold = EconomyController.GetInstance ().GetGold () + 1500;
				EconomyController.GetInstance ().SetGold (gold);
				CurrencyPopUpManager.GetInstance ().ShowPop (0,1500);
				PlayerPrefs.Save ();
			}
                break;
            case 6:
                AudioSource.PlayClipAtPoint(ClaimClip, transform.position);
			{
				int gems = EconomyController.GetInstance ().GetGems () + 20;
				EconomyController.GetInstance ().SetGems (gems);
				CurrencyPopUpManager.GetInstance ().ShowPop (20,0);
				PlayerPrefs.Save ();
			}
                break;
            case 7:
                AudioSource.PlayClipAtPoint(ClaimClip, transform.position);
			{
				int gold = EconomyController.GetInstance ().GetGold () + 1000;
				EconomyController.GetInstance ().SetGold (gold);
				int gems = EconomyController.GetInstance ().GetGems () + 15;
				EconomyController.GetInstance ().SetGems (gems);
				CurrencyPopUpManager.GetInstance ().ShowPop (15,1000);
				PlayerPrefs.Save ();
			}
                break;

        }

    }

    public void ResetDailyBonus()
    {
        PlayerPrefs.SetInt("DailyClaim", 0);
		PlayerPrefs.Save ();
    }

    public void ResetPrevious()
    {
        PlayerPrefs.SetInt("BonusYear",2000);
        PlayerPrefs.SetInt("BonusMonth", 2);
        PlayerPrefs.SetInt("BonusDay", 4);
    }

    public void CloseDailyBonus()
    {
		Camera.main.depth = 1;
        DailyBonusMenu.SetActive(false);
       

    }

    public void AwardDailyBonus(DateTime day, int dayBonus)
    {

        PlayerPrefs.SetInt("BonusDay", day.Day);
        PlayerPrefs.SetInt("BonusMonth", day.Month);
        PlayerPrefs.SetInt("BonusYear", day.Year);
		PlayerPrefs.Save ();
        previous = new DateTime(PlayerPrefs.GetInt("BonusYear"), PlayerPrefs.GetInt("BonusMonth"), PlayerPrefs.GetInt("BonusDay"));
        print("DAILY BONUS :) :) :)");

    }

    

    public void VanishBonus()
    {
        DailyBonusMenu.SetActive(false);


    }


}
