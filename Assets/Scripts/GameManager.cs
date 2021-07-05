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
        Debug.Log("���Urestart");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("test", LoadSceneMode.Single);
        Time.timeScale = 1f;
        GetComponent<WaveSpawner>().RestartSpwan();
        Debug.Log("���s�ͩ�00");
        //DeteleWeapon();
        Debug.Log("�R�����W�Z��");

        //LoadDataFromFile();


       
        /*
        string jsonString = PlayerPrefs.GetString("LastWeapon");
        Weapon lastWeapon = JsonUtility.FromJsonOverwrite(jsonString,obj);
        Debug.Log(lastWeapon.name);

        itemEntryUI.SetupEquipment(lastWeapon);
        
        Debug.Log("Ū���Z��");
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
        Debug.Log("�R��InGameItem");
    }

    public void SaveToFile(Weapon lastWeapon)
    {


        string json = JsonUtility.ToJson(lastWeapon);
        PlayerPrefs.SetString("LastWeapon", json);

        Debug.Log("�s�Z��"+json);
    }


    /*public void LoadDataFromFile()
    {
        Weapon lastWeapon = (Weapon)ScriptableObject.CreateInstance(typeof(Weapon));
      

        string jsonString = PlayerPrefs.GetString("LastWeapon");
        Debug.Log("Ū�Z��1" + jsonString);

        JsonUtility.FromJsonOverwrite(jsonString, lastWeapon); 
       
        Debug.Log("Ū�Z��2" + lastWeapon.ItemName);




        //��˳�   �����O lastWeapon



        EquipmentSystem equipmentSystem = new EquipmentSystem();
        equipmentSystem.Equip(lastWeapon);




    }*/
}


