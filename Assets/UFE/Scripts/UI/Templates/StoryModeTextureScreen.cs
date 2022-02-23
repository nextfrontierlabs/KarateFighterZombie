using UnityEngine;
using System.Collections;

public class StoryModeTextureScreen : StoryModeScreen {
	#region public instance properties
	public AudioClip onLoadSound;
	public AudioClip music;
	public bool skippable = true;
	public bool stopPreviousSoundEffectsOnLoad = false;
	public float delayBeforePlayingMusic = 0.1f;
	public float delayBeforeGoingToNextScreen = 3f;
	public float minDelayBeforeSkipping = 0.1f;
	public bool afterBattle = false;
	#endregion

	#region public override methods
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
		if(!afterBattle)
		this.StartCoroutine(this.ShowScreen());
	}
		
	public virtual IEnumerator ShowScreen(){
		float startTime = Time.realtimeSinceStartup;
		float time = 0f;
		
		while(
			time < this.delayBeforeGoingToNextScreen && 
			!(skippable && Input.anyKeyDown && time > this.minDelayBeforeSkipping)
		){
			yield return null;
			time = Time.realtimeSinceStartup - startTime;
		}
		
		this.GoToNextScreen();
	}
	void OnEnable()
	{
		Debug.Log ("On Enable");
		if (afterBattle) {
			#if UNITY_IOS || UNITY_ANDROID
				try{
				AdsManager.GetInstance().admobBanner.ShowBannerDouble();

				AdsManager.GetInstance().ShowGameWinAd();
				}
				catch(System.Exception e) {
					Debug.Log (" " + e.Message);
				}
			#endif
		}
	}
	#endregion
}
