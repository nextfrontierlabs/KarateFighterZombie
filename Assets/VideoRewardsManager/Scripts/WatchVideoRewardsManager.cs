using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using GoogleMobileAds.Api;
#if UNITY_IPHONE || UNITY_ANDROID
using UnityEngine.Advertisements;
#endif

public class WatchVideoRewardsManager : MonoBehaviour
{

    public string RewardedAdID_Android = "ca-app-pub-2814647089623348/7642153714";
    public string RewardedAdID_IOS = "ca-app-pub-2814647089623348/3162633974";
    public RewardBasedVideoAd rewardBasedVideoNew;   // Admob Rewarded

    public string unity_ID_Android = "1272223";
    public string unity_ID_IOS = "1272224";
    public bool unityAdsTestMode = true;

    //---------------------------------
    public GameObject LoadingAdGO;
    public Image RewardGivenLogo;
    public Text RewardGivenText;
    public GameObject RewardHasbeenGivenGO;

    //---------------------------------

    public Text VideosWatchedCountText;
    public Text VideosAdsLeftCountText;
    public Slider SliderHandle;
    //---------------------------------
    public Sprite NormalButtonBGSprite;
    public Sprite HighlightedButtonBGSprite;
    //---------------------------------
    public GameObject[] WatchedVideosTick;
    public float[] targetSliderValues;
    //---------------------------------
    public RewardButtonData[] rewardButtonsData;
    //---------------------------------
    public RewardRefillData [] RewardRefills;
    public RewardRefillData [] RandomRewardRefills;
    public RewardRefillData [] CurrentRewardRefill;
    [HideInInspector]
    public RewardsEnum currentReward = RewardsEnum.None;
    [HideInInspector]
    public int CurrentRewardAmountToBeGiven;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteKey("RewardRefillCurrentNum");
        //PlayerPrefs.DeleteKey("RewardVideosWatched");
        //PlayerPrefs.DeleteKey("RewardListComplete");

        if (!PlayerPrefs.HasKey("RewardRefillCurrentNum"))
            PlayerPrefs.SetInt("RewardRefillCurrentNum", 0);

        if (!PlayerPrefs.HasKey("RewardVideosWatched"))
            PlayerPrefs.SetInt("RewardVideosWatched", 0);

        if(!PlayerPrefs.HasKey ("RewardListComplete"))
            PlayerPrefs.SetString("RewardListComplete", "False");

       


        FindCurrentRefillToUse();
        UpdateRewardScreen();

