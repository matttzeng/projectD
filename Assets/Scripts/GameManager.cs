using UnityEngine.SceneManagement;
using UnityEngine;
using ProjectD;
using System.IO;
using ProjectDInternal;

public class GameManager : MonoBehaviour
{
    private bool gameEnded = false;
    public float restartDelay = 2f;
    public GameObject GameOverUI;
    public GameObject PlayUI;
    public Transform HighscoreTable;
  
   
    public ItemEntryUI itemEntryUI;

    private void Awake()
    {
        Time.timeScale = 0;
    }
    public void EndGame()
    {
        if(gameEnded== false)
        {
            gameEnded = true;
            GameOverUI .SetActive(true);
            PlayUI.SetActive(false);

            string jsonString = PlayerPrefs.GetString("highscoreTable");
            //Equipment.GetComponent<>
           // weapon highscores = JsonUtility.FromJson<Highscores>(jsonString);

            // HighscoreTable.Find("highscoreEntryTemplate").gameObject.SetActive(true);



            Time.timeScale = 0;
            Debug.Log("GAME OVER");
            //Invoke("restart",restartDelay);

            PlayfabManager pfManager = new PlayfabManager();
            pfManager.SendLeaderboard(WaveSpawner.waveNumber);

;          
        }

    }

    public void Restart()
    {
        Debug.Log("���Urestart");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("test", LoadSceneMode.Single);
        Time.timeScale = 1f;
        GetComponent<WaveSpawner>().RestartSpwan();
        Debug.Log("���s�ͩ�00");
        //DeteleWeapon();
        Debug.Log("�R�����W�Z��");

        string jsonString = PlayerPrefs.GetString("LastWeapon");
        Weapon lastWeapon = JsonUtility.FromJson<Weapon>(jsonString);
        Debug.Log(jsonString);
        Debug.Log(lastWeapon.name);

        itemEntryUI.SetupEquipment(lastWeapon);
        
        Debug.Log("Ū���Z��");


    }

    public void DeteleWeapon()
    {
    GameObject[] weapons =GameObject.FindGameObjectsWithTag("weapon");

        for (int i = 0; i < weapons.Length; i++)
        {
            Destroy(weapons[i].gameObject);
        }

        if (Directory.Exists("Assets/InGameItem/")) { Directory.Delete("Assets/InGameItem/", true); }
        Directory.CreateDirectory("Assets/InGameItem/");
        Debug.Log("�R��InGameItem");
    }
}


