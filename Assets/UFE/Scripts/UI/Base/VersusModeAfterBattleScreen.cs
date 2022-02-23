using UnityEngine;
using System.Collections;

public class VersusModeAfterBattleScreen : UFEScreen {
	public GameObject touchController;
	public virtual void GoToCharacterSelectionScreen(){
		
		UFE.StartCharacterSelectionScreen();

	}

	public virtual void GoToMainMenu(){
		// Fixing an error while reloading versus battle
		GameObject obj =  GameObject.FindObjectOfType<TouchController> ().gameObject as GameObject;
		obj.GetComponent<TouchController> ().enabled = true;
	
		UFE.StartMainMenuScreen();
	}

	public virtual void GoToStageSelectionScreen(){
		// Fixing an error while reloading versus battle


		UFE.StartStageSelectionScreen();
	}

	public virtual void RepeatBattle(){
		// Fixing an error while reloading versus battle
		GameObject obj =  GameObject.FindObjectOfType<TouchController> ().gameObject as GameObject;
		obj.GetComponent<TouchController> ().enabled = true;

		if (Application.platform == RuntimePlatform.Android  || Application.platform == RuntimePlatform.IPhonePlayer) {
            //if (AdsManager.GetInstance ().facebookBanner.isLoaded)

            AdsManager.GetInstance ().admobBanner.HideBanner ();
            AdsManager.GetInstance().admobBanner.HideBannerDouble();
        }
		
		UFE.StartLoadingBattleScreen();
	}
}
