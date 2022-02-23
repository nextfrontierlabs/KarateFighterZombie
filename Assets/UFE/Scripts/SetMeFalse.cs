using UnityEngine;
using System.Collections;

public class SetMeFalse : MonoBehaviour {

	public float timer;

	// Use this for initialization
	void Start () {
	
	}

	void OnEnable(){
		Invoke ("SetMeFalseAfter", timer);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetMeFalseAfter(){
		this.gameObject.SetActive (false);

	}
}
