using ProjectD;
using UnityEngine;
using ProjectDInternal;

public class Save : MonoBehaviour
{
    public GameObject player;

    public int level;
    public int SkillPoint;
    public int StatsPoint;
    public int Wave;

    // Start is called before the first frame update
    void OnApplicationQuit()
    {
        SavePlayer();
        SaveWave();
        //轉json
        //存檔
        
    }

    public  void SavePlayer()
    {
       level = player.GetComponent<CharacterControl>().level.currentLevel;
        string json = JsonUtility.ToJson(level);
        PlayerPrefs.SetString("Level", json);

        Debug.Log("存玩家等級" + json);
    }

    public  void SaveWave()
    {
        Wave = WaveSpawner.waveNumber;
        string json = JsonUtility.ToJson(Wave);
        PlayerPrefs.SetString("Wave", json);
        Debug.Log("存關卡" + json);

    }
}
