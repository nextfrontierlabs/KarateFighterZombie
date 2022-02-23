using UnityEngine;
using System.Collections;

public class charcterint : MonoBehaviour {

	bool p1Selected,p2Selected;
	public static charcterint inst;
	void OnEnable(){
		inst = this;
		p1Selected = false;
		p2Selected = false;
		characterSHows.inst.hideModels ();
		characterSHows.inst.showImg ();
		Camera.main.fieldOfView = 60f;
	}
	void OnDisable(){
		characterSHows.inst.hideModels ();
	}
	// Use this for initialization
	void Start () {
	
	}


	public void showChar1Ftn(int index)
	{
		if (UFE.gameMode == GameMode.StoryMode)
		{
			PlayerPrefs.SetInt("selectedTekkenIndex", index);
			PlayerPrefs.Save();

			characterSHows.inst.showChar(index);
		}
		else
		{
			if (UFE.config.player1Character != null && UFE.config.player2Character == null && DefaultCharacterSelectionScreen.isMultiplayerSelected)
			{
				return;
			}
			if (!p1Selected)
			{
				//characterSHows.inst.showChar (index);
				p1Selected = true;
			}
			else
			{
				if (!p2Selected)
					//	characterSHows.inst.showChar2Ftn(index);
					p2Selected = true;
			}

		}
	}
	public void showChar1FtnAuto(int index)
	{

		if (!p1Selected)
		{
			//characterSHows.inst.showChar (index);
			p1Selected = true;
		}
		else
		{
			if (!p2Selected)
				//	characterSHows.inst.showChar2Ftn(index);
				p2Selected = true;
		}


	}
	public void ShowChar1FtnOnMenu(int index)
	{

		PlayerPrefs.SetInt("selectedTekkenIndex", index);
		PlayerPrefs.Save();




	}
}
