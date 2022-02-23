using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchLostManager : MonoBehaviour {

	void OnEnable()
	{
		EconomyController.GetInstance().LooseFight(1);
	}

}
