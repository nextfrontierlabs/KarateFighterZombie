using UnityEngine;
using System.Collections;

public class StoryModeContinueScreen : UFEScreen {
	public virtual void RepeatBattle(){
		UFE.StartStoryModeBattle();
		if (Application.platform == RuntimePlatform.Android  || Application.platform == RuntimePlatform.IPhonePlayer) {
            

            AdsManager.GetInstance ().admobBanner.HideBanner ();
            AdsManager.GetInstance().admobBanner.HideBannerDouble();
        }
	}

	public virtual void GoToGameOverScreen(){
        AdsManager.GetInstance().admobBanner.HideBanner();
        AdsManager.GetInstance().admobBanner.HideBannerDouble();
        UFE.StartStoryModeGameOverScreen();
	}
}
