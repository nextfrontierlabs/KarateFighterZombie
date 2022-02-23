using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DefaultMainMenuScreen : MainMenuScreen{
	#region public instance fields
	public AudioClip onLoadSound;
	public AudioClip music;
	public AudioClip selectSound;
	public AudioClip cancelSound;
	public AudioClip moveCursorSound;
	public bool stopPreviousSoundEffectsOnLoad = false;
	public float delayBeforePlayingMusic = 0.1f;

	public Button buttonNetwork;
	public GameObject ExitDialogue;
	public GameObject PlayPanel;
	public GameObject shopPanel;

	public GameObject AchievementPanel;


	public bool isPlayButtonclicked = false;
	public bool isExitClicked = false;
	public bool comingFromAdToBuyStarterPack;

	public Text userLoginStatus;
	public GameObject starterPackBtn;
	public GameObject starterPackInAppBtn;
	public GameObject starterPackBtnBoughtByAd;
	public GameObject starterPackPanel;
	public Text starterPackPrice;
	public GameObject LoaderPanel;
	public GameObject FailedPanel;
	public Text FailMessage;
    private	static DefaultMainMenuScreen instance;
	public static DefaultMainMenuScreen GetInstance()
	{
		return instance;
	}
	void Awake()
	{
		instance = this;
	}
	void Start()
	{
#if UNITY_IOS || UNITY_ANDROID
        try
        {
            AdsManager.GetInstance().ShowBannerAd();
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
            #endif

		if (PlayerPrefs.GetInt ("StarterPackBoughtFromAd") == 0) {
			starterPackInAppBtn.SetActive (false);
			starterPackBtnBoughtByAd.SetActive (true);
		}
	   else  {
			starterPackInAppBtn.SetActive (true);
			starterPackBtnBoughtByAd.SetActive (false);
		}
	}
	#endregion

	#region public override methods
	public override void DoFixedUpdate(){
		base.DoFixedUpdate();
		this.DefaultNavigationSystem(this.selectSound, this.moveCursorSound, null, this.cancelSound);
	}

	public override void OnShow (){
		base.OnShow ();
		this.HighlightOption(this.FindFirstSelectable());

		if (this.music != null){
			UFE.DelayLocalAction(delegate(){UFE.PlayMusic(this.music);}, this.delayBeforePlayingMusic);
		}
		
		if (this.stopPreviousSoundEffectsOnLoad){
			UFE.StopSounds();
		}
		
		if (this.onLoadSound != null){
			UFE.DelayLocalAction(delegate(){UFE.PlaySound(this.onLoadSound);}, this.delayBeforePlayingMusic);
		}

		if (!UFE.isNetworkAddonInstalled){
			buttonNetwork.interactable = false;
		}
		InAppRandomOffersController.DoOnShowPanels();
	}
	#endregion


	public void OpenStarterPackPanel()
	{
		starterPackPanel.SetActive (true);
		starterPackPrice.text = IAP.Instance.starterPack;
	}

	public void CloseStarterPackPanel()
	{
		starterPackPanel.SetActive (false);
	}
	public void CloseFailPanel()
	{
		FailedPanel.SetActive (false);

	}

	public void BuyStarterPack(string productID)
	{
		LoaderPanel.SetActive (true);
		IAP.Instance.PurchaseItemButton (productID);
	}

	public void BuyStarterPackByAd()
	{
		comingFromAdToBuyStarterPack = true;
        
      
        if (AdsManager.GetInstance ().unityRewardedAd.isLoaded) {
			AdsManager.GetInstance ().unityRewardedAd.ShowRewardedVideo (10000, 100, false);
			//AdsManager.GetInstance ().facebookRewardedAd.LoadRewardedVideo ();
		}
		else if (AdsManager.GetInstance ().admobRewardedAd.isRewardedAdLoaded)
		{
			AdsManager.GetInstance ().admobRewardedAd.ShowRewardedVideo (10000, 100, false);
			//AdsManager.GetInstance ().facebookRewardedAd.LoadRewardedVideo ();
		}

	}
	public CanvasGroup [] groupSwitchOffForTraining;

	void OnEnable()
	{

		Camera.main.fieldOfView = 60;
		if (PlayerPrefs.GetInt ("FirstTimePlay") == 0) {
			for (int i = 0; i < groupSwitchOffForTraining.Length; i++) {
				groupSwitchOffForTraining[i].alpha = 0.28f;
				groupSwitchOffForTraining[i].interactable = false;
			}
		} else {
			for (int i = 0; i < groupSwitchOffForTraining.Length; i++) {
				groupSwitchOffForTraining[i].alpha = 1f;
				groupSwitchOffForTraining[i].interactable = true;
			}
		
		}
		if (PlayerPrefs.GetInt ("ShowStarterPack") == 1) {
			OpenStarterPackPanel ();
			PlayerPrefs.SetInt ("ShowStarterPack", 0);
		}
	}
	public void Update(){


		if (Input.GetKeyUp (KeyCode.Escape) && !isPlayButtonclicked && !isExitClicked) {
			ExitDialogueShow ();

		}
		else if (Input.GetKeyUp (KeyCode.Escape) && !isPlayButtonclicked && isExitClicked) {
			ExitDialogueCancel ();

		}
	}
//
//	public void FBBtn_Click ()
//	{
//		if (Application.platform == RuntimePlatform.Android)
//			Application.OpenURL ("http://www.facebook.com/sharer/sharer.php?u=https://play.google.com/store/apps/details?id=com.ultimate.fighters.kung.fu");
//
//
//	}
//
//	public void g_plusBtn_Click ()
//	{
//		if (Application.platform == RuntimePlatform.Android)
//			Application.OpenURL ("https://plus.google.com/share?url=https://play.google.com/store/apps/details?id=com.ultimate.fighters.kung.fu");
//
//
//	}
//
//	public void TwitterBtn_Click ()
//	{
//		//http://twitter.com/share?text=[your game namel]&url=[your-page-url]
//
//		if (Application.platform == RuntimePlatform.Android)
//			Application.OpenURL ("http://twitter.com/share?text=Ultimate Fighters &url=https://play.google.com/store/apps/details?id=com.ultimate.fighters.kung.fu");
//
//
//	}

	public void OuMoreGames()
	{
		#if UNITY_ANDROID
		Application.OpenURL ("https://play.google.com/store/apps/dev?id=8618359887532826706");
#else
		Application.OpenURL ("https://play.google.com/store/apps/dev?id=8618359887532826706");
#endif

	}
	public void RVGCricket()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.rockvillegames.real.cricket.game");
#else
		Application.OpenURL ("https://apps.apple.com/us/app/rvg-cricket-clash/id1550084385");
#endif

    }
    public void CK()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.rockvillegames.battlefield");
#else
		Application.OpenURL ("https://apps.apple.com/us/developer/rockville-technologies-pvt-ltd/id539407707");
#endif

    }
    public void ExitDialogueShow(){
		isExitClicked = true;
		ExitDialogue.SetActive (true);

		AdsManager.GetInstance().ShowInterstitialStatic();

	}
	public void ExitDialogueCancel(){
		isExitClicked = false;
		ExitDialogue.SetActive (false);
		if (Application.platform == RuntimePlatform.Android  || Application.platform == RuntimePlatform.IPhonePlayer) {
			//AdsManager.Instance.hideLargeNativeAd ();
		}
	}
	public void OpenShopPanel()
	{
		PlayPanel.SetActive (false);
		shopPanel.SetActive (true);
		characterSHows.inst.SetToShopTransform ();
		characterSHows.inst.ShowBackgroundImage ();
	}
	public void CloseShopPanel()
	{
		characterSHows.inst.SetToNormalTransform ();
		if (DefaultCharacterSelectionScreen.cameFromCharacterSelectionPanelToShop) {
			GoToStoryModeScreen (true);
			DefaultCharacterSelectionScreen.cameFromCharacterSelectionPanelToShop = false;
		}
		PlayPanel.SetActive (true);
		shopPanel.SetActive (false);


	}
	public void PlayPanelTurnOn(){
		isPlayButtonclicked = true;
		PlayPanel.SetActive (true);
	}

	public void PlayPanelTurnOff(){
		isPlayButtonclicked = false;
		PlayPanel.SetActive (false);
	}

	public void OpenLeaderBoard()
	{
		if (GPGManager.GetInstance ().loggedIn) {
            // show leader board
			GPGManager.GetInstance().ShowLeaderboard();
		} else {
			// login again
			GPGManager.GetInstance().Login();
		}
	}
	public void OpenAchievements()
	{
		if (GPGManager.GetInstance ().loggedIn) {
			// show leader board
			GPGManager.GetInstance().ShowAchievements();
		} else {
			// login again
			GPGManager.GetInstance().Login();
		}
	}
	public void CloseLoginPanel()
	{

		    PlayPanel.SetActive (true);

	}
	public void CloseLeaderBoard()
	{
		
		PlayPanel.SetActive (true);

	}
 

}
