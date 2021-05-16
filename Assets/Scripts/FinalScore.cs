using UnityEngine.UI;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    public Text finalS;

    


    void Update()
    {
        if (PlayerStats11.PlayHP == 0)
        {
            finalS.text = "GAME OVER!\n"+"your score: "+PlayerStats11.Score.ToString();
        }
        
    }
}
