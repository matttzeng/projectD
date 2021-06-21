using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayfabManager : MonoBehaviour
{
    public GameObject nameWindow;
    public GameObject leaderboardWindow;
    public GameObject highscoreEntryTemplate;
    public Transform highscoreEntryContainer;
    public GameObject nameInput;
    //public GameObject nameError;
    string MyPlayfabID;

    GameManager gameManager;
   
    // Start is called before the first frame update
    void Start()
    {
        login();
    }

    void login()
    {
        var request = new LoginWithCustomIDRequest
        {

            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
           
        };
        PlayFabClientAPI.LoginWithCustomID(request,OnSuccess, OnError);
    }



    void OnSuccess(LoginResult result)
    {
        Debug.Log("Suceessful Login/Account create!");

        string name = null;
        if(result.InfoResultPayload.PlayerProfile != null)
        name = result.InfoResultPayload.PlayerProfile.DisplayName;

        if (name != null)
        {
            nameWindow.SetActive(false);
            Debug.Log("already have Name");
            gameManager.Restart();
        }
       
      
    }
    public void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.GetComponent<TMP_InputField>().text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
        leaderboardWindow.SetActive(true);
    }
    void OnError(PlayFabError error)
    {
        //Debug.Log("Error while logging in/Creating account");
        Debug.Log("LeaderboardError");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "PlatformScore",
                  
                    Value = score
                }
                
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);

    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent");
    }
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 15
        };
        PlayFabClientAPI.GetLeaderboard(request,OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {

        foreach(var item in result.Leaderboard)
        {
           

            

            GameObject newRow = Instantiate(highscoreEntryTemplate, highscoreEntryContainer);


           
            newRow.transform.Find("pos").GetComponent<TMP_Text>().text = (item.Position+1).ToString(); 


          
            newRow.transform.Find("score").GetComponent<TMP_Text>().text = item.StatValue.ToString();

            
            newRow.transform.Find("name").GetComponent<TMP_Text>().text =item.DisplayName;

            GetAccountInfoRequest request = new GetAccountInfoRequest();
            PlayFabClientAPI.GetAccountInfo(request, Successs, OnError);


            if (item.PlayFabId == MyPlayfabID )
            {
                newRow.transform.Find("background").gameObject.SetActive(true);
            }
            else
            {
                newRow.transform.Find("background").gameObject.SetActive(false);
            }

            Debug.Log(item.StatValue + "  " + item.PlayFabId + " "+item.DisplayName);

        }




    }


    

    void Successs(GetAccountInfoResult result)
    {

        MyPlayfabID = result.AccountInfo.PlayFabId;
        Debug.Log("ID : " + MyPlayfabID) ;

    }

    



}
