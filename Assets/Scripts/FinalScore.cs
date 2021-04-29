using UnityEngine.UI;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    public Text finalS;

    


    void Update()
    {
        if (PlayerStats.PlayHP == 0)
        {
            finalS.text = "GAME OVER!\n"+"your score: "+PlayerStats.Score.ToString();
        }
        
    }
}
