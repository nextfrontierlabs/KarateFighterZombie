using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour {

	private static LeaderBoardManager leaderBoard;
	void Awake()
	{
		leaderBoard = this;
	}
	public static LeaderBoardManager GetInstance()
	{
		return leaderBoard;
	}

	public void SetUserName(string name)
	{
		
		PlayerPrefs.SetString ("Name",name);
		PlayerPrefs.Save ();
	}
	public void SetEmailAddress(string email)
	{
		PlayerPrefs.SetString ("Email",email);
		PlayerPrefs.Save ();
	}
	public string GetUserName()
	{
		return PlayerPrefs.GetString ("Name");

	}
	public string GetEmailAddress( )
	{

	   return	PlayerPrefs.GetString ("Email");

	}

}
