using UnityEngine;
using System.Collections;

public class PauseScreen : UFEScreen {
	public virtual void GoToMainMenu(){
		UFE.StartMainMenuScreen(0f);
        AdsManager.GetInstance().admobBanner.HideBannerDouble();
    }

	public virtual void ResumeGame(){
		UFE.PauseGame(false);

		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            
          
            AdsManager.GetInstance ().admobBanner.HideBanner ();
            AdsManager.GetInstance().admobBanner.HideBannerDouble();
        }
	}
}