        AdmobRewardBasedVideo();
        InitializeUnityAds();

    }


    void InitializeUnityAds() {
    #if UNITY_IPHONE || UNITY_ANDROID

        if (Advertisement.isSupported && !Advertisement.isInitialized)
        {
    #if UNITY_IOS
			Advertisement.Initialize (unity_ID_IOS, true);
			//Debug.Log ("ADS INIT CALLED FOR IOS " + unityAdsIOSGameID);
    #else
            Advertisement.Initialize(unity_ID_Android, unityAdsTestMode);
            Debug.Log("ADS INIT CALLED FOR ANDROID " + unity_ID_Android);
    #endif
        }
    #endif
    }

    public bool UnityRewardedAdAvailable(string zone = "rewardedVideo")
    {
    #if UNITY_IPHONE || UNITY_ANDROID

        if (Advertisement.IsReady(zone))
            return true;
        else
            return false;
    #else
		return false;
    #endif

    }

    public void UnityShowRewardedAd(string zone = "rewardedVideo")
    {
    #if UNITY_IPHONE || UNITY_ANDROID
        if (string.Equals(zone, ""))
            zone = null;
        Debug.Log("My ZONE = " + zone);
        var options = new ShowOptions { resultCallback = RewardedAdCallbackhandler };
        if (Advertisement.IsReady(zone))
        {
            Debug.Log("ADs READY");
            Advertisement.Show(zone, options);
        }
        else
        {
            Debug.Log("ADS NOT READY");
        }
    #endif
    }

    #if UNITY_IPHONE || UNITY_ANDROID
    void RewardedAdCallbackhandler(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Ad Finished. Rewarding player...");
                GiveReward();
                break;
            case ShowResult.Skipped:
                Debug.Log("Ad skipped. Son, I am dissapointed in you");
                break;
            case ShowResult.Failed:
                Debug.Log("I swear this has never happened to me before");
                break;
        }
    }
    #endif

    void AdmobRewardBasedVideo() {
#if !UNITY_EDITOR
        this.rewardBasedVideoNew = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardBasedVideoNew.OnAdLoaded += HandleRewardBasedVideoLoadedNew;
        // Called when an ad request failed to load.
        rewardBasedVideoNew.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoadNew;
        // Called when an ad is shown.
        rewardBasedVideoNew.OnAdOpening += HandleRewardBasedVideoOpenedNew;
        // Called when the ad starts to play.
        rewardBasedVideoNew.OnAdStarted += HandleRewardBasedVideoStartedNew;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideoNew.OnAdRewarded += HandleRewardBasedVideoRewardedNew;
        // Called when the ad is closed.
        rewardBasedVideoNew.OnAdClosed += HandleRewardBasedVideoClosedNew;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideoNew.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplicationNew;

        this.RequestRewardBasedVideo();
#endif
    }


    public void HandleRewardBasedVideoLoadedNew(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoadNew(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
        "HandleRewardBasedVideoFailedToLoad event received with message: "
        + args.Message);
        //this.RequestRewardBasedVideo();
    }

    public void HandleRewardBasedVideoOpenedNew(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStartedNew(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosedNew(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        this.RequestRewardBasedVideo();
    }

    public void HandleRewardBasedVideoRewardedNew(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
        "HandleRewardBasedVideoRewarded event received for "
        + amount.ToString() + " " + type);
        GiveReward();
    }

    public void HandleRewardBasedVideoLeftApplicationNew(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }
    private void RequestRewardBasedVideo()
    {

    #if UNITY_EDITOR
            string adUnitId = RewardedAdID_Android;
    #elif UNITY_ANDROID
		    string adUnitId = RewardedAdID_Android;
    #elif UNITY_IPHONE
		    string adUnitId = RewardedAdID_IOS;
    #else
		    string adUnitId = RewardedAdID_Android;
    #endif


        rewardBasedVideoNew.OnAdClosed += RewardBasedVideo_OnAdClosedNew;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideoNew.LoadAd(request, adUnitId);
    }

    private void RewardBasedVideo_OnAdClosedNew(object sender, EventArgs e)
    {
        this.RequestRewardBasedVideo();
    }


    void UpdateRewardScreen() {
        VideosWatchedCountText.text = "0" + PlayerPrefs.GetInt("RewardVideosWatched");
        VideosAdsLeftCountText.text = PlayerPrefs.GetInt("RewardVideosWatched") + "/5";
        if (PlayerPrefs.GetInt("RewardVideosWatched") != 0)
            AddSliderValue = true;
        for (int i = 0; i < WatchedVideosTick.Length ; i++) {

            if (i < PlayerPrefs.GetInt("RewardVideosWatched"))
                WatchedVideosTick[i].SetActive(true);
            else
                WatchedVideosTick[i].SetActive(false);

        }
        //---------------------------------

        for (int i = 0; i < rewardButtonsData.Length; i++)
        {
            rewardButtonsData[i].Heading.text = CurrentRewardRefill[PlayerPrefs.GetInt("RewardRefillCurrentNum")].rewardButtonDetails[i].HeadingText;
            rewardButtonsData[i].CoinsAmount.text = CurrentRewardRefill[PlayerPrefs.GetInt("RewardRefillCurrentNum")].rewardButtonDetails[i].Amount.ToString();
            rewardButtonsData[i].RewardLogo.sprite = CurrentRewardRefill[PlayerPrefs.GetInt("RewardRefillCurrentNum")].rewardButtonDetails[i].Logo;
            rewardButtonsData[i].ParentButton.interactable = false;
            rewardButtonsData[i].ButtonBG.sprite = NormalButtonBGSprite;
            rewardButtonsData[i].RewardSubButton.interactable = false;
            
            if (i < PlayerPrefs.GetInt("RewardVideosWatched")) {
                rewardButtonsData[i].TickGameobject.SetActive(true);
            }else
                rewardButtonsData[i].TickGameobject.SetActive(false);
        }

        // highlighting the next reward button
        rewardButtonsData[PlayerPrefs.GetInt("RewardVideosWatched")].ParentButton.interactable = true;
        rewardButtonsData[PlayerPrefs.GetInt("RewardVideosWatched")].ButtonBG.sprite = HighlightedButtonBGSprite;
        rewardButtonsData[PlayerPrefs.GetInt("RewardVideosWatched")].RewardSubButton.interactable = false;
        rewardButtonsData[PlayerPrefs.GetInt("RewardVideosWatched")].RewardSubButton.interactable = true;
        currentReward = CurrentRewardRefill[PlayerPrefs.GetInt("RewardRefillCurrentNum")].rewardButtonDetails[PlayerPrefs.GetInt("RewardVideosWatched")].RewardType;
        CurrentRewardAmountToBeGiven = CurrentRewardRefill[PlayerPrefs.GetInt("RewardRefillCurrentNum")].rewardButtonDetails[PlayerPrefs.GetInt("RewardVideosWatched")].Amount;

    }

    private bool AddSliderValue = false;
    public void Update()
    {
        if (AddSliderValue && PlayerPrefs.GetInt("RewardVideosWatched") == 0) {
            SliderHandle.value = Mathf.Lerp(SliderHandle.value, 0, 0.20f);

        }
        else if (AddSliderValue && SliderHandle.value < targetSliderValues[PlayerPrefs.GetInt("RewardVideosWatched") - 1])
        {
            SliderHandle.value = Mathf.Lerp(SliderHandle.value, targetSliderValues[PlayerPrefs.GetInt("RewardVideosWatched") - 1], 0.20f);
        }
        else
            AddSliderValue = false;
      
    }

    public void FindCurrentRefillToUse() {
        if (PlayerPrefs.GetString("RewardListComplete") == "False")
        {
            Array.Resize(ref CurrentRewardRefill, 0);
            CurrentRewardRefill = RewardRefills;
        }
        else if (PlayerPrefs.GetString("RewardListComplete") == "True")
        {
            Array.Resize(ref CurrentRewardRefill, 0);
            CurrentRewardRefill = RandomRewardRefills;
        }
    }

    public void WatchVideoForReward() {

        LoadingAdGO.SetActive(true);
        Invoke("CallToWatchRewardedVideo", UnityEngine.Random.Range(1.0f,3.0f));

    }

    void CallToWatchRewardedVideo() {
    #if UNITY_EDITOR
    
        GiveReward();

#elif UNITY_ANDROID || UNITY_IPHONE
     
        if(UnityRewardedAdAvailable())
        {
            UnityShowRewardedAd();
            LoadingAdGO.SetActive(false);
            Debug.LogError("unity rewarded video showing");

        }
       else if (rewardBasedVideoNew.IsLoaded())
		{
		    rewardBasedVideoNew.Show();
            LoadingAdGO.SetActive(false);

            Debug.LogError("admob rewarded video showing");

		}
       else{
            LoadingAdGO.SetActive(false);
            Debug.LogError("no rewarded video available");
        }

#endif
    }

    public int gems;
    public int gold;

    public void GiveReward()
    {

        //---------------------------------UPDATE AS PER REQUIREMENT---------------------------------//

        Debug.Log(" gold" + gold.ToString() + " gems" + gems.ToString());
        if (gold != 0 && gems == 0)
        {

            int goldsLocal = EconomyController.GetInstance().GetGold() + gold;
            EconomyController.GetInstance().SetGold(goldsLocal);
            CurrencyPopUpManager.GetInstance().ShowPop(0, gold);

        }
        else if (gold == 0 && gems != 0)
        {

            int gemLocal = EconomyController.GetInstance().GetGems() + gems;
            EconomyController.GetInstance().SetGems(gemLocal);
            CurrencyPopUpManager.GetInstance().ShowPop(gems, 0);

        }
        else if (gold != 0 && gems != 0)
        {
  

            int goldsLocal = EconomyController.GetInstance().GetGold() + (gold);
            EconomyController.GetInstance().SetGold(goldsLocal);

            int gemLocal = EconomyController.GetInstance().GetGems() + (gems);
            EconomyController.GetInstance().SetGems(gemLocal);
            CurrencyPopUpManager.GetInstance().ShowPop(gems, gold);
        }


        if (currentReward == RewardsEnum.Gold)
        {
            int goldsLocal = EconomyController.GetInstance().GetGold() + CurrentRewardAmountToBeGiven;
            EconomyController.GetInstance().SetGold(goldsLocal);
   
        }
        else if (currentReward == RewardsEnum.Gem)
        {
            int goldsLocal = EconomyController.GetInstance().GetGold() + CurrentRewardAmountToBeGiven;
            EconomyController.GetInstance().SetGold(goldsLocal);

        }
        else if (currentReward == RewardsEnum.specialMoves)
        {
            string[] rewardsSpecialMoves = (string[])Enum.GetNames(typeof(SpecialMoves));
            for (int i = 1; i <= rewardsSpecialMoves.Length; i++)
            {
                if (PlayerPrefs.GetInt(rewardsSpecialMoves + "" + (i)) == 0)
                {
                    PlayerPrefs.SetInt("Special Move Bought" + (i), 1);
                    PlayerPrefs.SetInt(rewardsSpecialMoves + "" + (i), 1);
                    break;
                }
            }

        }
        else if (currentReward == RewardsEnum.powershots)
        {
            string[] rewardsPowerShots = (string[])Enum.GetNames(typeof(PowerShots));
            for (int i = 1; i <= rewardsPowerShots.Length; i++)
            {
                if (PlayerPrefs.GetInt(rewardsPowerShots + "" + (i)) == 0)
                {
                    PlayerPrefs.SetInt("Power Move" + (i), 1);
                    PlayerPrefs.SetInt(rewardsPowerShots + "" + (i), 1);
                    break;
                   
                }
            }
        }

        else if (currentReward == RewardsEnum.character)
        {
            string[] rewardsCharacter = (string[])Enum.GetNames(typeof(Character));
            for (int i = 1; i <= rewardsCharacter.Length; i++)
            {
                if (PlayerPrefs.GetInt(rewardsCharacter +""+ (i)) == 0)
                {
                    PlayerPrefs.SetInt("Characters Bought" + (i),1);
                    PlayerPrefs.SetInt(rewardsCharacter + "" + (i), 1);
                    break;
                }
            }

        }

        currentReward = RewardsEnum.None;

        //---------------------------------UPDATE AS PER REQUIREMENT---------------------------------//


        LoadingAdGO.SetActive(false);
        RewardGivenLogo.sprite = CurrentRewardRefill[PlayerPrefs.GetInt("RewardRefillCurrentNum")].rewardButtonDetails[PlayerPrefs.GetInt("RewardVideosWatched")].Logo;
        RewardGivenLogo.preserveAspect = true;
        RewardGivenText.text = CurrentRewardRefill[PlayerPrefs.GetInt("RewardRefillCurrentNum")].rewardButtonDetails[PlayerPrefs.GetInt("RewardVideosWatched")].HeadingText;

        PlayerPrefs.SetInt("RewardVideosWatched", PlayerPrefs.GetInt("RewardVideosWatched") + 1);
        if(PlayerPrefs.GetInt("RewardVideosWatched") % 5 == 0)
        {
            PlayerPrefs.SetInt("RewardVideosWatched", 0);
            RewardHasbeenGivenGO.SetActive(true);
            if (PlayerPrefs.GetString("RewardListComplete") == "False")
            {
                if (PlayerPrefs.GetInt("RewardRefillCurrentNum") + 1 != CurrentRewardRefill.Length)
                    PlayerPrefs.SetInt("RewardRefillCurrentNum", PlayerPrefs.GetInt("RewardRefillCurrentNum") + 1);
                else
                {
                    PlayerPrefs.SetString("RewardListComplete", "True");
                    PlayerPrefs.SetInt("RewardRefillCurrentNum", 0);
                    FindCurrentRefillToUse();
                }
            }
            else if (PlayerPrefs.GetString("RewardListComplete") == "True") {
                PlayerPrefs.SetInt("RewardRefillCurrentNum", (PlayerPrefs.GetInt("RewardRefillCurrentNum") + 1) % RandomRewardRefills.Length);
            }

        }
        UpdateRewardScreen();

    }




}
[Serializable]
public class RewardButtonData{
    public Button ParentButton;
    public Text Heading;
    public Text CoinsAmount;
    public Image ButtonBG;
    public Image RewardLogo;
    public Button RewardSubButton;
    public GameObject TickGameobject;
    public Text WatchvideoAdBtnText;
}

[Serializable]
public class RewardRefillData {
    public string Name;
    public RewardButtonDetails[] rewardButtonDetails;
}

[Serializable]
public class RewardButtonDetails {
    public string HeadingText;
    public int Amount;
    public Sprite Logo;
    public RewardsEnum RewardType;
}

public enum RewardsEnum {
    None = -1,
    Gold,
    Gem,
    powershots,
    specialMoves,
    character
}
public enum PowerShots
{
    powerShots1,
    powerShots2,
}
public enum SpecialMoves
{
    specialMoves1,
    specialMoves2,
    specialMoves3,
    specialMoves4,
    specialMoves5,
    specialMoves6,
}
public enum Character
{
    character1 = 1,
    character2 = 2,
    character3 = 3,
    character4,
    character5,
    character6,
    character7,
    character8,
    character9,
    character10,
    character11,
    character12,
    character13,
    character14,
    character15,
}

