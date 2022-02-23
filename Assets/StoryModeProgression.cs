using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StoryModeProgression : MonoBehaviour {
	public static float level;


	public Image progressionStars;
	public Text progressionText;
	public Image[] whiteLayer;
	public Image[] stars;
	public Color fadeIt;

	public GameObject fightBtn;
	public GameObject finishBtn;
	public GameObject confirmationPanel;
	public DefaultCharacterSelectionScreen defaultCharacterSelectionScreen;
	 void OnEnable () {

		//allPortraitImages [PlayerPrefs.GetInt ("selectedTekkenIndex", 2)];
		characterSHows.inst.showChar(PlayerPrefs.GetInt("selectedTekkenIndex", 2));
		progressionText.text = PlayerPrefs.GetInt ("levels").ToString()+"/14";
		progressionStars.fillAmount = ((float)PlayerPrefs.GetInt ("levels") / 14f);
		for (int i = 0; i < PlayerPrefs.GetInt ("levels"); i++) {
		      
			stars[i].fillAmount =  PlayerPrefs.GetFloat ("StoryModeStarsOfLevel"+ (i+1).ToString());
		}

		for (int i = 0; i < whiteLayer.Length; i++) {
			
			if (i <= PlayerPrefs.GetInt ("levels")) {
				
				whiteLayer [i].gameObject.SetActive (false);
				if (i < PlayerPrefs.GetInt ("levels")) {
					whiteLayer [i].transform.parent.gameObject.GetComponent<Image> ().color = fadeIt;
				}
			
			}
		
		

		}

		if (PlayerPrefs.GetInt ("levels") == 15) {
			fightBtn.SetActive (false);
			finishBtn.SetActive (true);
		}
	}
	public void Reset()
	{
		for (int i = 0; i < stars.Length; i++) {

			stars [i].fillAmount = 0;
		}

		for (int i = 0; i < whiteLayer.Length; i++) {
			
				whiteLayer [i].gameObject.SetActive (true);
            	whiteLayer [i].transform.parent.gameObject.GetComponent<Image> ().color = new Color(255f,255f,255f,255f);
		
		}
	}
	public void FinishIt()
	{
		confirmationPanel.SetActive (true);
	}
	public void ResetStoryMode()
	{
		defaultCharacterSelectionScreen.yesFtn ();
		OnEnable ();
		confirmationPanel.SetActive (false);
		fightBtn.SetActive (true);
		finishBtn.SetActive (false);

	}
}
