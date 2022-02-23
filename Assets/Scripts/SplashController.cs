using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SplashController : MonoBehaviour {
	public Text loader;
	public Image loaderImage;
	public GameObject gameSplash;
	bool nextSceneCalled;
	AsyncOperation operation;
	void Start()
	{
		operation = new AsyncOperation ();
	}
	void Update()
	{
		if (nextSceneCalled) {
			if (operation != null) {
				
				loaderImage.fillAmount = operation.progress;
			}
		}
	}
	public void showSplash()
	{
		
		gameSplash.SetActive (true);
		#if UNITY_EDITOR
		LoadVideoIfCopied();
		#endif


	}
	public void LoadVideoIfCopied()
	{
		StartCoroutine (LoadVideo ());
	}
	IEnumerator LoadVideo()
	{
		yield return new WaitForSeconds (1.7f);
		//if (PlayerPrefs.GetInt ("PlayerVideoForOneTIme") == 0) {
		//	Handheld.PlayFullScreenMovie ("file://" + Application.persistentDataPath + "/" +  "Intro.mp4", Color.black, FullScreenMovieControlMode.Hidden);  
		//	PlayerPrefs.SetInt ("PlayerVideoForOneTIme", 1);
		//	Debug.Log ("Play Video");
		//	yield return new WaitForSeconds (1);
		//}


		LoadNext ();
		yield break;
	}
	void LoadNext()
	{
		nextSceneCalled = true;
		operation = SceneManager.LoadSceneAsync ("TrainingRoom");

	}


}
