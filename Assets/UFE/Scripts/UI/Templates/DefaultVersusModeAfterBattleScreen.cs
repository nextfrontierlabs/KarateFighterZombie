using UnityEngine;
using System.Collections;

public class DefaultVersusModeAfterBattleScreen : VersusModeAfterBattleScreen{
	#region public instance properties
	public AudioClip onLoadSound;
	public AudioClip music;
	public AudioClip selectSound;
	public AudioClip cancelSound;
	public AudioClip moveCursorSound;
	public bool stopPreviousSoundEffectsOnLoad = false;
	public float delayBeforePlayingMusic = 0.1f;
	public GameObject wonObject;
	public GameObject looseObject;
	#endregion
	#region public override methods
	public override void DoFixedUpdate(){
		base.DoFixedUpdate();
		this.DefaultNavigationSystem(this.selectSound, this.moveCursorSound, this.GoToMainMenu, this.cancelSound);
	}

	public override void OnShow (){
		base.OnShow ();
		
		if (this.music != null){
			UFE.DelayLocalAction(delegate(){UFE.PlayMusic(this.music);}, this.delayBeforePlayingMusic);
		}
		
		if (this.stopPreviousSoundEffectsOnLoad){
			UFE.StopSounds();
		}
		
		if (this.onLoadSound != null){
			UFE.DelayLocalAction(delegate(){UFE.PlaySound(this.onLoadSound);}, this.delayBeforePlayingMusic);
		}
	}


	#endregion
	void OnEnable()
	{
		Debug.Log ("On Enable");
		base.touchController =  GameObject.FindObjectOfType<TouchController> ().gameObject as GameObject;
		base.touchController.GetComponent<TouchController> ().enabled = false;
		if (UFE.player1WonLastBattleVersusMode) {
			wonObject.SetActive (true);
			looseObject.SetActive (false);
			AdsManager.GetInstance().ShowGameWinAd();
		} else {
			wonObject.SetActive (false);
			looseObject.SetActive (true);
			AdsManager.GetInstance().ShowGameLoseAd();
		}
		#if UNITY_IOS || UNITY_ANDROID
			try{
			AdsManager.GetInstance().ShowBannerAd();
				
			}
			catch(System.Exception e) {
				Debug.Log (" " + e.Message);
			}
		#endif
	}
	void OnDisbale()
	{
		
		base.touchController.GetComponent<TouchController> ().enabled = true;
}
}
