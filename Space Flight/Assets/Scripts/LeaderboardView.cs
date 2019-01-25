using UnityEngine;
using System.Collections;


/// <summary>
/// Leaderboard view.
/// Auther : Nikunj Rola
/// </summary>

public class LeaderboardView : MonoBehaviour {

	#region PRIVATE_VARIABLES

	string leaderBoardID = "PROVIDE_YOUR_LEADERBOARD_ID_HERE";

	#endregion

	#region BUTTON_EVENT_HANDLER

	/// <summary>
	/// Raises the login event.
	/// </summary>
	/// <param name="id">Identifier.</param>

	public void OnLogin(string id){
		LeaderboardManager.AuthenticateToGameCenter();
	}

	/// <summary>
	/// Raises the show leaderboard event.
	/// </summary>

	public void OnShowLeaderboard(){
		LeaderboardManager.ShowLeaderboard();
	}

	/// <summary>
	/// Raises the post score event.
	/// </summary>

	public void OnPostScore(){
		LeaderboardManager.ReportScore(100,leaderBoardID);
	}

	#endregion

}
