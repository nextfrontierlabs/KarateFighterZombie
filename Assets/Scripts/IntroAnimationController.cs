using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimationController : MonoBehaviour {
	[Header("For Ice")]

	public GameObject [] buffEfectForPowerOne;

	[Header("For Fire")]
	public GameObject [] buffEfectForPowerTwo;

	public static int iceEffectIndex;
	public static int fireEffectIndex;

	public Transform DevilBuffForLeft;
	public Transform DevilBuffForRight;

	public void BuffIceEffectOn()
	{
		Debug.Log (transform.localEulerAngles.y);
		SetTransformOfDevilBuff ();
			
		buffEfectForPowerOne[iceEffectIndex].SetActive (true);
		Invoke ("BuffIceEffectOff", 5);
	}
	public void BuffIceEffectOff()
	{
		buffEfectForPowerOne[iceEffectIndex].SetActive (false);
		if (iceEffectIndex < buffEfectForPowerOne.Length - 1)
			iceEffectIndex++;
		else
			iceEffectIndex = 0;
	}
	public void BuffFireEffectOn()
	{
		Debug.Log (transform.localEulerAngles.y);
		SetTransformOfDevilBuff ();

		buffEfectForPowerTwo[fireEffectIndex].SetActive (true);
		Invoke ("BuffFireEffectOff", 5);
	}
	public void BuffFireEffectOff()
	{
		buffEfectForPowerTwo[fireEffectIndex].SetActive (false);
		if (fireEffectIndex < buffEfectForPowerTwo.Length - 1)
			fireEffectIndex++;
		else
			fireEffectIndex = 0;
	}


	public	void SetTransformOfDevilBuff()
	{
        if (transform.localEulerAngles.y == 90)
        {
            buffEfectForPowerOne[4].transform.localEulerAngles = DevilBuffForLeft.localEulerAngles;
            buffEfectForPowerTwo[2].transform.localEulerAngles = DevilBuffForLeft.localEulerAngles;
            buffEfectForPowerOne[4].transform.localPosition = DevilBuffForLeft.localPosition;
            buffEfectForPowerTwo[2].transform.localPosition = DevilBuffForLeft.localPosition;


        }
        else if (transform.localEulerAngles.y == 270)
        {
            buffEfectForPowerOne[4].transform.localEulerAngles = DevilBuffForRight.localEulerAngles;
            buffEfectForPowerTwo[2].transform.localEulerAngles = DevilBuffForRight.localEulerAngles;
            buffEfectForPowerOne[4].transform.localPosition = DevilBuffForRight.localPosition;
            buffEfectForPowerTwo[2].transform.localPosition = DevilBuffForRight.localPosition;
        }
    }
}
