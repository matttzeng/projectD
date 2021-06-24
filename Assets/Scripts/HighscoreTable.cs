using ProjectD;
using ProjectDInternal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighscoreTable : MonoBehaviour
{

   
    public CharacterControl PlayerCharacter;
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;


    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

       


        AddHighscoreEntry(WaveSpawner.waveNumber, PlayerCharacter.level.currentLevel);


       // AddHighscoreEntry(100000, 100);
        Debug.Log("加入排行榜");

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //sort entry list by score
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    //swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 40f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryRectTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }


        entryTransform.Find("pos").GetComponent<TMP_Text>().text = rankString;


        int score = highscoreEntry.score;
        entryTransform.Find("score").GetComponent<TMP_Text>().text = score.ToString();

        int LV = highscoreEntry.LV;
        entryTransform.Find("LV").GetComponent<TMP_Text>().text = LV.ToString();

        if(score == WaveSpawner.waveNumber && LV == PlayerCharacter.level.currentLevel)
        {
            entryTransform.Find("background").gameObject.SetActive(true);
        }
        else
        {
            entryTransform.Find("background").gameObject.SetActive(false);
        }
       


        transformList.Add(entryTransform);

    }
    private void AddHighscoreEntry(int score, int LV)
    {
        //create highscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, LV = LV };

        //Load Saved HighScores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //add new entry to highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        //save update highscore
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();



    }
    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public int LV;

    }
    
    public void DeleteSaves()
    {
        Debug.Log("清除排行榜");
        PlayerPrefs.DeleteKey("highscoreTable");
        PlayerPrefs.Save();

        highscoreEntryList = new List<HighscoreEntry>()
        {
            new HighscoreEntry{score=0,LV = 0},
            new HighscoreEntry{score=0,LV = 0},
            new HighscoreEntry{score=0,LV = 0}
        };
        string json = JsonUtility.ToJson(highscoreEntryList);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();

    }



    /*
    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        Debug.Log(entryTemplate.name);
        entryTemplate.gameObject.SetActive(false);

        CharacterData data = PlayerCharacter.Data;

        AddHighscoreEntry(WaveSpawner.waveNumber, PlayerCharacter.level.currentLevel);



        Debug.Log("加入排行榜");

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //sort entry list by score
        for(int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for(int j = i+1; j < highscores.highscoreEntryList.Count; j++)
            {
                if(highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    //swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    /*

/**
        Highscores highscores = new Highscores { highscoreEntryList = highscoreEntryList };
        string json = JsonUtility.ToJson(highscoreEntryList);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("highscoreTable"));
**/
    /*
    }
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry,Transform container,List<Transform> transformList)
    {
        float templateHeight = 20f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryRectTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }


        entryTransform.Find("pos").GetComponent<TextMeshPro>().text = rankString;


        int score = highscoreEntry.score;
        entryTransform.Find("score").GetComponent<TextMeshPro>().text = score.ToString();

        int LV = highscoreEntry.LV;
        entryTransform.Find("LV").GetComponent<TextMeshPro>().text = LV.ToString();


        transformList.Add(entryTransform);

    }


    private void AddHighscoreEntry(int score,int LV)
    {
        //create highscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, LV = LV };

        //Load Saved HighScores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //add new entry to highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        //save update highscore
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();



    }
    private  class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }


    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public int LV;

    }


*/

}


