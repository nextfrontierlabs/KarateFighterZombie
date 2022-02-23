using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CurrencyPopUpManager : MonoBehaviour {

	private static CurrencyPopUpManager currencyPopUpManager;
	public static CurrencyPopUpManager GetInstance()
	{
		return currencyPopUpManager;
	}
	void Awake()
	{
		currencyPopUpManager = this;
	}

	public GameObject popUpCanvas;
	public Text gemCount;
	public Text goldCount;

	public Image gemImage;
	public Image goldImage;
	public Text  txt;

	public void ShowPop(int gem,int gold)
	{
		popUpCanvas.SetActive (true);
		txt.gameObject.SetActive(false);
		gemCount.text = gem.ToString () + " Gems";
		goldCount.text = gold.ToString()+ " Gold";
		if (gem == 0) {
			gemCount.gameObject.SetActive (false);
			gemImage.gameObject.SetActive (false);
		} else {
			gemCount.gameObject.SetActive (true);
			gemImage.gameObject.SetActive (true);
		}
		if (gold == 0) {
			goldCount.gameObject.SetActive (false);
			goldImage.gameObject.SetActive (false);
		} else {
			goldCount.gameObject.SetActive (true);
			goldImage.gameObject.SetActive (true);
		}
		Invoke ("HidePopUP", 2.0f); 
	}
	public void ShowPop(string text)
	{
		popUpCanvas.SetActive(true);
		txt.gameObject.SetActive(true);
		txt.text = text;
		goldCount.gameObject.SetActive(false);
		goldImage.gameObject.SetActive(false);

		gemCount.gameObject.SetActive(false);
		gemImage.gameObject.SetActive(false);

		Invoke("HidePopUP", 2.0f);
	}

	public void HidePopUP()
	{
		popUpCanvas.SetActive (false);
	}
}
