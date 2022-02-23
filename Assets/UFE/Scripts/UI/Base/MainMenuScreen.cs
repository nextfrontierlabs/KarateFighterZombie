using UnityEngine;
using System.Collections;

public class MainMenuScreen : UFEScreen {
	public static bool isStoryMode;
	public static bool isStoryModeCharacterSelection;
	public virtual void Quit(){
		UFE.Quit();
	}

	public virtual void GoToStoryModeScreen(bool characterSelection){
		MainMenuScreen.isStoryMode = true;
		MainMenuScreen.isStoryModeCharacterSelection = characterSelection;
		if (characterSelection)
			characterSHows.inst.SetToNormalTransform();
		else
		{
			 
			characterSHows.inst.SetToStoryTransform();
		}
		UFE.StartStoryMode();
	}

	public virtual void GoToVersusModeScreen(bool isMultiplayer)
	{
		//UFE.StartVersusModeScreen();
		MainMenuScreen.isStoryMode = false;
		DefaultCharacterSelectionScreen.isMultiplayerSelected = isMultiplayer;
		characterSHows.inst.SetToChallengeTransform();
		UFE.StartPlayerVersusCpu();
	}

	public virtual void GoToTrainingModeScreen(){
		MainMenuScreen.isStoryMode = false;
		DefaultCharacterSelectionScreen.isMultiplayerSelected = false;
		characterSHows.inst.SetToChallengeTransform ();
		UFE.StartTrainingMode();
	}

	public virtual void GoToNetworkPlayScreen(){
		UFE.StartNetworkGameScreen();
	}

	public virtual void GoToOptionsScreen(){
		UFE.StartOptionsScreen();
		AdsManager.GetInstance().ShowInterstitialStatic();
	}

	public virtual void GoToCreditsScreen(){
		UFE.StartCreditsScreen();
	}
}
