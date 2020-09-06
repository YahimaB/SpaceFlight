using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using TouchScript.Gestures;
using System;

public class LinkManager : MonoBehaviour
{
    public GameObject blockPanel;

    public GameObject rewardButton;
    public Text rewardText;

    public string appleId;
    public string androidAppUrl;



    string leaderBoardID = "PROVIDE_YOUR_LEADERBOARD_ID_HERE";

    float coolDown = 20.0f;
    float remainingCoolDown;

    DateTime currentTime = DateTime.Now;

    SaveLoadData saveLoadData = new SaveLoadData();

    private void Awake()
    {
        print("currentTime = " + currentTime);
        AuthenticateToGameCenter();
    }


    private void Update()
    {
        currentTime = DateTime.Now;
        if (PlayerPrefs.HasKey("LastPresentTime"))
        {
            TimeSpan ts = currentTime - Convert.ToDateTime(PlayerPrefs.GetString("LastPresentTime"));
            if (ts.TotalSeconds < coolDown)
            {
                remainingCoolDown = coolDown - (float)ts.TotalSeconds;
            }
            else
            {
                remainingCoolDown = 0.0f;
            }
        }

        if (rewardButton)
        {
            rewardButton.GetComponent<TapGesture>().enabled = IsReady();

            if (rewardButton.GetComponent<TapGesture>().enabled)
            {
                rewardText.text = "Reward ready! Touch to watch video and open";
            }
            else
            {
                if (remainingCoolDown > 0.0f)
                {
                    rewardText.text = "Next reward in: " + (int)(remainingCoolDown) + " seconds";
                }
                else
                {
                    rewardText.text = "Sorry :( \n No video available in your region";
                }
            }
        }
    }

    #region GAME_CENTER    
    public static void AuthenticateToGameCenter()
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

            Debug.Log(success ? "Reported score successfully" : "Failed to report score"); Debug.Log("New Score:" + score);
        });
#endif
    }

    public static void ShowLeaderboard()
    {
#if UNITY_IPHONE
        Social.ShowLeaderboardUI();
#endif
    }
    #endregion

    #region LEADERBOARD_BUTTON_HANDLER

    /// <summary>
    /// Raises the show leaderboard event.
    /// </summary>
    public void OnShowLeaderboard()
    {
        AuthenticateToGameCenter();
        ShowLeaderboard();
    }

    /// <summary>
    /// Raises the post score event.
    /// </summary>
    public void OnPostScore()
    {
        ReportScore(100, leaderBoardID);
    }

    #endregion

    public void OnRateButton()
    {
        MNRateUsPopup rateUs = new MNRateUsPopup("Rate Us", "Are you enjoying \n Space Flight? \n Please rate us! ", "Rate", "No, Thanks", "Later");
        blockPanel.SetActive(true);
        rateUs.SetAppleId(appleId);
        rateUs.SetAndroidAppUrl(androidAppUrl);
        rateUs.AddDeclineListener(() => { Debug.Log("rate us declined"); CloseBlockPanel(); });
        rateUs.AddRemindListener(() => { Debug.Log("remind me later"); CloseBlockPanel(); });
        rateUs.AddRateUsListener(() => { Debug.Log("rate us!!!"); CloseBlockPanel(); });
        rateUs.AddDismissListener(() => { Debug.Log("rate us dialog dismissed :("); CloseBlockPanel(); });
        rateUs.Show();
    }

    void CloseBlockPanel()
    {
        this.blockPanel.SetActive(false);
    }

    public void ShowSuccessMessage()
    {
        MNPopup popup = new MNPopup("Success", "Skin Changed");
        popup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
        popup.Show();
    }

    public void Delete()
    {
        SaveLoadData saveLoadData = new SaveLoadData();
        saveLoadData.DeleteFiles();
    }

    #region AD_MANAGER

    bool IsReady()
    {
        if (remainingCoolDown <= 0.0f)
        {
            //return Advertisement.IsReady("rewardedVideo");
            if (Advertisement.IsReady("rewardedVideo"))
            {
                return true;
            }
            else
            {
                //print("ADVERTISMENT IS NOT READY!!!");
                return false;
            }
        }
        return false;
    }

    public void ShowRewardedAd()
    {
        ShowAds(true);
    }

    public void ShowUsualAd()
    {
        ShowAds(false);
    }

    void ShowAds(bool freeGems)
    {
        if (freeGems)
        {
            ShowOptions options = new ShowOptions();
            options.resultCallback = HandleShowResult;

            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            Advertisement.Show("video");
        }
    }

    void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Video completed - Offer a reward to the player");
                saveLoadData.SaveScore(0, 20);

                if (coolDown > 0f)
                {
                    remainingCoolDown = coolDown;
                    PlayerPrefs.SetString("LastPresentTime", Convert.ToString(currentTime));
                }
                break;

            case ShowResult.Skipped:
                Debug.LogWarning("Video was skipped - Do NOT reward the player");
                break;

            case ShowResult.Failed:
                Debug.LogError("Video failed to show");
                break;
        }
    }
#endregion

    public void SaveLoadTest(){
        saveLoadData.LoadTestData();
    }
}
