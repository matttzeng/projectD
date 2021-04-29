
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    
    public Text scoreText;



  
    void Update()
    {


        scoreText.text = PlayerStats.Score.ToString();
       
        
    }
}
