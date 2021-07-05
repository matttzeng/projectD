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
            if (PlayerPrefs.GetString("highscoreTable") == "")
                HighscoreTable.GetComponent<HighscoreTable>(). DeleteSaves();
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
        Debug.Log("按下restart");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("test", LoadSceneMode.Single);
        Time.timeScale = 1f;
        GetComponent<WaveSpawner>().RestartSpwan();
        Debug.Log("重新生怪00");
        //DeteleWeapon();
        Debug.Log("刪除場上武器");

        //LoadDataFromFile();


       
        /*
        string jsonString = PlayerPrefs.GetString("LastWeapon");
        Weapon lastWeapon = JsonUtility.FromJsonOverwrite(jsonString,obj);
        Debug.Log(lastWeapon.name);

        itemEntryUI.SetupEquipment(lastWeapon);
        
        Debug.Log("讀取武器");
        */

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
        Debug.Log("刪除InGameItem");
    }

    public void SaveToFile(Weapon lastWeapon)
    {


        string json = JsonUtility.ToJson(lastWeapon);
        PlayerPrefs.SetString("LastWeapon", json);

        Debug.Log("存武器"+json);
    }


    /*public void LoadDataFromFile()
    {
        Weapon lastWeapon = (Weapon)ScriptableObject.CreateInstance(typeof(Weapon));
      

        string jsonString = PlayerPrefs.GetString("LastWeapon");
        Debug.Log("讀武器1" + jsonString);

        JsonUtility.FromJsonOverwrite(jsonString, lastWeapon); 
       
        Debug.Log("讀武器2" + lastWeapon.ItemName);




        //穿裝備   物器是 lastWeapon



        EquipmentSystem equipmentSystem = new EquipmentSystem();
        equipmentSystem.Equip(lastWeapon);




    }*/
}


