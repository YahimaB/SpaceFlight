using UnityEngine;
using System.Collections;

/// <summary>
/// Leaderboard manager.
/// Auther : Nikunj Rola
/// </summary>

public class LeaderboardManager : MonoBehaviour 
{
	#region GAME_CENTER	

	/// <summary>
	/// Authenticates to game center.
	/// </summary>

	public static void  AuthenticateToGameCenter()
	{
		#if UNITY_IPHONE
		Social.localUser.Authenticate(success =>
		                              {
			if (success)
			{
				Debug.Log("Authentication successful");
			}
			else
			{
				Debug.Log("Authentication failed");
			}
		});
		#endif
	}

	/// <summary>
	/// Reports the score on leaderboard.
	/// </summary>
	/// <param name="score">Score.</param>
	/// <param name="leaderboardID">Leaderboard I.</param>

	public static void ReportScore(long score, string leaderboardID)
	{
		#if UNITY_IPHONE
		//Debug.Log("Reporting score " + score + " on leaderboard " + leaderboardID);
		Social.ReportScore(score, leaderboardID, success =>
		   {
			if (success)
			{
				Debug.Log("Reported score successfully");
			}
			else
			{
				Debug.Log("Failed to report score");
			}

			Debug.Log(success ? "Reported score successfully" : "Failed to report score"); Debug.Log("New Score:"+score);  
		});
		#endif
	}

	/// <summary>
	/// Shows the leaderboard UI.
	/// </summary>

	public static void ShowLeaderboard()
	{
		#if UNITY_IPHONE
			Social.ShowLeaderboardUI();
		#endif
	}
	#endregion
}