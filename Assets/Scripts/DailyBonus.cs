using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyBonus : MonoBehaviour {
    public int day;
    public CanvasGroup content;
  
    public GameObject popUp;
    public ClaimState _claimState;
    public GameObject claimedTick;

    [System.Serializable]
    public enum ClaimState
    {
        claimed,
        readyToclaim,
        notReadyToClaim
    }

    public void StateUpdate()
    {
        switch (_claimState)
        {
		case ClaimState.claimed:
              
			claimedTick.SetActive (true);
			content.alpha = 0.35f;

                break;
            case ClaimState.readyToclaim:
               
                claimedTick.SetActive(false);
                content.alpha = 1f;

                break;
            case ClaimState.notReadyToClaim:
              
                claimedTick.SetActive(false);
                content.alpha = 0.35f;

                break;
        }
    }

    public void ShowPopUpView(bool show)
    {
      //  popUp.SetActive(show);
    }
}
