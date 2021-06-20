using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
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
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request.OnSeccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Suceessful Login/Account create!");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/Creating account");
        Debug.Log(error.GenerateErrorReport());
    }

}
