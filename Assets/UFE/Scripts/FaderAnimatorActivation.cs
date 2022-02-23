using UnityEngine;
using System.Collections;

public class FaderAnimatorActivation : MonoBehaviour {

	public GameObject MyFaderGO;
	public float TimerOfEnable = 4.0f;
	void OnEnable(){
		Invoke ("EnableFader", TimerOfEnable);
	}

	// Use this for initialization
	void Start () {
	
	}
	


	void EnableFader(){
		MyFaderGO.SetActive (true);
	}
}
