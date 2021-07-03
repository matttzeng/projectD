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
        //¬‡json
        //¶s¿…
        
    }

    public static void SavePlayer()
    {
       //level = player.GetComponent<CharacterControl>().level.currentLevel;
       // player.GetComponent<CharacterData>().Stats.
    }

    public static void SaveWave()
    {
      //  Wave = WaveSpawner.waveNumber;
    }
}
