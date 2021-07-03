using ProjectD;
using UnityEngine;
using ProjectDInternal;
using System;

[Serializable]
public class Save : MonoBehaviour
{
    public GameObject player;
    public UISystem UISystem;

  

    public class SaveData
    {
        public int level;
        public int exp;
        public int HP;
        public int[] skillData;

        public int[] statsData;
        public int wave;
    }

    // Start is called before the first frame update
    void OnApplicationQuit()
    {
        if(player.GetComponent<CharacterControl>().level.currentLevel>=0)
        SavingData();
        return;
        
        //¬‡json
        //¶s¿…
        
    }
    /*
    SkillPointText.text = $"Skill point : {stats.skillPoint}\nSkillAtk : {stats.skill} \nSkillSpeed : {stats.skillSpeed} ";
    PassiveSkillText.text = $"Atk + 5% : {AtkLevel}/10 \nDEF + 5% : {DEFLevel}/10 \nHP + 10%: {HPLevel}/10 \nMoveSpeed +1: {MoveSpeedLevel}/2 ";
    StatsPointText.text = $"LV point : {stats.statsPoint}\n   Atk : {stats.attack} \n   Def : {stats.defense} \n   HP : {stats.health}  ";
    StatsText.text = $"HP :{stats.health} \nAtk : {stats.attack} \nDef : {stats.defense} \nAtkRange : {stats.attackRange} \nAtkSpeed : {stats.attackSpeed} \nMoveSpeed :{stats.moveSpeed} ";
    */
    public  void SavingData()
    {
        SaveData savedata = new SaveData();

        var stats = player.GetComponent<CharacterControl>().Data.Stats.stats;

        savedata.level = player.GetComponent<CharacterControl>().level.currentLevel;



        savedata.exp = player.GetComponent<CharacterControl>().level.experience;

        savedata.HP = player.GetComponent<CharacterControl>().Data.Stats.CurrentHealth;

        savedata.skillData = new int[] { stats.skillPoint, stats.skill, stats.skillSpeed , UISystem .AtkLevel, UISystem .DEFLevel, UISystem .HPLevel, (int)UISystem .MoveSpeedLevel};
       

        savedata.statsData = new int[] { stats.statsPoint, stats.attack, stats.defense, stats.health };
       

        savedata.wave = WaveSpawner.waveNumber;
       


        string json = JsonUtility.ToJson(savedata);
        PlayerPrefs.SetString("SaveData", json);
        Debug.Log("¶s¿…" + json);

        
    
    }

    public void LoadingData()
    {
        //SaveData savedata = new SaveData();
        var stats = player.GetComponent<CharacterControl>().Data.Stats.stats;

        Debug.Log("¨r¿…");
        string json =  PlayerPrefs.GetString("SaveData");
        Debug.Log("¨r¿…1"+json);
        SaveData loaddata =   JsonUtility.FromJson<SaveData>(json);

        player.GetComponent<CharacterControl>().level.currentLevel = loaddata.level -1;
        player.GetComponent<CharacterControl>().level.experience = loaddata.exp;
        player.GetComponent<CharacterControl>().Data.Stats.CurrentHealth = loaddata.HP;

        stats.skillPoint =          loaddata.skillData[0];
        stats.skill =               loaddata.skillData[1];
        stats.skillSpeed =          loaddata.skillData[2];
        UISystem.AtkLevel =         loaddata.skillData[3];
        UISystem.DEFLevel =         loaddata.skillData[4];
        UISystem.HPLevel =          loaddata.skillData[5];
        UISystem.MoveSpeedLevel =   loaddata.skillData[6];
       

  
        stats.statsPoint =  loaddata.statsData[0];
        stats.attack =      loaddata.statsData[1];
        stats.defense =     loaddata.statsData[2];
        stats.health =      loaddata.statsData[3];

        WaveSpawner.waveNumber = loaddata.wave;

        Debug.Log("¨r¿…2");

    }

   
}
