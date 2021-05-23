using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveNumberUI : MonoBehaviour
{
    public Text WaveNumText;
    private int currentWave;
    void Update()
    {
        currentWave = WaveSpawner.waveNumber - 1;

        if(currentWave == 0)
        {
            WaveNumText.text = "START!";
        }
        else
        WaveNumText.text = "Wave "+currentWave.ToString();
    }
}


