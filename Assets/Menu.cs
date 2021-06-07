using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject menuUI;
    public GameObject menuButton;
    public GameObject resumeButton;
    public GameObject restartButton;
    // Update is called once per frame
    private void Start()
    {
        menuButton.GetComponent<Button>().onClick.AddListener(TaskOnClick);
  }
    void Update()
    {
      
       
    }
    void TaskOnClick()
    {
        
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        menuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        Debug.Log("¼È°±");

        menuUI.SetActive(true);
        Debug.Log("¼È°±1");

        Time.timeScale = 0f;
        Debug.Log("¼È°±2");
        GameIsPaused = true;
    }



}
