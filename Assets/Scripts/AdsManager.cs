using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;
using UnityEngine.Serialization;
using UnityEngine.Networking;

[System.Serializable]
struct Placement_Status
{
	public bool isEnabled
	{
		get
		{
			return _isActive;
		}
		set
		{
			_isActive = value;
		}
	}
	public AdsManager.AdPlacements placement;
	public List<AdsManager.AllAdNetworks> Priority;
	private bool _isActive;
}



public class AdsManager : MonoBehaviour {

	public const string JSonAdDataPath = "PlacementData";

	[System.Serializable]
	public enum AllAdNetworks
	{
		Facebook,
		Unity,
		Google,
		Adcolony

	}
	[System.Serializable]
	public enum AdPlacements
	{
		GameWin,
		GameLose,
		GamePause,
		BannerView,
		RewardedAd
	}


	#region banners
	public GoogleMobileAdsDemoScript admobBanner;

	#endregion

	#region Banner_Placements
	[SerializeField]
	Placement_Status BannerView = new Placement_Status { isEnabled = true, placement = AdPlacements.BannerView, Priority = new List<AllAdNetworks> {  AllAdNetworks.Facebook, AllAdNetworks.Google} };
	private void ShowBannerAd(Placement_Status _Status)
	{
		if (!_Status.isEnabled)
		{
			Debug.LogError("Status is set to False");
			return;
		}
		for (int i = 0; i < _Status.Priority.Count; i++)
		{

			if (_Status.Priority[i] == AllAdNetworks.Facebook)
			{

                  
					break;
				

			}
			else if (_Status.Priority[i] == AllAdNetworks.Google)
			{
		
				if (admobBanner.bannerView != null)
				{
					admobBanner.ShowBanner ();
					//facebookBanner.LoadBanner ();
					break;
				}

			}
			else 
			{
					admobBanner.ShowBanner ();

				    //facebookBanner.LoadBanner ();
			    	admobBanner.RequestBanner ();
					break;
			}


		}
	}
	public void ShowBannerAd()
	{
		ShowBannerAd(BannerView);
	}
	#endregion

	#region Interstitial
	
	public UnityAds unityInterstitialAd;
	public GoogleMobileAdsDemoScript admobInterstitialAd;




	Placement_Status Game_Win = new Placement_Status { isEnabled = true, placement = AdPlacements.GameWin, Priority = new List<AllAdNetworks> {  AllAdNetworks.Adcolony,AllAdNetworks.Unity, AllAdNetworks.Google,  AllAdNetworks.Facebook } };
	Placement_Status Game_Lose = new Placement_Status { isEnabled = true, placement = AdPlacements.GameLose, Priority = new List < AllAdNetworks > { AllAdNetworks.Adcolony,AllAdNetworks.Unity, AllAdNetworks.Google, AllAdNetworks.Facebook } };
	Placement_Status Game_Pause = new Placement_Status { isEnabled = true, placement = AdPlacements.GamePause, Priority = new List<AllAdNetworks> {  AllAdNetworks.Adcolony,AllAdNetworks.Unity, AllAdNetworks.Google, AllAdNetworks.Facebook } };


	#endregion

	#region Rewarded Ad
	
	public UnityAds   unityRewardedAd;
	public GoogleMobileAdsDemoScript  admobRewardedAd;
	
	#endregion


	private static AdsManager adsManager;
	public static AdsManager GetInstance()
	{
      return adsManager;
	}

