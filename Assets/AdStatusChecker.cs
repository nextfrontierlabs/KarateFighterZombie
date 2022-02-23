using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdStatusChecker : MonoBehaviour {

	public Button thisButton;

	void OnEnable()
	{
		thisButton = GetComponent<Button> ();
		if ( AdsManager.GetInstance ().unityRewardedAd.IsLoaded () == false && AdsManager.GetInstance ().admobRewardedAd.isRewardedAdLoaded == false) {
			thisButton.interactable = false;
		} else {
			thisButton.interactable = true;
		}
	}
}
