using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RandomAttackSaying : MonoBehaviour {
	public string [] Sayings;
	private Text myText;
	void Awake(){
		myText = GetComponent<Text> ();
	}

	// Use this for initialization
	void Start () {
		myText.text = "" + Sayings [Random.Range (0, Sayings.Length)];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
