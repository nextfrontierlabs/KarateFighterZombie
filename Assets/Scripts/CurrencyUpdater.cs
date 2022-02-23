using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CurrencyUpdater : MonoBehaviour {

	public Text gold;
	public Text gems;
	void OnEnable()
	{
		if(gold!=null)
		gold.text = EconomyController.GetInstance ().GetGold ().ToString();
		if(gems!=null)
		gems.text = EconomyController.GetInstance ().GetGems ().ToString();
	}
	public void SetCurrencyInTopBar()
	{
		if(gold!=null)
		gold.text = EconomyController.GetInstance ().GetGold ().ToString();
		if(gems!=null)
		gems.text = EconomyController.GetInstance ().GetGems ().ToString();
	}
}
