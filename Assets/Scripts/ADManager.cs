using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using ProjectDInternal;
using ProjectD;
using UnityEngine.UI;

public class ADManager : MonoBehaviour,IUnityAdsListener

{

    string GooglePlay_ID = "4181603";
    bool TestMode = true;
    public string mySurfacingId = "Rewarded_Android";
    public string placementId = "bannerPlacement";



    public bool adStarted;
    public bool adCompleted;


    public GameObject UISystem;
    public GameObject statsMenu;
    public GameObject skillMenu;

    public Toggle LoopToggle;
  
   
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(GooglePlay_ID,TestMode);

      
        StartCoroutine(ShowBannerWhenReady());
        
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
        if(statsMenu.activeSelf ==true ^ skillMenu.activeSelf ==true)
        
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
        if (skillMenu.activeSelf == true)
           
            UISystem.GetComponent<UISystem>().ResetSkill();
        else if (statsMenu.activeSelf == true)
            UISystem.GetComponent<UISystem>().StatsReset();
        else
            UISystem.GetComponent<UISystem>().Potion();
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
    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(placementId))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(placementId);
    }
    public void ShowBanner(bool bannerOn)
    {

        bannerOn = LoopToggle.isOn;
    
        if (bannerOn)
        {
            Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);

            Advertisement.Banner.Show();
           
        }

        else
        {
            Advertisement.Banner.Hide();
       
        }
           
    }
}
