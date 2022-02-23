using UnityEngine;
using System;

public class StoryModeScreen : UFEScreen {
	#region public instance properties
	public Action nextScreenAction{get; set;}
	#endregion

	public virtual void GoToMainMenu(){
		UFE.StartMainMenuScreen();
	}
	#region public instance methods
	public virtual void GoToNextScreen(){
		if (this.nextScreenAction != null){
			this.nextScreenAction();
		}
		
	}
	#endregion
}
