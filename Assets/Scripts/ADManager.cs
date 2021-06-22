using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using ProjectDInternal;
using ProjectD;

public class ADManager : MonoBehaviour,IUnityAdsListener

{

    string GooglePlay_ID = "4181603";
    bool TestMode = true;
    public string mySurfacingId = "Rewarded_Android";

  
    public bool adStarted;
    public bool adCompleted;


    public GameObject UISystem;
    public GameObject statsMenu;
  
   
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(GooglePlay_ID,TestMode);        
    }

    // Update is called once per frame
 
   
    public void DisplayVideoAD()
    {
        Time.timeScale = 0f;
        Advertisement.Show(mySurfacingId);
      
    }
    public void OnUnityAdsDidError(string message)
    {

    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(statsMenu.activeSelf ==true)
        
        Time.timeScale = 0f;
        else
        Time.timeScale = 1f;

        if (showResult ==ShowResult.Finished)
        {
            Debug.Log("影片完成");
            ADreward();
            
        }
        else if (showResult == ShowResult.Skipped)
        {
            Debug.Log("影片跳過未完成");
        }
        else if(showResult ==ShowResult.Failed)
        {
            Debug.Log("影片未看完");
        }
        adCompleted = showResult == ShowResult.Finished;

     
    }

    public void ADreward()
    {
        if (statsMenu.activeSelf != true)
            UISystem.GetComponent<UISystem>().Potion();
        else
            UISystem.GetComponent<UISystem>().StatsReset();
    }
  public void OnUnityAdsReady (string surfacingId) {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == mySurfacingId) {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

   

    public void OnUnityAdsDidStart (string surfacingId) {
        // Optional actions to take when the end-users triggers an ad.
    } 

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy() {
        Advertisement.RemoveListener(this);
    }
}
