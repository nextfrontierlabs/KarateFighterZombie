using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class KAU_LoadingScreenVideo : MonoBehaviour 
{
	int counter = 0;
	public ArrayList assetBundleNames = new ArrayList();

	//public SplashScreen splashObject; 
	public SplashController splashController;
	void Awake()
	{
		StartCopying ();
	}

	public void StartCopying () {
		AssignAssetBundleNames ();
		CopyAssetBundles ((string)assetBundleNames [counter]);
		//Debug.Log (Application.persistentDataPath);
	}

	void CopyAssetBundles (string fileName) 
	{

		Debug.Log ("Copying AssetBundles");
		string bundleFileName = fileName;
		if(!doesFileExist(bundleFileName))
		{
			StartCoroutine(WriteAssetBundles(bundleFileName));
		}
		else
		{
			counter++;
			if(counter < assetBundleNames.Count)
				CopyAssetBundles((string)assetBundleNames[counter]);
			else
			{
				CopyComplete ();
			}
		}
	}
	
	IEnumerator WriteAssetBundles (string fileName) {
		#if UNITY_IPHONE

		string path = Application.persistentDataPath;
		UnityEngine.iOS.Device.SetNoBackupFlag(path);

		#endif

		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);	
		if (filePath.Contains("://")) {
			WWW www = new WWW(filePath);
			yield return www;
			SaveBytesAsFile(Application.persistentDataPath+"/"+fileName, www.bytes);
			Debug.Log("Copied = " + fileName);
		} else {
			SaveBytesAsFile(Application.persistentDataPath+"/"+fileName, System.IO.File.ReadAllBytes(filePath));
			Debug.Log("Copied = " + fileName);
		}
		counter++;
		if(counter < assetBundleNames.Count)
			CopyAssetBundles((string)assetBundleNames[counter]);
		else{
			CopyComplete ();
		}
	}
	
	void SaveBytesAsFile (string _path, byte [] _data) 
	{
		File.WriteAllBytes(_path, _data);
	}
	
	void AssignAssetBundleNames()
	{
		Debug.Log ("Adding Videos");
		assetBundleNames.Add("Intro.mp4");
    }

	public static bool doesFileExist (string fileName) 
	{
		
		FileInfo info = new FileInfo(Application.persistentDataPath+"/"+fileName);
		bool fileExist = true;
		
		if(info == null || !info.Exists) fileExist = false;
		
		return fileExist;
	}
	

	public static void DeleteIfFileExist (string fileName) 
	{
		
		FileInfo info = new FileInfo(Application.persistentDataPath+"/"+fileName);
		
		if(info == null || !info.Exists) return;
		
		info.Delete();
	}

	void CopyComplete()
	{

		Debug.Log ("Copy Complete");
		//Load Next Scene//Video
	
		splashController.LoadVideoIfCopied();
	}
}

