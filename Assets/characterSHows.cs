using UnityEngine;
using System.Collections;

public class characterSHows : MonoBehaviour {

	public static characterSHows inst;
	public GameObject p1Parent,p2Parent,spriteImg;
	public GameObject[] p1Char, p2Char;
	public Transform shopTransform;
	public Transform normalTransform;
	public Transform challengeTransform;
	public Transform storyTransform;
	int _indexPlayerOne;
	// Use this for initialization
	void Start () {
		inst = this;

	}
	public void GotoShopAtPlayersPanel()
	{
		Invoke ("CallGotoShopAtPlayersPanel",0.5f);
	}
	void CallGotoShopAtPlayersPanel()
	{
		DefaultMainMenuScreen.GetInstance ().OpenShopPanel ();
		ShopManager.GetInstance ().OpenThisShopPanel (0);
	}
	public void GotoShopAtMovesPanel()
	{
		Invoke ("CallGotoShopAtMovesPanel",0.3f);
	}
	void CallGotoShopAtMovesPanel()
	{
		DefaultMainMenuScreen.GetInstance ().OpenShopPanel ();
		Invoke ("CallShopPanel",0.1f);

	}
	void CallShopPanel()
	{
		ShopManager.GetInstance ().OpenThisShopPanel (1);
	}

	/// <summary>
	/// Call For Special Moves
	/// </summary>

	public void GotoShopAtMovesPanelSpecialMove()
	{
		Invoke ("CallGotoShopAtMovesPanelSpecialMove",0.3f);
	}
	void CallGotoShopAtMovesPanelSpecialMove()
	{
		DefaultMainMenuScreen.GetInstance ().OpenShopPanel ();
		Invoke ("CallShopPanelSpecialMove",0.1f);

	}
	void CallShopPanelSpecialMove()
	{
		ShopManager.GetInstance ().OpenThisShopPanel (2);
	}



	public void hideModels(){
		p1Parent.SetActive (false);
		p2Parent.SetActive (false);
		foreach (GameObject objs in p1Char) {
			objs.SetActive (false);
		}
		foreach (GameObject objs in p2Char)
		{
			objs.SetActive(false);
		}

		HideBackgroundImage();
	}
	public void hidePlayer2Models()
	{
		 
		p2Parent.SetActive(false);
		 
		foreach (GameObject objs in p2Char)
		{
			objs.SetActive(false);
		}

		
	}
	public void hideModelsParent(){
		p1Parent.SetActive (false);
		p2Parent.SetActive(false);

	}
	public void ShowBackgroundImage()
	{
		spriteImg.SetActive (true);
	}
	public void HideBackgroundImage()
	{
		spriteImg.SetActive (false);
	}
	public void showImg() {
		ShowBackgroundImage();
		if (UFE.gameMode == GameMode.StoryMode)
		{
			p1Parent.SetActive(true);
			p2Parent.SetActive(false); 
		}
		if (UFE.gameMode != GameMode.StoryMode) {
			p1Parent.SetActive(true);
			p2Parent.SetActive (true);
//			p2Char [3].SetActive (true);
		}


	}
		
	public void ShowMove(string animName)
	{
		p1Char [_indexPlayerOne].GetComponent<Animator> ().SetTrigger (animName);
	}
	public void showChar(int index){
		if (UFE.gameMode == GameMode.StoryMode) {
			if(MainMenuScreen.isStoryModeCharacterSelection)
			p1Parent.SetActive (true);
          
        }
		foreach (GameObject objs in p1Char) {
			objs.SetActive (false);
		}
		p1Char [index].SetActive (true);
		p1Char [index].GetComponent<Animator> ().SetTrigger ("Selected");
		_indexPlayerOne = index;
	}
	public void showChar2(int index)
	{
		
		p2Parent.SetActive(true);
		
		foreach (GameObject objs in p2Char)
		{
			objs.SetActive(false);
		}
		p2Char[index].SetActive(true);
		p2Char[index].GetComponent<Animator>().SetTrigger("Selected");
		_indexPlayerOne = index;
	}
	public void showCharInShop(int index){

		p1Parent.SetActive (true);
		foreach (GameObject objs in p1Char) {
			objs.SetActive (false);
		}
		p1Char [index].SetActive (true);
		p1Char [index].GetComponent<Animator> ().SetTrigger ("Selected");
		_indexPlayerOne = index;

	}
	public void SetToShopTransform()
	{
		p1Parent.transform.localPosition = shopTransform.localPosition; 
		p1Parent.transform.localRotation = shopTransform.localRotation; 
	}
	public void SetToNormalTransform()
	{
		p1Parent.transform.localPosition = normalTransform.localPosition; 
		p1Parent.transform.localRotation = normalTransform.localRotation; 
	}
	public void SetToChallengeTransform()
	{
		p1Parent.transform.localPosition = challengeTransform.localPosition;
		p1Parent.transform.localRotation = challengeTransform.localRotation;
	}
	public void SetToStoryTransform()
	{
		p1Parent.transform.localPosition = storyTransform.localPosition;
		p1Parent.transform.localRotation = storyTransform.localRotation;
	}
	public void showChar2Ftn(int index){
	//	p2Parent.SetActive (true);
//		foreach (GameObject objs in p2Char) {
//			objs.SetActive (false);
//		}
//		p2Char [index].SetActive (true);
//		p2Char[index].GetComponent<Animator> ().SetTrigger ("Selected");
	}
}