	public void SetupPrioritySettings(string data)
	{
		Hashtable allPlacementStatus = (Hashtable)easy.JSON.JsonDecode(data);
		if (allPlacementStatus!= null)
		{
			Hashtable IP = allPlacementStatus["Interstitial_Placement"] as Hashtable;
			string rootKey = string.Empty;
			for (int i = 0; i < 3; i++)
			{
				if (i == 0)
				{
					rootKey = "Win";
				}
				else if (i == 1)
				{
					rootKey = "Lose";
				}
				else
				{
					rootKey = "Pause";
				}
				Hashtable GW = IP[rootKey] as Hashtable;

				bool isenabled = (bool)GW["Enabled"];

				ArrayList temp = (ArrayList)GW["PriorityString"];

				List<AllAdNetworks> test = new List<AllAdNetworks>();
				for (int j = 0; j < temp.Count; j++)
				{
					test.Add((AllAdNetworks)Enum.Parse(typeof(AllAdNetworks), temp[j].ToString()));
					Debug.LogError(temp[j]);
				}
				Placement_Status tempstatus = new Placement_Status() { isEnabled = isenabled, Priority = test };
				Debug.LogError(GW["PriorityString"]);
				if (i == 0)
				{
					tempstatus.placement = AdPlacements.GameWin;
					Game_Win = tempstatus;
				}
				else if (i == 1)
				{
					tempstatus.placement = AdPlacements.GameLose;
					Game_Lose = tempstatus;
				}
				else
				{
					tempstatus.placement = AdPlacements.GamePause;
					Game_Pause = tempstatus;
				}

			}

			Hashtable BP = allPlacementStatus["Banner_Placement"] as Hashtable;

			bool isenabledBP = (bool)BP["Enabled"];

			ArrayList tempBP = (ArrayList)BP["PriorityString"];

			List<AllAdNetworks> testBP = new List<AllAdNetworks>();
			for (int j = 0; j < tempBP.Count; j++)
			{
				testBP.Add((AllAdNetworks)Enum.Parse(typeof(AllAdNetworks), tempBP[j].ToString()));
				Debug.LogError("banner "+tempBP[j]);
			}
			BannerView.isEnabled = isenabledBP;
			BannerView.Priority = testBP;
			BannerView.placement = AdPlacements.BannerView;

			//			Hashtable RP = allPlacementStatus["RewardedVideo_Placement"] as Hashtable;
			//
			//			bool isenabledRP = (bool)RP["Enabled"];
			//
			//			ArrayList tempRP = (ArrayList)RP["PriorityString"];
			//
			//			List<AllAdNetworks> testRP = new List<AllAdNetworks>();
			//			for (int j = 0; j < tempRP.Count; j++)
			//			{
			//				testRP.Add((AllAdNetworks)Enum.Parse(typeof(AllAdNetworks), tempRP[j].ToString()));
			//				Debug.LogError(tempRP[j]);
			//			}
			//			BannerView.isEnabled = isenabledRP;
			//			BannerView.Priority = testRP;
			//			BannerView.placement = AdPlacements.RewardedAd;



		}
		else
		{

		}

	}
	IEnumerator GetRequest(string uri)
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();

			string[] pages = uri.Split('/');
			int page = pages.Length - 1;

			if (webRequest.isNetworkError)
			{
				Debug.LogError(pages[page] + ": Error: " + webRequest.error);
			}
			else
			{
				Debug.LogError(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
				SetupPrioritySettings(webRequest.downloadHandler.text);
			}
		}
	}
	void Awake()
	{
        if (adsManager == null)
            adsManager = this;

        DontDestroyOnLoad(this);
        StartCoroutine(GetRequest("https://www.dropbox.com/s/4wtguz83fr87gwi/PlacementDataKOKF.txt?dl=1"));
		TextAsset adData = Resources.Load<TextAsset>(JSonAdDataPath);
		var allPlacementStatus  = easy.JSON.JsonDecode(adData.text);

	}
	void Start()
	{
	
		admobRewardedAd.RequestRewarded ();
		admobBanner.RequestBanner ();
        admobBanner.RequestBannerDouble();
        admobInterstitialAd.RequestInterstitial ();
		admobInterstitialAd.RequestInterstitial_Static();

	}




	private void ShowAd(Placement_Status _Status)
	{
		if (PlayerPrefs.GetInt("RemoveAds") == 1)
			return;
		if (!_Status.isEnabled)
		{
			Debug.LogError("Status is set to False");
			return;
		}
		for (int i = 0; i < _Status.Priority.Count; i++)
		{

			bool adshown = false;
			switch (_Status.Priority[i])
			{

			case AllAdNetworks.Facebook:
				Debug.Log (_Status.Priority [i].ToString () + " Prioriry");
                   
                    
                    break;
			
			case AllAdNetworks.Google:
				Debug.Log (_Status.Priority [i].ToString () + " Prioriry");
				if (admobInterstitialAd.isInterstitialAdLoaded)
				{
					admobInterstitialAd.ShowInterstitial ();
					adshown = true;
				}

				break;
			case AllAdNetworks.Unity:
				Debug.Log (_Status.Priority [i].ToString () + " Prioriry");
				if (unityInterstitialAd.isLoaded)
				{
					adshown = true;
					unityInterstitialAd.ShowInterstitial ();

				}
				break;

			default:
				break;

			}
			if (adshown)
			{
				break;
			}


		}
	}
	public void ShowGameWinAd()
	{
		ShowAd(Game_Win);
	}
	public void ShowGameLoseAd()
	{
		ShowAd(Game_Lose);
	}
	public void ShowGamePauseAd()
	{
		Debug.Log ("ShowGamePauseAd");
		ShowAd(Game_Pause);
	}


	public	void ShowInterstitialAdsByPriority()
	{

		if (PlayerPrefs.GetInt("RemoveAds") == 1)
			return;
		if (unityInterstitialAd.isLoaded) {
			unityInterstitialAd.ShowInterstitial ();

		} else if (admobInterstitialAd.isInterstitialAdLoaded) {
			admobInterstitialAd.ShowInterstitial ();
			unityInterstitialAd.IsLoaded ();
		}   else {
			
			unityInterstitialAd.IsLoaded ();
			admobInterstitialAd.RequestInterstitial ();
		}
	}
	public void ShowInterstitialStatic()
    {
		if (PlayerPrefs.GetInt("RemoveAds") == 1)
			return;
		admobInterstitialAd.ShowInterstitial_Static();
	}
}
