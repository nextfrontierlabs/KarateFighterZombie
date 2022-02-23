using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleMobileAdsDemoScript : MonoBehaviour {

	public string appID = "ca-app-pub-2814647089623348~3530244765";
	public string bannerID = "ca-app-pub-2814647089623348/6257596405";
    public string bannerIDLeft = "ca-app-pub-2814647089623348/3272413834";
    public string bannerIDRight = "ca-app-pub-2814647089623348/2509923081";
    public string interstitialID = "ca-app-pub-3940256099942544/1033173712";
	public string interstitialID_Static = "ca-app-pub-2814647089623348/9367868375";
	public string rewardedID = "ca-app-pub-2814647089623348/6524562430";
	public BannerView bannerView;
    public BannerView bannerViewDoubleLeft;
    public BannerView bannerViewDoubleRight;
    private InterstitialAd interstitial;
	private InterstitialAd interstitial_Static;
	public RewardedAd rewardedAd;
	public bool isRewardedAdLoaded;
	public bool isInterstitialAdLoaded;
	public bool isInterstitialAdLoaded_Static;
	// Use this for initialization
	void Awake () {
#if UNITY_IOS
		appID = "ca-app-pub-2814647089623348~1487163640";
		bannerID = "ca-app-pub-2814647089623348/2026219692";
		interstitialID = "ca-app-pub-2814647089623348/9713138025";
		interstitialID_Static = "ca-app-pub-2814647089623348/9713138025";
		rewardedID = "ca-app-pub-2814647089623348/5773893014";

#elif UNITY_ANDROID
		appID = "ca-app-pub-1579727795693040~3659700951";
		bannerID = "ca-app-pub-1579727795693040/1141348827";
		interstitialID = "ca-app-pub-1579727795693040/6607290310";
		interstitialID_Static = "ca-app-pub-1579727795693040/3795432326";
		rewardedID = "ca-app-pub-1579727795693040/8643917409";
		#endif



		MobileAds.Initialize (initStatus => {});
		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize(appID);

	}

	public  void RequestInterstitial()
	{
		

		// Initialize an InterstitialAd.
		this.interstitial = new InterstitialAd(interstitialID);

		// Called when an ad request has successfully loaded.
		this.interstitial.OnAdLoaded += HandleOnAdLoaded;
		// Called when an ad request failed to load.
		this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is shown.
		this.interstitial.OnAdOpening += HandleOnAdOpened;
		// Called when the ad is closed.
		this.interstitial.OnAdClosed += HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;



		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		this.interstitial.LoadAd(request);

		}
	public void RequestInterstitial_Static()
	{


		// Initialize an InterstitialAd.
		this.interstitial_Static = new InterstitialAd(interstitialID_Static);

		// Called when an ad request has successfully loaded.
		this.interstitial_Static.OnAdLoaded += HandleOnAdLoaded_Static;
		// Called when an ad request failed to load.
		this.interstitial_Static.OnAdFailedToLoad += HandleOnAdFailedToLoad_Static;
		// Called when an ad is shown.
		this.interstitial_Static.OnAdOpening += HandleOnAdOpened_Static;
		// Called when the ad is closed.
		this.interstitial_Static.OnAdClosed += HandleOnAdClosed_Static;
		// Called when the ad click caused the user to leave the application.
		this.interstitial_Static.OnAdLeavingApplication += HandleOnAdLeavingApplication_Static;



		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		this.interstitial_Static.LoadAd(request);

	}
	public void RequestBanner()
	{

		// Create a 320x50 banner at the top of the screen.
		this.bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Top);

		// Called when an ad request has successfully loaded.
		this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
		// Called when an ad request failed to load.
		this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
		// Called when an ad is clicked.
		this.bannerView.OnAdOpening += this.HandleOnAdOpened;
		// Called when the user returned from the app after an ad click.
		this.bannerView.OnAdClosed += this.HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;


		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();

		// Load the banner with the request.
		this.bannerView.LoadAd(request);
		HideBanner ();

	}
    public void RequestBannerDouble()
    {

        // Create a 320x50 banner at the top of the screen.
        this.bannerViewDoubleLeft = new BannerView(bannerIDLeft, AdSize.Banner, AdPosition.TopLeft);

        // Called when an ad request has successfully loaded.
        this.bannerViewDoubleLeft.OnAdLoaded += this.HandleOnAdLoadedLeft;
        // Called when an ad request failed to load.
        this.bannerViewDoubleLeft.OnAdFailedToLoad += this.HandleOnAdFailedToLoadLeft;
        // Called when an ad is clicked.
        this.bannerViewDoubleLeft.OnAdOpening += this.HandleOnAdOpenedLeft;
        // Called when the user returned from the app after an ad click.
        this.bannerViewDoubleLeft.OnAdClosed += this.HandleOnAdClosedLeft;
        // Called when the ad click caused the user to leave the application.
        this.bannerViewDoubleLeft.OnAdLeavingApplication += this.HandleOnAdLeavingApplicationLeft;


        // Create an empty ad request.
        AdRequest requestLeft = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerViewDoubleLeft.LoadAd(requestLeft);


        // Create a 320x50 banner at the top of the screen.
        this.bannerViewDoubleRight = new BannerView(bannerIDRight, AdSize.Banner, AdPosition.TopRight);

        // Called when an ad request has successfully loaded.
        this.bannerViewDoubleRight.OnAdLoaded += this.HandleOnAdLoadedRight;
        // Called when an ad request failed to load.
        this.bannerViewDoubleRight.OnAdFailedToLoad += this.HandleOnAdFailedToLoadRight;
        // Called when an ad is clicked.
        this.bannerViewDoubleRight.OnAdOpening += this.HandleOnAdOpenedRight;
        // Called when the user returned from the app after an ad click.
        this.bannerViewDoubleRight.OnAdClosed += this.HandleOnAdClosedRight;
        // Called when the ad click caused the user to leave the application.
        this.bannerViewDoubleRight.OnAdLeavingApplication += this.HandleOnAdLeavingApplicationRight;


        // Create an empty ad request.
        AdRequest requestRight = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerViewDoubleRight.LoadAd(requestRight);


        HideBannerDouble();

    }


    #region admob rewared ad events

    public int gems;
	public int gold;
	public bool twiceTheReward;

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

	public  void RequestRewarded()
	{
	
		this.rewardedAd = new RewardedAd(rewardedID);
		// Called when an ad request has successfully loaded.
		this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
		// Called when an ad request failed to load.
		this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
		// Called when an ad is shown.
		this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
		// Called when an ad request failed to show.
		this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
		// Called when the user should be rewarded for interacting with the ad.
		this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
		// Called when the ad is closed.
		this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the rewarded ad with the request.
		this.rewardedAd.LoadAd(request);


	}




	public void HandleRewardedAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdLoaded event received");
		isRewardedAdLoaded = true;
	}

	public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
	{
		isRewardedAdLoaded = false;
		MonoBehaviour.print(
			"HandleRewardedAdFailedToLoad event received with message: "
			+ args.Message);
	}

	public void HandleRewardedAdOpening(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdOpening event received");
	}

	public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
	{
		MonoBehaviour.print(
			"HandleRewardedAdFailedToShow event received with message: "
			+ args.Message);
	}

	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdClosed event received");
	}

	public void HandleUserEarnedReward(object sender, Reward args)
	{
		string type = args.Type;
		double amount = args.Amount;
		SetReward();
		RequestRewarded();
		MonoBehaviour.print(
			"HandleRewardedAdRewarded event received for "
			+ amount.ToString() + " " + type);
	}

	public void ShowRewardedVideo(int gold,int gems, bool twiceIt)
	{
		this.gold = gold;
		this.gems = gems;
		this.twiceTheReward = twiceIt;


		if (this.rewardedAd.IsLoaded()) {
			this.rewardedAd.Show();
		}
	}

    #endregion


    #region Banners Call Back

    public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLoaded event received");
		isInterstitialAdLoaded = true;
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
			+ args.Message);
		isInterstitialAdLoaded = false;
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdOpened event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdClosed event received");
		RequestInterstitial ();
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeavingApplication event received");
	}


	public void HandleOnAdLoaded_Static(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLoaded_Static event received");
		isInterstitialAdLoaded_Static = true;
	}

	public void HandleOnAdFailedToLoad_Static(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleFailedToReceiveAd_Static event received with message: "
			+ args.Message);
		isInterstitialAdLoaded_Static = false;
	}

	public void HandleOnAdOpened_Static(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdOpened_Static event received");
	}

	public void HandleOnAdClosed_Static(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdClosed_Static event received");
		RequestInterstitial_Static();
	}

	public void HandleOnAdLeavingApplication_Static(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeavingApplication_Static event received");
	}

	public void HandleOnAdLoadedLeft(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        isInterstitialAdLoaded = true;
    }

    public void HandleOnAdFailedToLoadLeft(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
            + args.Message);
        isInterstitialAdLoaded = false;
    }

    public void HandleOnAdOpenedLeft(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosedLeft(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");

    }

    public void HandleOnAdLeavingApplicationLeft(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    public void HandleOnAdLoadedRight(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        isInterstitialAdLoaded = true;
    }

    public void HandleOnAdFailedToLoadRight(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
            + args.Message);
        isInterstitialAdLoaded = false;
    }

    public void HandleOnAdOpenedRight(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosedRight(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
      
    }

    public void HandleOnAdLeavingApplicationRight(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    #endregion 

    public void ShowBanner()
	{
		this.bannerView.Show ();
	}
	public void HideBanner()
	{
		this.bannerView.Hide ();
	}
    public void ShowBannerDouble()
    {
        this.bannerViewDoubleLeft.Show();
        this.bannerViewDoubleRight.Show();
       
    }
    public void HideBannerDouble()
    {
        this.bannerViewDoubleLeft.Hide();
        this.bannerViewDoubleRight.Hide();
    }

    public void ShowInterstitial()
	{
		this.interstitial.Show ();
	}
	public void ShowInterstitial_Static()
	{
		this.interstitial_Static.Show();
	}

}
